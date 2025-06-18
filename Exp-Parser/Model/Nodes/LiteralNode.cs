namespace Exp_Parser.Model.Nodes;
using System.Linq.Expressions;
internal abstract class LiteralNode() : Node(99);

internal class LiteralNode<T>(T value) :LiteralNode
{
    private T Value { get; } = value;
    
    internal override Expression BuildExpression(Expression? callerExpression = null) => Expression.Constant(Value);
    public override string? ToString()
    {
        return Value != null ? Value.ToString() : null;
    }
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}