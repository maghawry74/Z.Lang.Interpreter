using System.Text.Json;
using System.Text.Json.Serialization;
using Z.Lang.Lexer;

var user = Environment.UserName;

var jsonSerializerOptions = new JsonSerializerOptions()
{
    WriteIndented = true,
    Converters = { new JsonStringEnumConverter() }
};

Console.WriteLine($"Hello, {user}! Welcome to Z Lang");
Console.Write("> ");

while (true)
{
    var input = Console.ReadLine();
    if (string.IsNullOrEmpty(input)) break;
    
    var lexer = new Lexer(input);

    foreach (var token in lexer.GenerateTokens())
    {
        Console.WriteLine(JsonSerializer.Serialize(token, jsonSerializerOptions));
    }

    Console.ReadKey();
    Console.Clear();
    Console.Write("> ");
}