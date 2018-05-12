using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

namespace Benchmarks.Configurations
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(MemoryDiagnoser.Default);

            Add(new Job(BenchmarkDotNet.Jobs.RunMode.Short, EnvMode.Clr));
            Add(new Job(BenchmarkDotNet.Jobs.RunMode.Short, EnvMode.Core));
        }
    }
}
