namespace Exp_Parser;

public class Parser(List<Token> tokens)
{
    private int _pos = 0;


    public INodeType Parse()
    {
        INodeType result = Expression();
        
        if (_pos < tokens.Count)
            throw new Exception($"Unexpected token '{tokens[_pos].Type}' at position {_pos}");
            
        return result;
    }

    private Token Eat(params string[] expectedTypes)
    {
        if (_pos >= tokens.Count)
            throw new Exception($"Unexpected end of input; expected one of {string.Join(", ", expectedTypes)} at {_pos}");

        Token token = tokens[_pos];

        if (!expectedTypes.Contains(token.Type))
            throw new Exception($"Expected {string.Join(" or ", expectedTypes)}, but found '{token.Type}' at {_pos}");

        _pos++;
        return token;
    }

    private bool Is(params string[] types) =>
         _pos < tokens.Count && types.Contains(tokens[_pos].Type);

    private Token Peek() => (_pos < tokens.Count ? tokens[_pos] : null) ?? throw new Exception($"Unexpected end of input at {_pos}");
    
    

    // Grammar: Expression -> Addition -> MultiplicationOrDivision -> ImplicitMultiplication -> Exponentiation -> Primary

    private INodeType Expression() => Addition();
    

    private INodeType Addition()
    {
        INodeType left = Mult();
        
        while (Is("+", "-"))
        {
            string op = Eat("+", "-").Type;
            INodeType right = Mult();
            left = new BinaryNode(op, left, right);
        }
        
        return left;
    }

    private INodeType Mult()
    {
        INodeType left = ImplicitMult();
        
        while (Is("*", "/"))
        {
            string op = Eat("*", "/").Type;
            INodeType right = ImplicitMult();
            left = new BinaryNode(op, left, right);
        }
        
        return left;
    }

    private INodeType ImplicitMult()
    {
        INodeType left = Exponentiation();
        
        while ( Is("(") || Is("IDENT") || Is("NUMBER"))
        {
            INodeType right = Exponentiation();
            left = new BinaryNode("*", left, right);
        }
        
        return left;
    }
    
    private INodeType Exponentiation()
    {
        INodeType left = Primary();

        if (!Is("^")) return left;
        string op = Eat("^").Type;
        INodeType right = Exponentiation(); 
        left = new BinaryNode(op, left, right);

        return left;
    }

    private INodeType Primary()
    {
        INodeType result;
        switch (Peek().Type)
        {
            case "(":
                Eat("(");
                INodeType expr = Expression();
                Eat(")");
                result = expr;
                break;
            case "NUMBER":
                result = new NumberNode(Eat("NUMBER").Value);
                break;
            case "IDENT":
                string ident = Eat("IDENT").Value;
            
                if (Is("("))
                {
                    Eat("(");
                    var args = new List<INodeType>();
                    do
                    {
                        if (Is(")")) break;
                        args.Add(Expression());
                    }
                    while (Is(",") && Eat(",") is { } _);
                
                    Eat(")");
                    result = new CallNode(ident, args);
                }
                else 
                    result = new IdentifierNode(ident);
                
                break;
            case "STRING":
                string token = Eat("STRING").Value;
                string innerExpr = token.Substring(1, token.Length - 2); // remove quotes
            
                List<Token> innerTokens = new Tokenizer().Tokenize(innerExpr);
                _ = new Parser(innerTokens).Parse();
            
                result = new ExprStringNode(innerExpr);
                break;
            default:
                throw new Exception($"Expected a number, identifier, or parenthesized expression at {_pos}");
                
        }
        return result;
    }
}