using Getitdone.Client;
using Getitdone.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ClientModel;
using System.Linq;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Create a client instance
        var client = new GetitdoneClient(new Uri("http://localhost:5091"), new GetitdoneClientOptions());

        // Get the projects client
        var projectsClient = client.GetProjectsClient();

        // Get all projects
        ClientResult<IList<Project>> projectsResult = await projectsClient.GetProjectsAsync();

        foreach (var project in projectsResult.Value)
        {
            Console.WriteLine($"Project Name: {project.Name}, ID: {project.Id}");

            // Get the sections client
            var sectionsClient = client.GetSectionsClient();

            // Get sections for the project
            ClientResult<IList<Section>> sectionsResult = await sectionsClient.GetSectionsAsync(project.Id);

            foreach (var section in sectionsResult.Value)
            {
                Console.WriteLine($"  Section Name: {section.Name}, ID: {section.Id}");
            }

            // Create a new section
            var createSectionRequest = GetitdoneClientModelFactory.CreateSectionRequest(name: "My New Section", projectId: project.Id);
            ClientResult<Section> createSectionResult = await sectionsClient.CreateSectionAsync(createSectionRequest);

            Console.WriteLine($"  Created section with ID: {createSectionResult.Value.Id}");
        }

        // Create a new project
        var createProjectRequest = GetitdoneClientModelFactory.CreateProjectRequest(name: "My New Project", color: "red");
        ClientResult<Project> createProjectResult = await projectsClient.CreateProjectAsync(createProjectRequest);

        Console.WriteLine($"Created project with ID: {createProjectResult.Value.Id}");

        // Get the todo items client
        var todoItemsClient = client.GetTodoItemsClient();

        // Get all todo items
        ClientResult<IList<TodoItem>> todoItemsResult = await todoItemsClient.GetTodoItemsAsync();

        foreach (var todoItem in todoItemsResult.Value)
        {
            Console.WriteLine($"Todo Item Content: {todoItem.Content}, ID: {todoItem.Id}");

            // Get the comments client
            var commentsClient = client.GetCommentsClient();

            // Get comments for the todo item
            ClientResult<IList<Comment>> commentsResult = await commentsClient.GetCommentsAsync(todoItem.Id, todoItem.ProjectId);

            foreach (var comment in commentsResult.Value)
            {
                Console.WriteLine($"    Comment Content: {comment.Content}, ID: {comment.Id}");
            }

            // Create a new comment
            var createCommentRequest = GetitdoneClientModelFactory.CreateCommentRequest(content: "My New Comment", todoitemId: todoItem.Id, projectId: todoItem.ProjectId);
            ClientResult<Comment> createCommentResult = await commentsClient.CreateCommentAsync(createCommentRequest);

            Console.WriteLine($"    Created comment with ID: {createCommentResult.Value.Id}");
        }

        // Create a new todo item
        var createTodoItemRequest = GetitdoneClientModelFactory.CreateTodoItemRequest(content: "My New Todo Item");
        ClientResult<TodoItem> createTodoItemResult = await todoItemsClient.CreateTodoItemAsync(createTodoItemRequest);

        Console.WriteLine($"Created todo item with ID: {createTodoItemResult.Value.Id}");

        // Get the labels client
        var labelsClient = client.GetLabelsClient();

        // Get personal labels
        ClientResult<IList<Label>> labelsResult = await labelsClient.GetPersonalLabelsAsync();

        foreach (var label in labelsResult.Value)
        {
            Console.WriteLine($"Label Name: {label.Name}, ID: {label.Id}");
        }

        // Create a new label
        var createLabelRequest = GetitdoneClientModelFactory.CreateLabelRequest(name: "My New Label", color: "blue");
        ClientResult<Label> createLabelResult = await labelsClient.CreateLabelAsync(createLabelRequest);

        Console.WriteLine($"Created label with ID: {createLabelResult.Value.Id}");
    }
}