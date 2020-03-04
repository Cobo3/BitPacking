# BitPacking

Bit packing is a compression technique in which unnecessary bits are removed from the data we want to compress.

As an example, say you want to serialize someone's age. You would most likely represent that piece of data with an integer structure; say, 32 bits. However, because the range of that data is known to be between {0, 100} (for the purpose of this example), you only need 7 bits at most to represent it. 

Even if you initially represented the data as a single byte (8 bits), if you wanted to serialize the age of 100 different people, using this techinque, all that data would only take 700 bits (88 bytes) instead of 800 bits (100 bytes).

This library provides an easy to use API so that you don't have to worry about all the math involved in the process.

## Usage
The *BitWriter* class packs the data we want to compress.

```csharp
using SickDev.BitPacking;

...

//The data we want to serialize
byte[] ages = new byte[100];
for (int i = 0; i < ages.Length; i++)
    ages[i] = (byte) random.Next(0, 101);

//Pack the data, specifying that only the first 7 bits matter
BitWriter writer = new BitWriter();
for (int i = 0; i < ages.Length; i++)
    writer.Write(ages[i], 7);
```

Once all the data is written, we can get the compressed version.
```csharp
//Get the compressed version of the data
byte[] compressedAges = writer.GetBytes();

//Non compressed data size: 100, compressed data size: 88
Console.WriteLine($"Non compressed data size: {ages.Length}, " +
    $"compressed data size: {compressedAges.Length}");
```

On the other hand, we use *BitReader* to read from the compressed data.
```csharp
byte[] uncompressedData = new byte[100];

//Unpack the data, reading only 7 bits at a time
BitReader reader = new BitReader(compressedAges);
for (int i = 0; i < uncompressedData.Length; i++)
    uncompressedData[i] = reader.Read(7);

//Test passed!
Assert.AreEqual(ages, uncompressedData);
```

#### Neagtive Numbers
Negative numebrs are not currently supported. However, it is fairly simple to work around that. When you need to know whether some data is positive or negative, simply write an extra sign indicating the sign.

```csharp
//The data we want to serialize
int[] data = new int[10];
for (int i = 0; i < data.Length; i++)
    data[i] = random.Next(-1024, 1025);

BitWriter writer = new BitWriter();
for (int i = 0; i < data.Length; i++)
{
    //When packing, write the absolute value of the data
    writer.Write(Math.Abs(data[i]), 10);
    //After that, write 1 bit indicating the sign of the data
    writer.Write(data[i] < 0 ? 0 : 1, 1);
}

byte[] compressedData = writer.GetBytes();

int[] uncompressedData = new int[10];

BitReader reader = new BitReader(compressedData);
for (int i = 0; i < uncompressedData.Length; i++)
{
    uncompressedData[i] = reader.Read(10);
    //When unpacking, if the next bit is a 0, multiply by -1
    if (reader.Read(1) == 0)
        uncompressedData[i] *= -1;
}

//Test passed!
Assert.AreEqual(data, uncompressedData);
```

### Performance
The class _BinaryNumber_ provides a cool binary string representation of any numeric type; which works wonders for debugging. As such, the library is made to use them when built in Debug configuration; however, that vastly reduces performance. In a production scenario, it is advised to build the library in Release configuration.

## Dependencies

The _BitPacking.Tests_ project has the following dependencies:
- [NUnit 3.12.0](https://www.nuget.org/packages/NUnit/3.12.0)
- [NUnit3TestAdapter 3.16.1](https://www.nuget.org/packages/NUnit3TestAdapter/3.16.1)

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](/blob/master/LICENSE.md)
