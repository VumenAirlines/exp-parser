namespace Exp_Parser.Model.Tokens;
using Nodes;
internal class CallToken : Token
{
    private readonly string _name;

    internal CallToken(string name,string type)
    {
        this._name = name;
        NodeType = type;
    }
    public string NodeType { get; set; }

    internal override Node CreateNode() => NodeType == "Function" ? (Node) new CallNode(_name) : new VariableNode(_name);
    
}