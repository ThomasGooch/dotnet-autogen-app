# dotnet-autogen-app

## Overview
The dotnet-autogen-app is a console application that facilitates discussions around building simple .NET applications. It utilizes two intelligent agents: a Developer Agent and a Unit Tester Agent, to assist users in generating application code and corresponding unit tests.

## Features
- **Developer Agent**: Generates code based on user input regarding application development.
- **Unit Test Agent**: Creates unit tests for the code produced by the Developer Agent.
- **Interactive Console**: Users can engage in a dialogue with the agents to refine their application requirements and receive step-by-step guidance.

## Project Structure
```
dotnet-autogen-app
├── AutogenDevelopmentTeam.Console
│   ├── Program.cs
│   ├── Agents
│   │   ├── DeveloperAgent.cs
│   │   └── UnitTestAgent.cs
│   ├── Services
│   │   └── ApplicationBuilder.cs
│   └── Models
│       └── Message.cs
├── AutogenDevelopmentTeam.Core
│   ├── AutoGen.Core.csproj
│   ├── Interfaces
│   │   └── IAgent.cs
│   └── Utilities
│       └── MessageConnector.cs
├── AutogenDevelopmentTeam.Tests
│   ├── AutogenDevelopmentTeam.Tests.csproj
│   └── UnitTests
│       └── ApplicationBuilderTests.cs
├── dotnet-autogen-app.sln
└── README.md
```

## Setup Instructions
1. Clone the repository to your local machine.
2. Navigate to the project directory.
3. Restore the project dependencies using the command:
   ```
   dotnet restore
   ```
4. Build the project using:
   ```
   dotnet build
   ```

## Usage
1. Run the console application with the command:
   ```
   dotnet run --project AutogenDevelopmentTeam.Console
   ```
2. Follow the prompts to discuss your application requirements with the Developer Agent.
3. Once the code is generated, the Unit Test Agent will provide unit tests for the generated code.
4. Review the generated code and tests, and modify as necessary.

## Contributing
Contributions are welcome! Please feel free to submit a pull request or open an issue for any enhancements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.