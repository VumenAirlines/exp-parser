using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;

internal class ExponentNode(): BinaryNode(2,"^")
{
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        return Expression.Power(Left?.BuildExpression(callerExpression),Right?.BuildExpression(callerExpression));
    }
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}