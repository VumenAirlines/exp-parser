namespace Exp_Parser.Model.Nodes;
using System.Linq.Expressions;
internal abstract class LiteralNode() : Node(99);

internal class LiteralNode<T>(T value) :LiteralNode()
{
    internal T Value { get; } = value;
    
    internal override Expression BuildExpression(Expression? callerExpression = null) => Expression.Constant(Value);
}