namespace Exp_Parser.Model.Nodes;

internal abstract class UnaryNode(int precedence) : Node(precedence)
{

    internal Node? Child { get; set; }

    internal override bool IsClosed => Child is { IsClosed: true };

    internal override bool TryAddNode(Node node)
    {
        if (Child is not null) return Child.TryAddNode(node);
        Child = node;
        return true;
    }
}