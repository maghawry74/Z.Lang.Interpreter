using System.Collections;
using Z.Lang.Parser.Types.DataTypes;
using Z.Lang.Parser.Types.Nodes.Expressions;

namespace Z.Lang.Parser.Types;

public class Environment(Environment? parent = null)
{
    private static Dictionary<string, object> BuiltIn { get; } = new()
    {
         {
            "len",
            (Func<object, long>)(obj =>
            {
                return obj switch
                {
                    string s1 => s1.Length, 
                    ArrayValue e => e.Elements.Count,
                    _ => throw new NotSupportedException()
                };
            })
        },
        {
            "first",
            (Func<object, object>)(obj =>
            {
                if (obj is not ArrayValue list) throw new NotSupportedException($"unexpected type {obj.GetType().Name}");
                if(list.Elements.Count == 0) throw new InvalidOperationException("list is empty");
                return list.Elements[0];
            })
        },
        {
            "last",
            (Func<object, object>)(obj =>
            {
                if (obj is not ArrayValue list) throw new NotSupportedException($"unexpected type {obj.GetType().Name}");
                if(list.Elements.Count == 0) throw new InvalidOperationException("list is empty");
                return list.Elements[^1];
            })
        },
        {
            "rest",
            (Func<object, object>)(obj =>
            {
                if (obj is not ArrayValue list) throw new NotSupportedException($"unexpected type {obj.GetType().Name}");
                if(list.Elements.Count == 0) throw new InvalidOperationException("list is empty");
                return new ArrayValue(list.Elements.Skip(1).ToList()!); 
            })
        },
        {
            "push",
            (Func<object, object,object>)((obj1, obj2) =>
            {
                if(obj1 is not ArrayValue list) throw new NotSupportedException();
                return new ArrayValue(list.Elements.Concat([obj2]).ToList()!);
            }) 
        }
    };
    
    private readonly Dictionary<string, object?> _variables = new();

    public object? Get(string name)
    {
        if (_variables.TryGetValue(name, value: out var variable)) return variable;
        return parent?.Get(name) ?? BuiltIn.GetValueOrDefault(name) ?? throw new System.Exception($"Variable {name} not found");
    }

    public void Set(string name, object? value)
    {
        if (!_variables.TryAdd(name, value)) throw new System.Exception($"Variable {name} already exists");
    }
    public void ReAssign(string name, object? value)
    {
        if (!_variables.ContainsKey(name)) throw new System.Exception($"Variable {name} not found");
        _variables[name] = value;
    }
}