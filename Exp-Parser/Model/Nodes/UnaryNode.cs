namespace Exp_Parser.Model.Nodes;

public abstract class UnaryNode(int precedence) : Node(precedence)
{

    internal Node? Child { get; private set; }

    internal override bool IsClosed => Child is { IsClosed: true };

    internal override bool TryAddNode(Node node)
    {
        if (ReferenceEquals(node, this)) return false;
        if (Child is not null) return Child.TryAddNode(node);
        Child = node;
        return true;
    }
}