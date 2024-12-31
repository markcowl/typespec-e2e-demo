using System.CommandLine;
using System.CommandLine.Invocation;
using Getitdone.Client;
using Getitdone.Client.Models;
using System;
using System.ClientModel;
using System.Linq;
using System.Threading.Tasks;

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

        var deleteProjectCommand = new Command("delete-project", "Deletes a specific project by ID")
        {
            idOption
        };
        deleteProjectCommand.SetHandler(async (InvocationContext context) =>
        {
            string? apiEndpoint = context.ParseResult.GetValueForOption<string>(apiEndpointOption);
            string id = context.ParseResult.GetValueForOption(idOption)!;
            await DeleteProject(apiEndpoint ?? "https://api.Getitdone.com", id);
        });
        rootCommand.AddCommand(deleteProjectCommand);

        var deleteAllProjectsCommand = new Command("delete-all-projects", "Deletes all projects");
        deleteAllProjectsCommand.SetHandler(async (InvocationContext context) =>
        {
            string? apiEndpoint = context.ParseResult.GetValueForOption<string>(apiEndpointOption);
            await DeleteAllProjects(apiEndpoint ?? "https://api.Getitdone.com");
        });
        rootCommand.AddCommand(deleteAllProjectsCommand);

        var listTodoItemsCommand = new Command("list-todos", "Lists all todo items");
        listTodoItemsCommand.SetHandler(async (InvocationContext context) =>
        {
            string? apiEndpoint = context.ParseResult.GetValueForOption<string>(apiEndpointOption);
            await ListTodoItems(apiEndpoint ?? "https://api.Getitdone.com");
        });
        rootCommand.AddCommand(listTodoItemsCommand);

        var contentOption = new Option<string>("--content", "The content of the todo item") { IsRequired = true };
        var createTodoItemCommand = new Command("create-todo", "Creates a new todo item")
        {
            contentOption
        };
        createTodoItemCommand.SetHandler(async (InvocationContext context) =>
        {
            string? apiEndpoint = context.ParseResult.GetValueForOption<string>(apiEndpointOption);
            string content = context.ParseResult.GetValueForOption(contentOption)!;
            await CreateTodoItem(apiEndpoint ?? "https://api.Getitdone.com", content);
        });
        rootCommand.AddCommand(createTodoItemCommand);

        var todoIdOption = new Option<string>("--id", "The ID of the todo item") { IsRequired = true };
        var getTodoItemCommand = new Command("get-todo", "Gets a specific todo item by ID")
        {
            todoIdOption
        };
        getTodoItemCommand.SetHandler(async (InvocationContext context) =>
        {
            string? apiEndpoint = context.ParseResult.GetValueForOption<string>(apiEndpointOption);
            string id = context.ParseResult.GetValueForOption(todoIdOption)!;
            await GetTodoItem(apiEndpoint ?? "https://api.Getitdone.com", id);
        });
        rootCommand.AddCommand(getTodoItemCommand);

        var deleteTodoItemCommand = new Command("delete-todo", "Deletes a specific todo item by ID")
        {
            todoIdOption
        };
        deleteTodoItemCommand.SetHandler(async (InvocationContext context) =>
        {
            string? apiEndpoint = context.ParseResult.GetValueForOption<string>(apiEndpointOption);
            string id = context.ParseResult.GetValueForOption(todoIdOption)!;
            await DeleteTodoItem(apiEndpoint ?? "https://api.Getitdone.com", id);
        });
        rootCommand.AddCommand(deleteTodoItemCommand);

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

    private static async Task DeleteProject(string apiEndpoint, string id)
    {
        Console.WriteLine($"Deleting project with ID '{id}' using API endpoint: {apiEndpoint}...");

        if (string.IsNullOrWhiteSpace(id))
        {
            Console.WriteLine("Error: Project ID cannot be empty or whitespace.");
            return;
        }

        try
        {
            var client = new GetitdoneClient(new Uri(apiEndpoint), new GetitdoneClientOptions());
            var projectsClient = client.GetProjectsClient();
            await projectsClient.GetProjectsProjectOpsClient().DeleteProjectAsync(id);

            Console.WriteLine($"Project with ID '{id}' deleted successfully.");
        }
        catch (ClientResultException ex)
        {
            Console.WriteLine($"Error: Failed to delete project with ID '{id}'.");
            Console.WriteLine($"  Status Code: {ex.Status}");
            Console.WriteLine($"  Message: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    private static async Task DeleteAllProjects(string apiEndpoint)
    {
        Console.WriteLine($"Deleting all projects using API endpoint: {apiEndpoint}...");

        try
        {
            var client = new GetitdoneClient(new Uri(apiEndpoint), new GetitdoneClientOptions());
            var projectsClient = client.GetProjectsClient();
            var projects = await projectsClient.GetProjectsAsync();

            foreach (var project in projects.Value)
            {
                await projectsClient.GetProjectsProjectOpsClient().DeleteProjectAsync(project.Id);
            }

            Console.WriteLine($"All projects deleted successfully.");
        }
        catch (ClientResultException ex)
        {
            Console.WriteLine($"Error: Failed to delete all projects.");
            Console.WriteLine($"  Status Code: {ex.Status}");
            Console.WriteLine($"  Message: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    private static async Task ListTodoItems(string apiEndpoint)
    {
        Console.WriteLine($"Listing todo items using API endpoint: {apiEndpoint}...");
        var client = new GetitdoneClient(new Uri(apiEndpoint), new GetitdoneClientOptions());
        var todoItemsClient = client.GetTodoItemsClient();
        var todoItems = await todoItemsClient.GetTodoItemsAsync();

        foreach (var todoItem in todoItems.Value)
        {
            Console.WriteLine($"- {todoItem.Content} (ID: {todoItem.Id})");
        }
    }

    private static async Task CreateTodoItem(string apiEndpoint, string content)
    {
        Console.WriteLine($"Creating todo item with content '{content}' using API endpoint: {apiEndpoint}...");
        var client = new GetitdoneClient(new Uri(apiEndpoint), new GetitdoneClientOptions());
        var todoItemsClient = client.GetTodoItemsClient();
        var createRequest = new CreateTodoItemRequest(content);
        var todoItem = await todoItemsClient.CreateTodoItemAsync(createRequest);

        Console.WriteLine($"Created todo item with ID: {todoItem.Value.Id}");
    }

    private static async Task GetTodoItem(string apiEndpoint, string id)
    {
        Console.WriteLine($"Getting todo item with ID '{id}' using API endpoint: {apiEndpoint}...");

        if (string.IsNullOrWhiteSpace(id))
        {
            Console.WriteLine("Error: Todo item ID cannot be empty or whitespace.");
            return;
        }

        try
        {
            var client = new GetitdoneClient(new Uri(apiEndpoint), new GetitdoneClientOptions());
            var todoItemsClient = client.GetTodoItemsClient();
            var todoItem = await todoItemsClient.GetTodoItemsTodoItemOpsClient().GetTodoItemAsync(id);

            Console.WriteLine($"Todo Item Content: {todoItem.Value.Content}");
            Console.WriteLine($"Todo Item ID: {todoItem.Value.Id}");
        }
        catch (ClientResultException ex)
        {
            Console.WriteLine($"Error: Failed to get todo item with ID '{id}'.");
            Console.WriteLine($"  Status Code: {ex.Status}");
            Console.WriteLine($"  Message: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    private static async Task DeleteTodoItem(string apiEndpoint, string id)
    {
        Console.WriteLine($"Deleting todo item with ID '{id}' using API endpoint: {apiEndpoint}...");

        if (string.IsNullOrWhiteSpace(id))
        {
            Console.WriteLine("Error: Todo item ID cannot be empty or whitespace.");
            return;
        }

        try
        {
            var client = new GetitdoneClient(new Uri(apiEndpoint), new GetitdoneClientOptions());
            var todoItemsClient = client.GetTodoItemsClient();
            await todoItemsClient.GetTodoItemsTodoItemOpsClient().DeleteTodoItemAsync(id);

            Console.WriteLine($"Todo item with ID '{id}' deleted successfully.");
        }
        catch (ClientResultException ex)
        {
            Console.WriteLine($"Error: Failed to delete todo item with ID '{id}'.");
            Console.WriteLine($"  Status Code: {ex.Status}");
            Console.WriteLine($"  Message: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}