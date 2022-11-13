namespace ManagedCode.IdGenerator.NewId.NewIdParsers;

public class ZBase32Parser : Base32Parser
{
    private const string ConvertChars = "ybndrfg8ejkmcpqxot1uwisza345h769YBNDRFG8EJKMCPQXOT1UWISZA345H769";

    private const string TransposeChars = "ybndrfg8ejkmcpqx0tlvwis2a345h769YBNDRFG8EJKMCPQX0TLVWIS2A345H769";

    public ZBase32Parser(bool handleTransposedCharacters = false)
        : base(handleTransposedCharacters ? ConvertChars + TransposeChars : ConvertChars)
    {
    }
}