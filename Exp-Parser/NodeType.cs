namespace Exp_Parser;

// Base class for AST nodes
public interface INodeType
{
    void Accept(IVisitor visitor);
}

// Visitor interface to handle different node types
public interface IVisitor
{
    void Visit(BinaryNode binaryNode);
    void Visit(NumberNode numberNode);
    void Visit(ExprStringNode exprStringNode);
    void Visit(CallNode callNode);
    void Visit(IdentifierNode variableNode);
}


public class NumberNode(string value) : INodeType
{
    public readonly string Value = value;
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class IdentifierNode(string name) : INodeType
{
    public readonly string Name = name;
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class BinaryNode(string op, INodeType left, INodeType right) : INodeType
{
    public readonly string Op = op;
    public readonly INodeType Left = left;
    public readonly INodeType Right = right;

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class CallNode(string functionName, List<INodeType> arguments) : INodeType
{
    public readonly string FunctionName = functionName;
    public readonly List<INodeType> Arguments = arguments;

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class ExprStringNode(string expr) : INodeType
{
    public string Expr = expr;
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}