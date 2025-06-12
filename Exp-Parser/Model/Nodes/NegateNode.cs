using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;

public class NegateNode(): UnaryNode(2)
{
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        return Expression.Negate(Child?.BuildExpression(callerExpression));
    }
    public override void  Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}