namespace Exp_Parser;

using System;
using System.Text;
using System.Collections.Generic;


using System;
using System.Text;
using System.Collections.Generic;

public class AstVisualizer : IVisitor
{
    private readonly StringBuilder _builder = new();
    private string _indent = "";
    private bool _isLast = true;

    public string Visualize(INodeType root)
    {
        _builder.Clear();
        _indent = "";
        _isLast = true;
        VisitNode(root, true);
        return _builder.ToString();
    }

    private void VisitNode(INodeType node, bool isLast)
    {
        string savedIndent = _indent;
        _builder.Append(_indent);
        _builder.Append(isLast ? "└── " : "├── ");
        _indent += isLast ? "    " : "│   ";
        _isLast = isLast;
        node.Accept(this);
        _indent = savedIndent;
    }

    public void Visit(NumberNode numberNode)
    {
        _builder.AppendLine($"NumberNode: {numberNode.Value}");
    }

    public void Visit(IdentifierNode variableNode)
    {
        _builder.AppendLine($"IdentifierNode: {variableNode.Name}");
    }

    public void Visit(BinaryNode binaryNode)
    {
        _builder.AppendLine($"BinaryNode: {binaryNode.Op}");
        VisitNode(binaryNode.Left, false);
        VisitNode(binaryNode.Right, true);
    }

    public void Visit(CallNode callNode)
    {
        _builder.AppendLine($"CallNode: {callNode.FunctionName}");
        for (int i = 0; i < callNode.Arguments.Count; i++)
        {
            VisitNode(callNode.Arguments[i], i == callNode.Arguments.Count - 1);
        }
    }

    public void Visit(ExprStringNode exprStringNode)
    {
        _builder.AppendLine($"ExprStringNode: \"{exprStringNode.Expr}\"");
    }
}

