using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;

public class VariableNode(string name) : Node(99)
{
   
    private string Name { get; } = name;
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        return callerExpression switch
        {
            null => throw new InvalidOperationException($"Unknown identifier '{Name}'."),
            ParameterExpression parameterExpression when parameterExpression.Name == Name => callerExpression,
            _ => Expression.PropertyOrField(callerExpression, Name)
        };
    }

    public override string ToString() => Name;
    public override void  Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}