using System.Text.RegularExpressions;
using Exp_Parser.Model.Tokens;

namespace Exp_Parser.Engine;
using Model;
public class Tokenizer: IExpTokenizer
{
    private readonly Regex VariableRegex = new Regex(@"^[x]", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    private readonly Regex WhiteSpaceRegex = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    private readonly Regex FunctionRegex = new Regex(@"^[\w]*", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    private readonly Regex DecimalRegex = new Regex(@"^((\d*\.\d+)|(\d+\.\d*))", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    private readonly Regex DigitRegex = new Regex(@"^\d+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    public TokenList Tokenize(string input) 
    {
        if (string.IsNullOrWhiteSpace(input)) throw new Exception();
        
        TokenList result = [];
        
        string trimmed = WhiteSpaceRegex.Replace(input, string.Empty);
        
        for (int characterPosition = 0; characterPosition < trimmed.Length;)
            if (!IsValidToken(trimmed,result, ref characterPosition)) 
                throw new ArgumentException($"Invalid token at position {characterPosition + 1}.", nameof(input));
        return result;
    }
    private bool IsValidToken(string input,TokenList result, ref int position) => 
        IsNumber(input,ref position,result) || IsSymbol(input[position].ToString(), ref position,result) || IsFunctionCall(input,ref position,result)||IsVariable(input,ref position, result);

    private bool IsVariable(string input,ref int position,TokenList result)
    {
       return TryMakeToken(input[position..], ref position,result,VariableRegex,x  => 
            new CallToken(x,"Property"));
    }
    private bool IsFunctionCall(string input,ref int position,TokenList result)
    {
        return IsVariable(input, ref position,result) || TryMakeToken(input[position..],ref position,result,FunctionRegex, x=> new CallToken(x,"Function") );
    }

    private bool IsSymbol(string token, ref int position,TokenList result)
    {
        string[] candidates = TokenList.SupportedOperators.Keys.Where(i => i.Length is 1).ToArray();
        string? op = candidates.FirstOrDefault(s => s == token);
        if (op is null) return false;
        switch (op)
        {
            case "-" when IsUnary(result):
                result.Add(new OperationToken("[-]"));
                break;
            case "(" when IsFunctionParen(result):
            default:
                result.Add(new OperationToken(token));
                break;
        }
        position += token.Length;
        return true;
    }
    private bool IsUnary(TokenList result)
    {
        return !result.Any() || result.TokenAt(result.Count - 1) is OperationToken { ClosingBracket: false };
    }

    private bool IsFunctionParen(TokenList result)
    {
        CallToken? token = result.TokenAt(result.Count - 1) as CallToken;
        return token != null;
    }

    private bool IsNumber(string input, ref int position,TokenList result)
    {
        bool isDecimal = TryMakeToken(input[position..],ref position,result ,DecimalRegex, x =>
        {
            if (!double.TryParse(x, out double value))
                throw new Exception();
            return new LiteralToken<double>(value);
        });
        if (isDecimal) return true;
        return TryMakeToken(input[position..] ,ref position,result,DigitRegex,x  =>
        {
            if (!double.TryParse(x, out double value))
                throw new Exception();
            return new LiteralToken<double>(value);
        });
    }

    private bool TryMakeToken(string input,ref int position,TokenList result ,Regex regex, Func<string,Token?> make)
    {
        Match match = regex.Match(input);
        if (!match.Success) return false;
        Token? token = make(match.Value);
        if(token is not null) result.Add(token);
        position += match.Length;
        return true;
    }
    
}