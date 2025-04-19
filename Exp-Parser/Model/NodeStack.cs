namespace Exp_Parser.Model;
using Nodes;
internal class NodeStack : Stack<Node>
{
    internal Node? LastAdded;

    internal void Add(Node node)
    {
        if (!this.Any())
            Push(node);
        else switch (node) {
            case BinaryNode { IsClosed: true }:
                AddToRoot(node);
                break;
            case BinaryNode binaryNode when Peek() is BinaryNode root && root.Precedence <= binaryNode.Precedence:
                AddRootToNodeLeft(binaryNode);
                break;
            case BinaryNode binaryNode when Peek() is BinaryNode root:
                binaryNode.Left ??= root.Right;
                root.Right = binaryNode;
                break;
            case BinaryNode binaryNode:
                AddRootToNodeLeft(binaryNode);
                break;
            default:
                AddToRoot(node);
                break;
        }
        LastAdded = node;
    }

    private void AddToRoot(Node node)
    {
        if (!Peek().TryAddNode(node))
            throw new InvalidOperationException($"Error adding '{node.GetType().Name}' to '{Peek().GetType().Name}'.");
    }

    private void AddRootToNodeLeft(BinaryNode node)
    {
        node.Left = Pop();
        Push(node);
    }
}