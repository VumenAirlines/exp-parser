using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;

internal class VariableNode(string name) : Node(99)
{
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        throw new NotImplementedException();
    }
}