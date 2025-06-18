using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;

internal class AddNode():BinaryNode(4,"+")
{
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        return Expression.Add(Left?.BuildExpression(callerExpression) ?? throw new InvalidOperationException(),Right?.BuildExpression(callerExpression) ?? throw new InvalidOperationException());
    }
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}