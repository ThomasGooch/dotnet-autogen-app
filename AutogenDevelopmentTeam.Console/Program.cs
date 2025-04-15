using AutoGen.Core;
using AutoGen.Ollama;
using AutoGen.Ollama.Extension;

using var httpClient = new HttpClient()
{
    BaseAddress = new Uri("http://localhost:11434"),
};

var developerAgent = new OllamaAgent(
    httpClient: httpClient,
    name: "DeveloperAgent",
    modelName: "llama3.1",
    systemMessage: @"You are a professional dotnet engineer, known for your expertise in 
software development.
    You use your skills to create software applications, tools, and 
games that are both functional and efficient.
    Your preference is to write clean, well-structured code that is easy 
to read and maintain."
)
    .RegisterMessageConnector()
    .RegisterPrintMessage();

var unitTestAgent = new OllamaAgent(
    httpClient: httpClient,
    name: "UnitTestAgent",
    modelName: "llama3.1",
    systemMessage: @"You are a professional dotnet engineer, known for your expertise in unit testing dotnet applications.
    You use your skills to create unit tests in xunit with NSubstitute that are both functional and efficient.
    Your preference is to write clean, well-structured unit tests that are easy 
to read and maintain.")
    .RegisterMessageConnector()
    .RegisterPrintMessage();



Console.WriteLine("Welcome to the .NET Application Builder!");

await RunApplicationAsync(developerAgent.StreamingAgent, unitTestAgent.StreamingAgent);

Console.WriteLine("Thank you for using the .NET Application Builder!");

static async Task RunApplicationAsync(IStreamingAgent developerAgent, IStreamingAgent unitTestAgent)
{
    while (true)
    {
        Console.WriteLine("Enter your application idea (or type 'exit' to quit):");
        var userInput = Console.ReadLine();
        if (userInput?.ToLower() == "exit")
        {
            break;
        }

        var textMessage = new TextMessage(Role.User, userInput!);
        var response = await developerAgent.SendAsync(textMessage);
        var codeResponse = response.GetContent()!.ToString();
        Console.WriteLine("Generated Code:");
        Console.WriteLine(response.GetContent()!.ToString());

        var testMessage = new TextMessage(Role.User, $"write unit tests for the code: {codeResponse}");
        var testResponse = await unitTestAgent.SendAsync(testMessage);
        Console.WriteLine("Generated Unit Tests:");
        Console.WriteLine(testResponse.GetContent()!.ToString());
    }
}