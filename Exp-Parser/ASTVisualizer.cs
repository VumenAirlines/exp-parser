using System.Text;
using Exp_Parser.Model.Nodes;

namespace Exp_Parser.Model;

internal interface IVisitor
{
    void Visit(BinaryNode binaryNode);
    void Visit(LiteralNode numberNode);
    void Visit( UnaryNode unaryNode);
    void Visit(CallNode callNode);
    void Visit(VariableNode variableNode);
}
// public class AstVisualizer : IVisitor
// {
//     private readonly StringBuilder _builder = new();
//     private string _indent = "";
//     private bool _isLast = true;
//
//     public string Visualize(Node root)
//     {
//         _builder.Clear();
//         _indent = "";
//         _isLast = true;
//         VisitNode(root, true);
//         return _builder.ToString();
//     }
//
//     private void VisitNode(Node node, bool isLast)
//     {
//         string savedIndent = _indent;
//         _builder.Append(_indent);
//         _builder.Append(isLast ? "└── " : "├── ");
//         _indent += isLast ? "    " : "│   ";
//         _isLast = isLast;
//         node.Accept(this);
//         _indent = savedIndent;
//     }
//
//     public void Visit(LiteralNode numberNode)
//     {
//         _builder.AppendLine($"NumberNode: {numberNode.ToString()}");
//     }
//
//     public void Visit(UnaryNode unaryNode)
//     {
//         _builder.AppendLine($"NegateNode: -{unaryNode.Child}");
//     }
//
//     public void Visit(VariableNode variableNode)
//     {
//         _builder.AppendLine($"IdentifierNode: {variableNode.ToString()}");
//     }
//
//     public void Visit(BinaryNode binaryNode)
//     {
//         _builder.AppendLine($"BinaryNode: {binaryNode.Op}");
//         if (binaryNode?.Left != null) VisitNode(binaryNode.Left, false);
//         if (binaryNode?.Right != null) VisitNode(binaryNode.Right, true);
//     }
//
//     public void Visit(CallNode callNode)
//     {
//         _builder.AppendLine($"CallNode: {callNode.Name}");
//         for (int i = 0; i < callNode.Parameters.Count; i++)
//         {
//             VisitNode(callNode.Parameters[i], i == callNode.Parameters.Count - 1);
//         }
//     }
//
//    
// }
