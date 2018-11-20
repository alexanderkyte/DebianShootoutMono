
/*
  The Computer Language Benchmarks Game
  https://salsa.debian.org/benchmarksgame-team/benchmarksgame/

  contributed by Marek Safar
  optimized by kasthack
  *reset*
*/

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;

namespace BenchmarkDebianShootout
{
	public class RevCompSequence { public List<byte[]> Pages; public int StartHeader, EndExclusive; public Thread ReverseThread; }

	[Config(typeof(BenchmarkDebianShootoutConfig))]
	public class RevComp
	{
		[Benchmark]
		public void Run ()
		{
			// Need object reference,
			// otherwise we hide the initialization time of collections and stuff behind
			// the static class init
			new RevCompContainer ();
		}
	}

	public class RevCompContainer
	{
		const int READER_BUFFER_SIZE = 1024 * 1024;
		const byte LF = 10, GT = (byte)'>', SP = 32;

		public BlockingCollection<byte[]> readQue;
		public BlockingCollection<RevCompSequence> writeQue;
		public byte[] map;

		Thread readerThread;
		Thread grouperThread;

		public RevCompContainer ()
		{
			readQue = new BlockingCollection<byte[]> ();
			writeQue = new BlockingCollection<RevCompSequence> ();

			readerThread = new Thread (this.Reader);
			grouperThread = new Thread (this.Grouper);

			readerThread.Start ();
			grouperThread.Start ();

			Writer ();
		}

		static int read(Stream stream, byte[] buffer, int offset, int count)
		{
			var bytesRead = stream.Read(buffer, offset, count);
			return bytesRead==count ? offset+count
				 : bytesRead==0 ? offset
				 : read(stream, buffer, offset+bytesRead, count-bytesRead);
		}

		public void Reader ()
		{
			using (var stream = File.Open (System.Environment.GetEnvironmentVariable ("MONO_BENCH_INPUT"), FileMode.Open))
			{
				int bytesRead;
				do
				{
					var buffer = new byte[READER_BUFFER_SIZE];
					bytesRead = read(stream, buffer, 0, READER_BUFFER_SIZE);
					this.readQue.Add(buffer);
				} while(bytesRead==READER_BUFFER_SIZE);
				this.readQue.CompleteAdding();
			}
		}
	
		static bool tryTake<T>(BlockingCollection<T> q, out T t) where T : class
		{
			t = null;
			while(!q.IsCompleted && !q.TryTake(out t)) Thread.SpinWait(0);
			return t!=null;
		}
	
		public void Grouper ()
		{
			// Set up complements map
			this.map = new byte[256];
			for (byte b=0; b<255; b++) this.map[b]=b;
			this.map[(byte)'A'] = (byte)'T';
			this.map[(byte)'B'] = (byte)'V';
			this.map[(byte)'C'] = (byte)'G';
			this.map[(byte)'D'] = (byte)'H';
			this.map[(byte)'G'] = (byte)'C';
			this.map[(byte)'H'] = (byte)'D';
			this.map[(byte)'K'] = (byte)'M';
			this.map[(byte)'M'] = (byte)'K';
			this.map[(byte)'R'] = (byte)'Y';
			this.map[(byte)'T'] = (byte)'A';
			this.map[(byte)'V'] = (byte)'B';
			this.map[(byte)'Y'] = (byte)'R';
			this.map[(byte)'a'] = (byte)'T';
			this.map[(byte)'b'] = (byte)'V';
			this.map[(byte)'c'] = (byte)'G';
			this.map[(byte)'d'] = (byte)'H';
			this.map[(byte)'g'] = (byte)'C';
			this.map[(byte)'h'] = (byte)'D';
			this.map[(byte)'k'] = (byte)'M';
			this.map[(byte)'m'] = (byte)'K';
			this.map[(byte)'r'] = (byte)'Y';
			this.map[(byte)'t'] = (byte)'A';
			this.map[(byte)'v'] = (byte)'B';
			this.map[(byte)'y'] = (byte)'R';
	
			var startHeader = 0;
			var i = 0;
			bool afterFirst = false;
			var data = new List<byte[]>();
			byte[] bytes;
			while (tryTake(this.readQue, out bytes))
			{
				data.Add(bytes);
				while((i=Array.IndexOf<byte>(bytes, GT, i+1))!=-1)
				{
					var sequence = new RevCompSequence { Pages = data
						, StartHeader = startHeader, EndExclusive = i };
					if(afterFirst)
						(sequence.ReverseThread = new Thread(() => Reverse(sequence))).Start();
					else
						afterFirst = true;
					this.writeQue.Add(sequence);
					startHeader = i;
					data = new List<byte[]> { bytes };
				}
			}
			i = Array.IndexOf<byte>(data[data.Count-1],0,0);
			var lastSequence = new RevCompSequence { Pages = data
				, StartHeader = startHeader, EndExclusive = i==-1 ? data[data.Count-1].Length : i };
			Reverse(lastSequence);
			this.writeQue.Add(lastSequence);
			this.writeQue.CompleteAdding();
		}
	
