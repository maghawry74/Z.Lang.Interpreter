namespace Z.Lang.Parser.Types.DataTypes;

public class ArrayValue (List<object?> elements)
{
    public List<object?> Elements { get; } = elements;
}