using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;

public class SubtractNode():BinaryNode(4,"-")
{
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        return Expression.Subtract(Left?.BuildExpression(callerExpression), Right?.BuildExpression(callerExpression));
    }
    public override void  Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}