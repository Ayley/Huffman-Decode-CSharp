using System.Diagnostics;

namespace HuffmanDecoder.Util;

public class Struct
{
    public static long[] Unpack(String format, byte[] values)
    {
        int size = ClacSize(format);

        if (size != values.Length)
            throw new Exception("Format lenght and values not equal");

        var result = new long[format.Length];

        var bufferReader = new MemoryStream(values);

        for (var i = 0; i < format.Length; i++)
        {
            var buffer = new byte[ClacSize(format[i].ToString())];

            var read = bufferReader.Read(buffer);

            result[i] = UnpackSingleData(format[i], buffer);
        }

        return result;
    }

    private static long UnpackSingleData(char format, byte[] values)
    {
        var ms = new MemoryStream(values);
        
        return format switch
        {
            'I' => new BinaryReader(ms).ReadUInt32(),
            'c' or 'x' => new BinaryReader(ms).Read(),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    public static int ClacSize(String format)
    {
        return format.ToCharArray()
            .Sum(c => c switch
            {
                'I' => 4,
                'c' or 'x' => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
            });
    }
}