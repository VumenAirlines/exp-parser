namespace Exp_Parser.Model;
using Nodes;
internal class NodeStack : Stack<Node>
{
    internal Node? LastAdded;

    internal Node? Root => this.Any() ? Peek() : null;

    private BinaryNode? _prevBinary;
    //-1+4*5^3
    internal void Add(Node node)
    {
        if (!this.Any())
            Push(node);
        else switch (node) {
            case BinaryNode { IsClosed: true }:
                AddToRoot(node);
                break;
            case ExponentNode exponentNode when _prevBinary is ExponentNode topExponent:
                AddNodeToRootRight(topExponent, exponentNode);
                break;
            case ExponentNode exponentNode when _prevBinary  is not null:
                if (_prevBinary.Precedence < exponentNode.Precedence)
                    
                    AddRootToNodeLeft(exponentNode);

                else
                    AddNodeToRootRight(_prevBinary, exponentNode);
                
                break;
            case ExponentNode exponentNode:
                AddRootToNodeLeft(exponentNode);
                break;
            case BinaryNode binaryNode when Peek() is BinaryNode root && root.Precedence <= binaryNode.Precedence:
                AddRootToNodeLeft(binaryNode);
                break;
            case BinaryNode binaryNode when Peek() is BinaryNode root:
                AddNodeToRootRight(root,binaryNode);
                break;
            case BinaryNode binaryNode:
                AddRootToNodeLeft(binaryNode);
                break;
            default:
                AddToRoot(node);
                break;
        }

        if (node is BinaryNode binNode) _prevBinary = binNode;
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
    private void AddNodeToRootRight(BinaryNode root, BinaryNode node)
    {
        node.Left ??= root.Right;
        root.Right = node;
        //Push(node);
    }

    internal void Reset()
    {
        Clear();
        LastAdded = null;
        _prevBinary = null;
    }
}