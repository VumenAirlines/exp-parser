using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;

internal class ExponentNode(): BinaryNode(2)
{
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        throw new NotImplementedException();
    }
}