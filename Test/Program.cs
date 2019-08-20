using System;
using System.Diagnostics;

namespace SickDev.BinaryStream
{
	class Program
	{
		static int maxNumber = 10000000;
		static int[] numbers = new int[maxNumber + 1];
		static byte[] compressedData;
		static Stopwatch stopwatch = new Stopwatch();

		static void Main(string[] args)
		{
			Random random = new Random(0);
			for (int i = 0; i < numbers.Length; i++)
				numbers[i] = random.Next(ushort.MaxValue);

			Compress();
			Decompress();
		}

		static void Compress()
		{
			BitWriter writter = new BitWriter();
			stopwatch.Start();
			for (int i = 0; i < numbers.Length; i++)
			{
				writter.Write(numbers[i], 32);
				//Console.WriteLine("Write: "+((float)i) / numbers.Length);
			}
			Console.WriteLine(stopwatch.Elapsed.ToString());
			compressedData = writter.GetBytes();

			Console.WriteLine(string.Format("---Initial state---\nNumbers: {0}\nBytes: {1}\n\n---Final state---\nBytes: {2}",
				numbers.Length, numbers.Length * sizeof(int), compressedData.Length));
			Console.WriteLine();
		}

		static void Decompress()
		{
			int[] result = new int[numbers.Length];
			GC.Collect();
			BitReader reader = new BitReader(compressedData);
			stopwatch.Restart();
			for (int i = 0; i < result.Length; i++)
				result[i] = (int)reader.Read(32);
			Console.WriteLine(stopwatch.Elapsed.ToString());


			if (result.Length != numbers.Length)
			{
				Console.WriteLine("Wrong!");
				return;
			}

			bool wrong = false;
			for (int i = 0; i < result.Length; i++)
			{
				if (result[i] != numbers[i])
				{
					wrong = true;
					break;
				}
			}

			if (wrong)
				Console.WriteLine("Wrong!");
			else
				Console.WriteLine("Good!");
		}
	}
}