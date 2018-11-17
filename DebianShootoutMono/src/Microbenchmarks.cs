using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Diagnosers;

namespace BenchmarkDebianShootout
{
	public class BenchmarkDebianShootoutConfig : ManualConfig
	{
		void 
		AddRunner (string jobName, string monoPathVal, string monoRuntimePath, string aotArgs, string aotRunArgs)
		{
			var job = Job.ShortRun;

			// Make sure that an empty var for net_4_x won't try to "aot" with no --aot= flag
			if (aotArgs && aotArgs.Length == 0)
				aotArgs = null;

			job = job.With(new MonoRuntime(jobName, monoRuntimePath, aotArgs, monoPathVal));

			if (aotRunArgs != null)
				job = job.With(new[] { new MonoArgument(aotRunArgs) });

			Add(job);
		}

		public BenchmarkDebianShootoutConfig ()
		{
			var monoRuntimePath = System.Environment.GetEnvironmentVariable ("MONO_BENCH_EXECUTABLE");
			var monoPathVal = System.Environment.GetEnvironmentVariable("MONO_BENCH_PATH");
			var aotArgs = System.Environment.GetEnvironmentVariable("MONO_BENCH_AOT_BUILD");
			var aotRunArgs = System.Environment.GetEnvironmentVariable("MONO_BENCH_AOT_RUN");
			var jobName = string.Format("Mono At {0}", monoRuntimePath);

			// Default to use llvm, mono falls back to not where applicable
			Job.Default.With(Jit.Llvm);

			this.AddRunner (jobName, monoPathVal, monoRuntimePath, aotArgs, aotRunArgs);

			Add(DisassemblyDiagnoser.Create(new DisassemblyDiagnoserConfig(printIL: true, printAsm: true, printPrologAndEpilog: true, recursiveDepth: 0)));
		}
	}
}