		void Reverse(RevCompSequence sequence)
		{
			var startPageId = 0;
			var startBytes = sequence.Pages[0];
			var startIndex = sequence.StartHeader;
	
			// Skip header line
			while((startIndex=Array.IndexOf<byte>(startBytes, LF, startIndex))==-1)
			{
				startBytes = sequence.Pages[++startPageId];
				startIndex = 0;
			}
	
			var endPageId = sequence.Pages.Count - 1;
			var endIndex = sequence.EndExclusive - 1;
			if(endIndex==-1) endIndex = sequence.Pages[--endPageId].Length-1;
			var endBytes = sequence.Pages[endPageId];
	
			// Swap in place across pages
			do
			{
				var startByte = startBytes[startIndex];
				if(startByte<SP)
				{
					if (++startIndex == startBytes.Length)
					{
						startBytes = sequence.Pages[++startPageId];
						startIndex = 0;
					}
					if (startIndex == endIndex && startPageId == endPageId) break;
					startByte = startBytes[startIndex];
				}
				var endByte = endBytes[endIndex];
				if(endByte<SP)
				{
					if (--endIndex == -1)
					{
						endBytes = sequence.Pages[--endPageId];
						endIndex = endBytes.Length - 1;
					}
					if (startIndex == endIndex && startPageId == endPageId) break;
					endByte = endBytes[endIndex];
				}
	
				startBytes[startIndex] = map[endByte];
				endBytes[endIndex] = map[startByte];
	
				if (++startIndex == startBytes.Length)
				{
					startBytes = sequence.Pages[++startPageId];
					startIndex = 0;
				}
				if (--endIndex == -1)
				{
					endBytes = sequence.Pages[--endPageId];
					endIndex = endBytes.Length - 1;
				}
			} while (startPageId < endPageId || (startPageId == endPageId && startIndex < endIndex));
			if (startIndex == endIndex) startBytes[startIndex] = map[startBytes[startIndex]];
		}
	
		void Writer ()
		{
			var outName = String.Format("{0}.out", System.Environment.GetEnvironmentVariable ("MONO_BENCH_INPUT"));

			using (var stream = File.Open (outName, FileMode.OpenOrCreate))
			{
				bool first = true;
				RevCompSequence sequence;
				while (tryTake(this.writeQue, out sequence))
				{
					var startIndex = sequence.StartHeader;
					var pages = sequence.Pages;
					if(first)
					{
						Reverse(sequence);
						first = false;
					}
					else
					{
						sequence.ReverseThread?.Join();
					}
					for (int i = 0; i < pages.Count - 1; i++)
					{
						var bytes = pages[i];
						stream.Write(bytes, startIndex, bytes.Length - startIndex);
						startIndex = 0;
					}
					stream.Write(pages[pages.Count-1], startIndex, sequence.EndExclusive - startIndex);
				}
			}
		}
	}
}


