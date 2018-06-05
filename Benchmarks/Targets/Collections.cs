using BenchmarkDotNet.Attributes;
using Benchmarks.Configurations;
using System.Collections.Generic;
using System.Linq;

namespace Benchmarks.Targets
{
    [Config(typeof(BenchmarkConfig))]
    public class Collections
    {
        private readonly IEnumerable<int> _source;
        private readonly int[] _array;
        private readonly List<int> _list;

        public Collections()
        {
            _source = Enumerable.Range(0, 100).Select(x => x);
            _array = _source.ToArray();
            _list = _source.ToList();
        }

        [Benchmark]
        public void Array_For()
        {
            for (var i = 0; i < _array.Length; i++)
            {
                var item = _array[i];
            }
        }

        [Benchmark]
        public void List_For()
        {
            for (var i = 0; i < _list.Count; i++)
            {
                var item = _list[i];
            }
        }

        [Benchmark]
        public void Enumerable_ForEach()
        {
            foreach (var item in _source) { }
        }

        [Benchmark]
        public void Array_ForEach()
        {
            foreach (var item in _array) { }
        }

        [Benchmark]
        public void List_ForEach()
        {
            foreach (var item in _list) { }
        }

        [Benchmark]
        public void ToArray()
        {
            var array = _source.ToArray();
        }

        [Benchmark]
        public void ToList()
        {
            var list = _source.ToList();
        }
    }
}
