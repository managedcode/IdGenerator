namespace ManagedCode.IdGenerator.NewId.NewIdFormatters;

public class ZBase32Formatter : Base32Formatter
{
    // taken from analysis done at http://philzimmermann.com/docs/human-oriented-base-32-encoding.txt
    private const string LowerCaseChars = "ybndrfg8ejkmcpqxot1uwisza345h769";
    private const string UpperCaseChars = "YBNDRFG8EJKMCPQXOT1UWISZA345H769";

    public ZBase32Formatter(bool upperCase = false)
        : base(upperCase ? UpperCaseChars : LowerCaseChars)
    {
    }
}