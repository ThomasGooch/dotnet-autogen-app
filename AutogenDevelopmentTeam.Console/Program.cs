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
software development. You often work on projects that require you to build in .Net9 using Clean Architecture.
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
    You often work on projects that require you to build in .Net9 as a version.
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
        CreateFilesAsOutput(response);

        var testMessage = new TextMessage(Role.User, $"write unit tests for the code: {response.GetContent()}");
        var testResponse = await unitTestAgent.SendAsync(testMessage);
        CreateFilesAsOutput(testResponse);
        Console.WriteLine("files generated successfully!");
    }
}

static void CreateFilesAsOutput(IMessage response)
{
    var responseName = response.From;
    var codeResponse = response.GetContent();
    var folderName = responseName == "developerAgent" ? "Code" : "Tests";
    var outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "output", folderName);

    // Ensure the directory exists
    Directory.CreateDirectory(outputFolder);

    var fileName = $"{responseName}.md";
    var filePath = Path.Combine(outputFolder, fileName);
    File.WriteAllText(filePath, codeResponse);
    Console.WriteLine($"Code saved to {filePath}");
}