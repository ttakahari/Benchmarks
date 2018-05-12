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

            Add(Job.ShortRun, Job.Core);
            Add(Job.ShortRun, Job.Clr);
        }
    }
}
