using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using System.ClientModel;

IChatClient chatClient = new OpenAI.Chat.ChatClient(
       "gpt-4.1-mini",
       new ApiKeyCredential("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"),
       new OpenAI.OpenAIClientOptions { Endpoint = new Uri("https://models.github.ai/inference") }
    ).AsIChatClient();


AIAgent TamilAgent  = new ChatClientAgent(
    chatClient, 
    new ChatClientAgentOptions { Name = "TamilAgent", ChatOptions = new ChatOptions { Instructions = "You are a translation assistant that the provided text to Tamil." } }
    );

AIAgent HindiAgent = new ChatClientAgent(
    chatClient,
    new ChatClientAgentOptions { Name = "HindiAgent", ChatOptions = new ChatOptions { Instructions = "You are a translation assistant that the provided text to Hindi." } }
    );

string QualityReviewerInstruction = """
        You are a multilingual translation quality reviewer.
    Check the translations for grammar accuracy, tone consistency, and cultural fit
    compared to the original English text.

    Give a brief summary with a quality rating (Excellent / Good / Need a Review).

    Example output:
    Quality: Excellent
    Feedback: Accurate translation, friendly tone preserved, minor punctuation tweaks only.

    """;

AIAgent ReviewerAgent = new ChatClientAgent(
    chatClient,
    new ChatClientAgentOptions { Name = "ReviewerAgenet", ChatOptions = new ChatOptions { Instructions = QualityReviewerInstruction } }
    );

AIAgent workflowAgent = AgentWorkflowBuilder.BuildSequential(TamilAgent, HindiAgent,ReviewerAgent).AsAIAgent();

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("You: ");
string userInput = Console.ReadLine() ?? "Welcome to our application. Please Verify you email address before continuing.";


AgentResponse agentRunResponse = await workflowAgent.RunAsync(userInput);

Console.WriteLine();

foreach(var message in agentRunResponse.Messages)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"{message.AuthorName}:");

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(message.Text);

    Console.WriteLine();
}