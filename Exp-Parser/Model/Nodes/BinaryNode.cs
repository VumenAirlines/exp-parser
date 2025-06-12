namespace Exp_Parser.Model.Nodes;

public abstract class BinaryNode(int precedence,string type) : Node(precedence)
{
    internal Node? Left { get; set; }
    internal Node? Right { get; set; }
    public readonly string Op = type;
    internal override bool IsClosed => (Left?.IsClosed ?? false) && Right is not null && Right.IsClosed;

    internal override bool TryAddNode(Node node)
    {
        if (ReferenceEquals(node, this)) return false;
        
        if (Right is not null) return Right.TryAddNode(node);
        Right = node;
        return true;
    }
}