using Microsoft.Extensions.AI;
using System.ClientModel;

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("This is bala. My first AI proram.");
IChatClient chatClient = new OpenAI.Chat.ChatClient(
       "gpt-4.1-mini",
       new ApiKeyCredential("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"),
       new OpenAI.OpenAIClientOptions { Endpoint = new Uri("https://models.github.ai/inference") }
    ).AsIChatClient();

Console.WriteLine("GPT 4.1 mini Chat - type 'exit' to quit");
Console.WriteLine();
List<Microsoft.Extensions.AI.ChatMessage> messages = new();

while(true){
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("You :");
    var userInput = Console.ReadLine();

    if (string.IsNullOrEmpty(userInput)) continue;
    if(userInput.Equals("exit",StringComparison.OrdinalIgnoreCase)) break;

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Assitent:");
    messages.Add(new ChatMessage(ChatRole.User, userInput));
    string assistentMessage = GetResponse(chatClient.GetStreamingResponseAsync(messages)).Result;                
    messages.Add(new ChatMessage(ChatRole.Assistant, assistentMessage));
    Console.WriteLine();
}

async static Task<string> GetResponse(IAsyncEnumerable<ChatResponseUpdate> messages)
{
    string assistentMessage = string.Empty;
    await foreach (var res in messages)
    {
        Console.WriteLine(res.Text);
        assistentMessage += res.Text;
    }

    return assistentMessage;
}
