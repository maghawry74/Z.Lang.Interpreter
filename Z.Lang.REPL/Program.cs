using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Z.Lang.Lexer;
using Z.Lang.Lexer.Types;
using Z.Lang.Parser;
var user = Environment.UserName;
if (args.Length > 0)
{
    var filePath = args[0];
    if (!File.Exists(filePath))
    {
        Console.WriteLine($"File {filePath} does not exist"); 
        return;
    }
    var input = string.Empty;
    
    await using (var fileStream = new FileStream(filePath, FileMode.Open,FileAccess.Read))
    {
        var buffer = new byte[1024];
        var bytesRead = await fileStream.ReadAsync(buffer.AsMemory(0, 1024));
        while (bytesRead > 0)
        {
            input += Encoding.UTF8.GetString(buffer, 0, bytesRead);
            bytesRead = await fileStream.ReadAsync(buffer.AsMemory(0, 1024));
        }
    }
    HandleInput(input);
    return;
}

Console.WriteLine($"Hello, {user}! Welcome to Z Lang");
while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    if(string.IsNullOrWhiteSpace(input)) break;
    try
    {
        HandleInput(input);
    }
    catch (Exception e)
    {
        Console.WriteLine($"{e.Message}");
    }
}

return;

void HandleInput(string s)
{
    var lexer = new Lexer(s);
    var parser = new Parser(lexer);
    Console.WriteLine(parser.Parse().Evaluate());
}