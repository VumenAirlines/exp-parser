using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;
using Nodes;

internal class CallNode(string name) : Node(1)
{
    internal string Name { get; }
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        throw new NotImplementedException();
    }
}
