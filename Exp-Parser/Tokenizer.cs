namespace Exp_Parser;

using System.Text.RegularExpressions;

public class Tokenizer
{
    private static readonly (Regex regex, string type)[] TokenList = new (Regex, string)[]
    {
        (new Regex(@"^\s+"), null),                              // Whitespace (ignored)
        (new Regex(@"^\d+(?:\.\d+)?"), "NUMBER"),                // Number (positive int or float) - removed the leading negative
        (new Regex(@"^[a-zA-Z]+"), "IDENT"),                     // Identifier (variable or function)
        (new Regex("^\"[^\"]+\""), "STRING"),                    // Quoted string
        (new Regex(@"^\+"), "+"),                                // Operator +
        (new Regex(@"^-"), "-"),                                 // Operator -
        (new Regex(@"^\*"), "*"),                                // Operator *
        (new Regex(@"^\^"), "^"),                                // Operator ^
        (new Regex(@"^/"), "/"),                                 // Operator /
        (new Regex(@"^\("), "("),                                // Left paren
        (new Regex(@"^\)"), ")"),                                // Right paren
        (new Regex(@"^,"), ","),                                 // Comma
    };
    private static readonly HashSet<string> OpTypes = ["+", "-", "*", "/", "^", "("];
    private Regex _negNum = new Regex(@"^-\d+(?:\.\d+)?");
    public List<Token> Tokenize(string input)
    {
        int pos = 0;
        List<Token> tokens = [];

        while (pos < input.Length)
        {
            bool matchFound = false;
            
            if (ParseNegNum(pos, input, tokens))
            {
                Match match = _negNum.Match(input[pos..]);
    
                if (match.Success)
                {
                    tokens.Add(new Token("NUMBER", match.Value));
                    pos += match.Value.Length;
                    matchFound = true;
                }
            }

            if (!matchFound)
            {
                foreach (var (regex, type) in TokenList)
                {
                    Match match = regex.Match(input[pos..]);
                    if (!match.Success) continue;
                    pos += match.Value.Length;

                    if (type is not null)
                        tokens.Add(new Token(type, match.Value));

                    matchFound = true;
                    break;
                }
            }

            if (!matchFound)
                throw new Exception($"Unexpected character at position {pos}: {input[pos]}");
        }

        return tokens;
    }
    private static bool ParseNegNum(int pos, string input, List<Token> tokens)
    {
       
        return (pos == 0 || tokens.Count == 0 || OpTypes.Contains(tokens[^1].Type)) && 
               pos < input.Length && 
               input[pos] == '-' && 
               pos + 1 < input.Length && 
               char.IsDigit(input[pos + 1]);
    }
    

    

}