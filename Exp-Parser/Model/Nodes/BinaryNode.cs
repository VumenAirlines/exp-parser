namespace Exp_Parser.Model.Nodes;

internal abstract class BinaryNode(int precedence) : Node(precedence)
{
    internal Node? Left { get; set; }
    internal Node? Right { get; set; }

    internal override bool IsClosed => (Left?.IsClosed ?? false) && Right is not null && Right.IsClosed;

    internal override bool TryAddNode(Node node)
    {
        if (Right is not null) return Right.TryAddNode(node);
        Right = node;
        return true;
    }
}