using System;

namespace SickDev.BinaryCompressor {
    class Program {
        static void Main(string[] args) {
            BinaryWriter writer = new BinaryWriter();
            Random random = new Random();
            int amount = 100;
            for (int i = 0; i < amount; i++)
                writer.Write(random.Next(0, byte.MaxValue));
            Console.WriteLine(string.Format("---Initial state---\nNumbers: {0}\nSize in bytes: {1}\n\n---Final state---\nNumbers: {2}\nSize in bytes: {3}", 
                amount, amount*sizeof(int), writer.GetAllAsLongs().Length, writer.GetAllAsLongs().Length*sizeof(long)));
        }
    }
}
