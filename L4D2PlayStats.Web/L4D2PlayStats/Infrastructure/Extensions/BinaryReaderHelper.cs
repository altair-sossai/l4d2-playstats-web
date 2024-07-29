using System.Text;

namespace L4D2PlayStats.Infrastructure.Extensions;

public static class BinaryReaderHelper
{
    public static string ReadNullTerminatedString(this BinaryReader binaryReader)
    {
        var stringBuilder = new StringBuilder();
        var value = binaryReader.ReadChar();

        while (value != '\x00')
        {
            stringBuilder.Append(value);
            value = binaryReader.ReadChar();
        }

        return stringBuilder.ToString();
    }
}