using System;

namespace BinaryStream {
    class Program {
        static void Main(string[] args) {
            BinaryWriter writer = new BinaryWriter();
            writer.Write(300);
            writer.Write(1900);
            writer.Write(0);
            writer.Write(0);
            writer.Write(12);
        }
    }
}
