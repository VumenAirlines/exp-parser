namespace Exp_Parser.Model;

using System.Collections.Generic;
using Nodes;
using Tokens;
public class TokenList : List<Token>
{
    internal Token? TokenAt(int position) => (position >= 0 && position < Count) ? this[position] : null;

    internal Token Current => this[0];
    internal void MoveNext() => RemoveAt(0);

    internal static readonly IReadOnlyDictionary<string, Func<Node>?> SupportedOperators = new Dictionary<string, Func<Node>?>
    {
        {"[-]",()=>new NegateNode()},
        {"+", () =>new AddNode()},
        {"-", () => new SubtractNode()},
        {"*", () => new MultiplyNode()},
        {"/", () => new DivideNode()},
        {"^",(() => new ExponentNode())},
        {",", null},
        {"(", null},
        {")", null},
        
    };
}