using Exp_Parser.Model.Tokens;

namespace Exp_Parser.Engine;
using System.Linq.Expressions;
using Model;
using Model.Nodes;

internal static class Parser
{
    
    internal static LambdaExpression BuildExpressionFor<T>(TokenList tokens, string? parameterName)
    {
        Node root = BuildTree(tokens);
        ParameterExpression parameterExpression = parameterName is not null ? Expression.Parameter(typeof(T)) : Expression.Parameter(typeof(T), parameterName);
        Expression body = root.BuildExpression(parameterExpression);
        return Expression.Lambda(body, parameterExpression);
    }

    public static Node BuildTree(TokenList tokens)
    {
        NodeStack nodes = [];
        while (tokens.Any() && !(tokens.Current.ClosingBracket || tokens.Current.IsSeparator))
        {
            switch (tokens.Current.OpeningBracket)
            {
                case true when nodes.LastAdded is CallNode method:
                    tokens.MoveNext();
                    ProcessParameters(tokens, method);
                    break;
                case true when nodes.LastAdded is LiteralNode or VariableNode :
                    tokens.MoveNext();
                    ProcessImplicitMult(nodes,tokens);
                    break;
                case true:
                    tokens.MoveNext();
                    ProcessExpression(tokens, nodes);
                    break;
                case false when nodes.LastAdded is LiteralNode or VariableNode or CallNode :
                    ProcessImplicitMult(nodes,tokens);
                    nodes.Add(tokens.Current.CreateNode());
                    break;
                default:
                    nodes.Add(tokens.Current.CreateNode());
                    break;
            }

            tokens.MoveNext();
        }
        return nodes.Pop();
    }

    private static void ProcessImplicitMult( NodeStack nodes, TokenList tokenList)
    {
        //todo: add (x+1)(x-1) support
        switch (nodes.LastAdded)
        {
            case VariableNode when tokenList.Current is not CallToken{NodeType:"Function"}:
            case LiteralNode when tokenList.Current is not CallToken:
            case CallNode when tokenList.Current is not CallToken:
                return;
            default:
                Node childNode = new MultiplyNode();
                nodes.Add(childNode);
                break;
            
        }
    }
    private static void ProcessParameters(TokenList tokens, CallNode callNode)
    {
        while (!tokens.Current.ClosingBracket) {
            Node childNode = BuildTree(tokens);
            //callNode.Parameters.Add(childNode);
            if (tokens.Current.IsSeparator) {
                tokens.MoveNext();
            }
        }
    }

    private static void ProcessExpression(TokenList tokens, NodeStack nodes)
    {
        Node childNode = BuildTree(tokens);
        childNode.RaisePrec();
        nodes.Add(childNode);
    }
    
}