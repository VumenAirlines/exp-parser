using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;

internal class SubtractNode():BinaryNode(4)
{
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        throw new NotImplementedException();
    }
}