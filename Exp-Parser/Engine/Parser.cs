using System.Collections.Concurrent;
using Exp_Parser.Model.Tokens;

namespace Exp_Parser.Engine;
using System.Linq.Expressions;
using Model;
using Model.Nodes;

public class Parser : IExpParser
{
    //todo: (x+1)x dont work
    private static readonly ConcurrentDictionary<string, Delegate> CompiledExpressions = new();
    private readonly ParameterExpression _parameterExpression = Expression.Parameter(typeof(double), "x");

    //todo: (x+1)x dont work
    public Delegate BuildExpressionFor<T>(TokenList tokens, string? parameterName, string? input)
    {
       
        if (input is not null && CompiledExpressions.TryGetValue(input, out var cached))
            return cached;
        Node root = BuildTree(tokens);
        Expression body = root.BuildExpression(_parameterExpression);

        Delegate result = Expression.Lambda(body, _parameterExpression).Compile();
        
        if (input is not null)
            CompiledExpressions.GetOrAdd(input, _ => result);
        
        return result;
    }

    private Node BuildTree(TokenList tokens)
    {
        NodeStack nodes = new NodeStack();
        Node? res = null;
        try
        {
            while (tokens.Any() && !(tokens.Current.ClosingBracket || tokens.Current.IsSeparator))
            {
                Process(tokens,nodes);
                tokens.MoveNext();
            }

            res = nodes.Root;
            if (res is null)
                throw new Exception();
            return res;
        }
        catch(Exception e)
        {
            if(res is BinaryNode node)
                Reset(node);
            throw;
        }
        finally
        {
            nodes.Reset();
        }
    }

    private void Process(TokenList tokens , NodeStack nodes)
    {
        switch (tokens.Current.OpeningBracket)
        {
            case true when nodes.LastAdded is CallNode method:
                tokens.MoveNext();
                ProcessParameters(tokens, method);
                break;
            case true when nodes.LastAdded is LiteralNode or ExponentNode:
                tokens.MoveNext();
                ProcessImplicitMult(nodes, tokens);
                ProcessExpression(tokens, nodes);
                break;
            case true when nodes.LastAdded is VariableNode:
                tokens.MoveNext();
                if (!ProcessImplicitMult(nodes, tokens))
                {
                    Node childNode = new MultiplyNode();
                    nodes.Add(childNode);
                }
                ProcessExpression(tokens, nodes);
                break;
            case true:
                tokens.MoveNext();
                ProcessExpression(tokens, nodes);
                break;
            case false when nodes.LastAdded is LiteralNode or VariableNode or CallNode:
                ProcessImplicitMult(nodes, tokens);
                nodes.Add(tokens.Current.CreateNode());
                break;
            default:
                nodes.Add(tokens.Current.CreateNode());
                break;
        }
    }
    private bool ProcessImplicitMult( NodeStack nodes, TokenList tokenList)
    {
        //todo: add (x+1)(x-1) support
        switch (nodes.LastAdded)
        {
            case VariableNode when tokenList.Current is not CallToken{NodeType:"Function"}:
            case LiteralNode or CallNode when tokenList.Current is not CallToken:
                return false;
            default:
                Node childNode = new MultiplyNode();
                nodes.Add(childNode);
                return true;
        }
    }
    private void ProcessParameters(TokenList tokens, CallNode callNode)
    {
        while (!tokens.Current.ClosingBracket) {
            Node childNode = BuildTree(tokens);
            callNode.Parameters.Add(childNode);
            if (tokens.Current.IsSeparator) {
                tokens.MoveNext();
            }
        }
    }

    private void ProcessExpression(TokenList tokens, NodeStack nodes)
    {
        Node childNode = BuildTree(tokens);
        childNode.RaisePrec();
        nodes.Add(childNode);
    }
    private void Reset(BinaryNode? node)
    {
        if (node == null) return;
    
        if (node.Left is BinaryNode leftBinary)
            Reset(leftBinary);
        if (node.Right is BinaryNode rightBinary)
            Reset(rightBinary);
        
        node.Left = null;
        node.Right = null;
    }
    
}