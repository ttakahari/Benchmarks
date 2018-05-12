using BenchmarkDotNet.Attributes;
using Benchmarks.Configurations;
using Benchmarks.Utilities;
using System;
using System.IO;
using System.Text;

namespace Benchmarks.Targets
{
    [Config(typeof(BenchmarkConfig))]
    public class JsonSerializers
    {
        private readonly SerializeObject _obj;
        private readonly Newtonsoft.Json.JsonSerializer _jsonNet;

        private readonly string _jsonNetString;
        private readonly string _jilString;
        private readonly string _utf8jsonString;
        private readonly byte[] _jsonNetBytes;
        private readonly byte[] _jilBytes;
        private readonly byte[] _utf8jsonBytes;
        private readonly byte[] _jsonNetStream;
        private readonly byte[] _jilStream;

        public JsonSerializers()
        {
            Utf8Json.JsonSerializer.SetDefaultResolver(Utf8Json.Resolvers.StandardResolver.Default);

            _obj = new SerializeObject
            {
                Foo = 1,
                Bar = "AAA",
                Baz = Baz.One,
                Qux = DateTimeOffset.Now,
            };
            _jsonNet = new Newtonsoft.Json.JsonSerializer();

            _jsonNetString = Newtonsoft.Json.JsonConvert.SerializeObject(_obj);
            _jilString = Jil.JSON.Serialize(_obj);
            _utf8jsonString = Encoding.UTF8.GetString(Utf8Json.JsonSerializer.Serialize(_obj));

            _jsonNetBytes = Encoding.UTF8.GetBytes(_jsonNetString);
            _jilBytes = Encoding.UTF8.GetBytes(_jilString);
            _utf8jsonBytes = Utf8Json.JsonSerializer.Serialize(_obj);

            using (var memory = new MemoryStream())
            using (var writer = new StreamWriter(memory) { AutoFlush = true })
            {
                _jsonNet.Serialize(writer, _obj);

                _jsonNetStream = memory.ToArray();
            }

            using (var memory = new MemoryStream())
            using (var writer = new StreamWriter(memory) { AutoFlush = true })
            {
                Jil.JSON.Serialize(_obj, writer);

                _jilStream = memory.ToArray();
            }
        }

        [Benchmark]
        public void JsonNet_ToString()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(_obj);
        }

        [Benchmark]
        public void Jil_ToString()
        {
            var json = Jil.JSON.Serialize(_obj);
        }

        [Benchmark]
        public void Utf8Json_ToString()
        {
            var json = Encoding.UTF8.GetString(Utf8Json.JsonSerializer.Serialize(_obj));
        }

        [Benchmark]
        public void JsonNet_ToStream()
        {
            using (var memory = new MemoryStream())
            using (var writer = new StreamWriter(memory) { AutoFlush = true })
            {
                _jsonNet.Serialize(writer, _obj);

                var json = memory;
            }
        }

        [Benchmark]
        public void Jil_ToStream()
        {
            using (var memory = new MemoryStream())
            using (var writer = new StreamWriter(memory) { AutoFlush = true })
            {
                Jil.JSON.Serialize(_obj, writer);

                var json = memory;
            }
        }

        [Benchmark]
        public void JsonNet_ToBytes()
        {
            var json = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(_obj));
        }

        [Benchmark]
        public void Jil_ToBytes()
        {
            var json = Encoding.UTF8.GetBytes(Jil.JSON.Serialize(_obj));
        }

        [Benchmark]
        public void Utf8Json_ToBytes()
        {
            var json = Utf8Json.JsonSerializer.Serialize(_obj);
        }

        [Benchmark]
        public void JsonNet_FromString()
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializeObject>(_jsonNetString);
        }

        [Benchmark]
        public void Jil_FromString()
        {
            var obj = Jil.JSON.Deserialize<SerializeObject>(_jilString);
        }

        [Benchmark]
        public void Utf8Json_FromString()
        {
            var obj = Utf8Json.JsonSerializer.Deserialize<SerializeObject>(Encoding.UTF8.GetBytes(_utf8jsonString));
        }

        [Benchmark]
        public void JsonNet_FromBytes()
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializeObject>(Encoding.UTF8.GetString(_jsonNetBytes));
        }

        [Benchmark]
        public void Jil_FromBytes()
        {
            var obj = Jil.JSON.Deserialize<SerializeObject>(Encoding.UTF8.GetString(_jilBytes));
        }

        [Benchmark]
        public void Utf8Json_FromBytes()
        {
            var obj = Utf8Json.JsonSerializer.Deserialize<SerializeObject>(_utf8jsonBytes);
        }

        [Benchmark]
        public void JsonNet_FromStream()
        {
            using (var memory = new MemoryStream(_jsonNetStream))
            using (var reader = new StreamReader(memory))
            {
                var obj = (SerializeObject)_jsonNet.Deserialize(reader, typeof(SerializeObject));
            }
        }

        [Benchmark]
        public void Jil_FromStream()
        {
            using (var memory = new MemoryStream(_jilStream))
            using (var reader = new StreamReader(memory))
            {
                var obj = (SerializeObject)Jil.JSON.Deserialize(reader, typeof(SerializeObject));
            }
        }
    }
}
