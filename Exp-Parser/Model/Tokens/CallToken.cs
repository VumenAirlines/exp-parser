namespace Exp_Parser.Model.Tokens;
using Nodes;
internal class CallToken : Token
{
    private readonly string _name;

    internal CallToken(string name,string type)
    {
        _name = name;
        NodeType = type;
    }
    public string NodeType { get; set; }

    internal override Node CreateNode() => NodeType == "Function" ?  new CallNode(_name) : new VariableNode(_name);
    
}