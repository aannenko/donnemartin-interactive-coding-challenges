using System.Diagnostics;
using System.Text;

Debug.Assert(Compress("AABBCC") is "AABBCC");
Debug.Assert(Compress("AAABCCDDDDE") is "A3BC2D4E");
Debug.Assert(Compress("BAAACCDDDD") is "BA3C2D4");
Debug.Assert(Compress("AAABAACCDDDD") is "A3BA2C2D4");

static string Compress(string text)
{
    if (string.IsNullOrEmpty(text))
        return text;

    var builder = new StringBuilder(text.Length);
    var prevChar = text[0];
    var count = 1;
    for (int i = 1; i < text.Length; i++)
    {
        if (prevChar == text[i])
        {
            count++;
            continue;
        }

        AppendCharAndCount(builder, prevChar, count);
        prevChar = text[i];
        count = 1;
    }

    AppendCharAndCount(builder, prevChar, count);
    return builder.Length < text.Length ? builder.ToString() : text;
}

static void AppendCharAndCount(StringBuilder builder, char c, int count)
{
    builder.Append(c);
    if (count > 1)
        builder.Append(count);
}