using System.Collections;
using HuffmanDecoder.Objects;
using HuffmanDecoder.Util;

namespace HuffmanDecoder;

public class HuffmanDecoder
{
    public static string Decode(byte[] input)
    {
        return UnpackFile(new MemoryStream(input));
    }

    public static string UnpackFile(MemoryStream memoryStream)
    {
        var frequencies = Frequencies(memoryStream);

        var minHeap = CreateTree(frequencies);

        Console.WriteLine(minHeap.RootNode().Character);
        
        var packets = Read("III", memoryStream);

        var buffer = new byte[packets[1]];

        var read = memoryStream.Read(buffer);

        return DecodeHuffman(minHeap, buffer, packets[0]);
    }

    private static string DecodeHuffman(MinHeap minHeap, byte[] buffer, long packet)
    {
        var result = "";

        var bitArray = CreateBitArray(buffer);

        var pos = 0;

        while (pos < packet)
        {
            var node = minHeap.RootNode();

            while (true)
            {
                if (pos >= bitArray.Count) throw new Exception("Invalid Data: pos");

                if (bitArray.Get(pos))
                    node = node?.Right;
                else
                    node = node?.Left;

                pos++;

                if (node == null)
                    throw new Exception("Invalid Data: Node");
                
                if(node.Left == null && node.Right == null)
                    break;
            }

            result += node.Character;
        }
        
        return result;
    }

    private static BitArray CreateBitArray(byte[] buffer)
    {
        var bitArray = new BitArray(buffer);

        var binaryString = "";

        foreach (var b in buffer)
            binaryString += Convert.ToString(b & 255 | 256, 2).Substring(1);
        
        for (int i = 0; i < binaryString.Length; i++)
        {
            if(binaryString[i] == '1')
                bitArray[i] = true;
            else
                bitArray[i] = false;
            
        }

        return bitArray;
    }

    private static MinHeap CreateTree(LinkedList<Frequency> frequencies)
    {
        var minHeap = new MinHeap();

        foreach (var frequency in frequencies)
            minHeap.Push(new Node(frequency.Character.ToString(), frequency.Frequent, null, null));

        while (minHeap.Size() > 1)
        {
            var a = minHeap.Pop();
            var b = minHeap.Pop();

            var parent = new Node(a.Character + b.Character, a.Frequency + b.Frequency, a, b);
            
            minHeap.Push(parent);
        }
        
        return minHeap;
    }

    private static LinkedList<Frequency> Frequencies(MemoryStream memoryStream)
    {
        var count = Read("III", memoryStream)[2];
        
        var result = new LinkedList<Frequency>();
        
        for (int i = 0; i < count; i++)
        {
            var frequency = Read("I", memoryStream)[0];
            var character = Read("cxxx", memoryStream)[0];
            
            result.AddLast(new Frequency((char) character, frequency));
        }

        return result;
    }

    private static long[] Read(string format, MemoryStream ms)
    {
        var size = Struct.ClacSize(format);
        
        var buffer = new byte[size];

        var read = ms.Read(buffer, 0, buffer.Length);
        
        return Struct.Unpack(format, buffer);
    }
}