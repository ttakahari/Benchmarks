using BenchmarkDotNet.Attributes;
using Benchmarks.Configurations;
using LZ4;
using System.IO;
using System.IO.Compression;
using ZstdNet;

namespace Benchmarks.Targets
{
    [Config(typeof(BenchmarkConfig))]
    public class Compressors
    {
        private readonly byte[] _source;
        private readonly byte[] _gzip;
        private readonly byte[] _lz4;
        private readonly byte[] _zstd;

        public Compressors()
        {
            _source = new byte[1024 * 10];

            using (var memory = new MemoryStream())
            using (var gzip = new GZipStream(memory, CompressionLevel.Fastest))
            {
                gzip.Write(_source, 0, _source.Length);

                _gzip = memory.ToArray();
            }

            using (var memory = new MemoryStream())
            using (var lz4 = new LZ4Stream(memory, LZ4StreamMode.Compress))
            {
                lz4.Write(_source, 0, _source.Length);

                _lz4 = memory.ToArray();
            }

            using (var zstd = new Compressor())
            {
                _zstd = zstd.Wrap(_source);
            }
        }

        [Benchmark]
        public void GZip_Compress()
        {
            using (var memory = new MemoryStream())
            using (var gzip = new GZipStream(memory, CompressionMode.Compress))
            {
                gzip.Write(_source, 0, _source.Length);
            }
        }

        [Benchmark]
        public void LZ4_Compress()
        {
            using (var memory = new MemoryStream())
            using (var lz4 = new LZ4Stream(memory, LZ4StreamMode.Compress))
            {
                lz4.Write(_source, 0, _source.Length);
            }
        }

        [Benchmark]
        public void Zstd_Compress()
        {
            using (var zstd = new Compressor())
            {
                var zipped = zstd.Wrap(_source);
            }
        }

        [Benchmark]
        public void GZip_Decompress()
        {
            var buffer = new byte[1024];

            using (var memory = new MemoryStream(_gzip))
            using (var gzip = new GZipStream(memory, CompressionMode.Decompress))
            {
                while (true)
                {
                    var size = gzip.Read(buffer, 0, buffer.Length);

                    if (size == 0)
                    {
                        break;
                    }

                    memory.Write(buffer, 0, size);
                }
            }
        }

        [Benchmark]
        public void LZ4_Decompress()
        {
            var buffer = new byte[1024];

            using (var memory = new MemoryStream(_lz4))
            using (var lz4 = new LZ4Stream(memory, LZ4StreamMode.Decompress))
            {
                while (true)
                {
                    var size = lz4.Read(buffer, 0, buffer.Length);

                    if (size == 0)
                    {
                        break;
                    }

                    memory.Write(buffer, 0, size);
                }
            }
        }

        [Benchmark]
        public void Zstd_Decompress()
        {
            using (var zstd = new Decompressor())
            {
                var unzipped = zstd.Unwrap(_zstd);
            }
        }
    }
}
