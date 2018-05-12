using BenchmarkDotNet.Running;
using Benchmarks.Targets;
using System;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<JsonSerializers>();
            BenchmarkRunner.Run<Compressors>();

            Console.ReadLine();
        }
    }
}
