namespace Exp_Parser.Model.Tokens;
using Nodes;
internal class OperationToken(string symbol) : Token
{
    internal string Symbol { get; } = symbol;

    internal override Node CreateNode() => TokenList.SupportedOperators[Symbol]();
    
    internal override bool OpeningBracket => Symbol == "(";
    internal override bool IsSeparator => Symbol == ",";
    internal override bool ClosingBracket => Symbol == ")";
}