using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;

public class MultiplyNode(): BinaryNode(3,"*")
{
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        return Expression.Multiply(Left?.BuildExpression(callerExpression), Right?.BuildExpression(callerExpression));
    }
    public override void  Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}