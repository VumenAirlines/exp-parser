using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Exp_Parser.Model.Nodes;


public class CallNode(string name) : Node(1)
{
    //todo: optimize for log(2,x) and such
    public string Name { get; } = name;
    internal IList<Node> Parameters { get; } = [];
    internal override Expression BuildExpression(Expression? callerExpression = null)
    {
        if (callerExpression is null) throw new InvalidExpressionException("Callerexpression was null");
        var args = Parameters.Select(x => x.BuildExpression(callerExpression)).ToList();
        var candidates = callerExpression.Type.GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
            .Where(x=>x.Name.Equals(Name,StringComparison.InvariantCultureIgnoreCase))
            .Where(x =>
            {
                var paramTypes = x.GetParameters().Select(p => p.ParameterType).ToArray();
                return paramTypes.Length == args.Count && paramTypes.Zip(args,
                        (a, p) => p.Type.Name.Equals(a.Name,StringComparison.InvariantCultureIgnoreCase) || p.Type.GetInterfaces().Any(i => i.Name.Equals(a.Name,StringComparison.InvariantCultureIgnoreCase)))
                    .All(e => e);
            }).FirstOrDefault();
        if (candidates is not null)
           return Expression.Call(candidates, args);
        
        throw new Exception();
    }
    public override void  Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
