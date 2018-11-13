using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Diagnosers;

namespace BenchmarkDebianShootout
{
	public class BenchmarkDebianShootoutConfig : ManualConfig
	{
		public BenchmarkDebianShootoutConfig ()
		{
			var monoRuntimePath = System.Environment.GetEnvironmentVariable ("MONO_BENCH_EXECUTABLE");
            var monoPathVal = System.Environment.GetEnvironmentVariable("MONO_BENCH_PATH");
            var aot_args = System.Environment.GetEnvironmentVariable("MONO_BENCH_AOT_BUILD");
            var aot_run_args = System.Environment.GetEnvironmentVariable("MONO_BENCH_AOT_RUN");
            var jobName = string.Format("Mono At {0}", monoRuntimePath);

            // Default to use llvm, mono falls back to not where applicable
            Job.Default.With(Jit.Llvm);

            var job = Job.ShortRun;
            job = job.With(new MonoRuntime(jobName, monoRuntimePath, aot_args, monoPathVal));

            if (aot_run_args != null)
                job = job.With(new[] { new MonoArgument(aot_run_args) });

            Add(job);

            // Add(DisassemblyDiagnoser.Create(new DisassemblyDiagnoserConfig(printIL: true, printAsm: true, printPrologAndEpilog: true, recursiveDepth: 0)));
		}
	}
}
