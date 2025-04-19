namespace Exp_Parser.Model.Tokens;
using Nodes;
internal class LiteralToken<T> : Token
{
    private readonly T _value;

    internal LiteralToken(T value) => this._value = value;

    internal override Node CreateNode() => new LiteralNode<T>(_value);
}