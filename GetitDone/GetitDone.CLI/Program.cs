using System.CommandLine;
using System.CommandLine.Invocation;
using Getitdone.Client;
using Getitdone.Client.Models;
using System;
using System.ClientModel;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var apiEndpointOption = new Option<string>(
            "--api-endpoint",
            getDefaultValue: () => "https://api.Getitdone.com",
            description: "The base URL for the Getitdone API."
        );

        var rootCommand = new RootCommand("Getitdone CLI");
        rootCommand.AddGlobalOption(apiEndpointOption);

        var listProjectsCommand = new Command("list-projects", "Lists all projects");
        listProjectsCommand.SetHandler(async (InvocationContext context) =>
        {
            string? apiEndpoint = context.ParseResult.GetValueForOption<string>(apiEndpointOption);
            await ListProjects(apiEndpoint ?? "https://api.Getitdone.com");
        });
        rootCommand.AddCommand(listProjectsCommand);

        var nameOption = new Option<string>("--name", "The name of the project") { IsRequired = true };
        var colorOption = new Option<string>("--color", "The color of the project");

        var createProjectCommand = new Command("create-project", "Creates a new project")
        {
            nameOption,
            colorOption
        };
        createProjectCommand.SetHandler(async (InvocationContext context) =>
        {
            string? apiEndpoint = context.ParseResult.GetValueForOption<string>(apiEndpointOption);
            string name = context.ParseResult.GetValueForOption(nameOption)!;
            string? color = context.ParseResult.GetValueForOption(colorOption);
            await CreateProject(apiEndpoint ?? "https://api.Getitdone.com", name, color);
        });
        rootCommand.AddCommand(createProjectCommand);

        var idOption = new Option<string>("--id", "The ID of the project") { IsRequired = true };
        var getProjectCommand = new Command("get-project", "Gets a specific project by ID")
        {
            idOption
        };
        getProjectCommand.SetHandler(async (InvocationContext context) =>
        {
            string? apiEndpoint = context.ParseResult.GetValueForOption<string>(apiEndpointOption);
            string id = context.ParseResult.GetValueForOption(idOption)!;
            await GetProject(apiEndpoint ?? "https://api.Getitdone.com", id);
        });
        rootCommand.AddCommand(getProjectCommand);

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task ListProjects(string apiEndpoint)
    {
        Console.WriteLine($"Listing projects using API endpoint: {apiEndpoint}...");
        var client = new GetitdoneClient(new Uri(apiEndpoint), new GetitdoneClientOptions());
        var projectsClient = client.GetProjectsClient();
        var projects = await projectsClient.GetProjectsAsync();

        foreach (var project in projects.Value)
        {
            Console.WriteLine($"- {project.Name} (ID: {project.Id})");
        }
    }

    private static async Task CreateProject(string apiEndpoint, string name, string? color)
    {
        Console.WriteLine($"Creating project '{name}' using API endpoint: {apiEndpoint}...");
        var client = new GetitdoneClient(new Uri(apiEndpoint), new GetitdoneClientOptions());
        var projectsClient = client.GetProjectsClient();
        var createRequest = new CreateProjectRequest(name)
        {
            Color = color
        };
        var project = await projectsClient.CreateProjectAsync(createRequest);

        Console.WriteLine($"Created project with ID: {project.Value.Id}");
    }

    private static async Task GetProject(string apiEndpoint, string id)
    {
        Console.WriteLine($"Getting project with ID '{id}' using API endpoint: {apiEndpoint}...");

        if (string.IsNullOrWhiteSpace(id))
        {
            Console.WriteLine("Error: Project ID cannot be empty or whitespace.");
            return;
        }

        try
        {
            var client = new GetitdoneClient(new Uri(apiEndpoint), new GetitdoneClientOptions());
            var projectsClient = client.GetProjectsClient();
            var project = await projectsClient.GetProjectsProjectOpsClient().GetProjectAsync(id);

            Console.WriteLine($"Project Name: {project.Value.Name}");
            Console.WriteLine($"Project ID: {project.Value.Id}");
            Console.WriteLine($"Project Color: {project.Value.Color}");
        }
        catch (ClientResultException ex)
        {
            Console.WriteLine($"Error: Failed to get project with ID '{id}'.");
            Console.WriteLine($"  Status Code: {ex.Status}");
            Console.WriteLine($"  Message: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}