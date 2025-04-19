using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;

internal class MultiplyNode(): BinaryNode(3)
{
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        throw new NotImplementedException();
    }
}