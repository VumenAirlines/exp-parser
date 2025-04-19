using System.Text.RegularExpressions;
using Exp_Parser.Model.Tokens;

namespace Exp_Parser.Engine;
using Model;
internal class Tokenizer
{
    private readonly TokenList _result = [];
    private int _characterPosition;
    //private static readonly IDictionary<string, Type> availableTypes = new Dictionary<string, Type>(Keywords.BuiltInTypes);

    //internal static void AddTypeMap(string alias, Type type) => availableTypes[alias ?? type?.Name ?? throw new ArgumentNullException(nameof(type))] = type;
    public TokenList Tokenize(string input)
    {
       string trimmed=  Regex.Replace(input, @"\s+", string.Empty);
        for (_characterPosition = 0; _characterPosition < input.Length;)
            if (!IsValidToken(trimmed)) 
                throw new ArgumentException($"Invalid token at position {_characterPosition + 1}.", nameof(input));
        return _result;
    }
    private bool IsValidToken(string input) => 
        IsNumber(input) || IsSymbol(input[_characterPosition].ToString()) || IsFunctionCall(input)||IsVariable(input);

    private bool IsVariable(string input)
    {
        bool isVariable = TryMakeToken(input[_characterPosition..], @"^[x]",x  => 
            new CallToken(x,"Property"));
        return isVariable;
    }
    private bool IsFunctionCall(string input)
    {
        if(IsVariable(input)) return true;
        bool isFunctionCall =TryMakeToken(input[_characterPosition..], @"^[\w]*", x=> new CallToken(x,"Function") );
        
        return isFunctionCall;
    }

    private bool IsSymbol(string token)
    {
        string[] candidates = TokenList.SupportedOperators.Keys.Where(i => i.Length is 1).ToArray();
        string? op = candidates.FirstOrDefault(s => s == token);
        if (op is null) return false;
        switch (op)
        {
            case "-" when IsUnary():
                _result.Add(new OperationToken("[-]"));
                break;
            case "(" when IsFunctionParen(out _):
            default:
                _result.Add(new OperationToken(token));
                break;
        }
        _characterPosition += token.Length;
        return true;
    }
    private bool IsUnary()
    {
        return !_result.Any() || (_result.TokenAt(_result.Count - 1) is OperationToken);
    }

    private bool IsFunctionParen(out CallToken? token)
    {
        token = (_result.TokenAt(_result.Count - 1) is CallToken candidate) ? candidate : null;
        return token != null;
    }

    private bool IsNumber(string input)
    {
        bool isDecimal = TryMakeToken(input[_characterPosition..], @"^((\d*\.\d+)|(\d+\.\d*))",x  => 
            new LiteralToken<double>(Convert.ToDouble(x)));
        if (isDecimal) return true;
        bool isInt = TryMakeToken(input[_characterPosition..], @"^\d+",x  => new LiteralToken<int>(Convert.ToInt32(x)));
        return isInt;
    }

    private bool TryMakeToken(string input, string regex, Func<string,Token> make)
    {
        Match match = Regex.Match(input, regex, RegexOptions.IgnoreCase);
        if (!match.Success) return false;
        Token? token = make(match.Value);
        if(token is not null) _result.Add(token);
        _characterPosition += match.Length;
        return true;
    }
    
}