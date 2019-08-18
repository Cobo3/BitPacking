using System;

namespace SickDev.BinaryStream
{
	class Program
	{
		static int maxNumber = byte.MaxValue;
		static int[] numbers = new int[maxNumber + 1];
		static byte[] compressedData;

		static void Main(string[] args)
		{
			Random random = new Random(0);
			for (int i = 0; i < numbers.Length; i++)
				numbers[i] = i;// random.Next(maxNumber);

			//Peta
			//numbers = new int[] {
			//	1, 1
			//};

			//for (int i = 0; i < numbers.Length; i++)
			//	Console.WriteLine(i + ": " + numbers[i]);

			Compress();
			Decompress();
		}

		static void Compress()
		{
			BitWriter writter = new BitWriter();
			for (int i = 0; i < numbers.Length; i++)
			{
				writter.Write(numbers[i], 8);
			}
			compressedData = writter.GetBytes();

			Console.WriteLine(string.Format("---Initial state---\nNumbers: {0}\nBytes: {1}\n\n---Final state---\nBytes: {2}",
				numbers.Length, numbers.Length * sizeof(int), compressedData.Length));
			Console.WriteLine();
		}

		static void Decompress()
		{
			int[] result = new int[numbers.Length];
			BitReader reader = new BitReader(compressedData);
			for (int i = 0; i < result.Length; i++)
			{
				BinaryNumber value = reader.Read(8);
				result[i] = value;
			}


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
		}
	}
}