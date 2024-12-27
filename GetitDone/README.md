# Build a Todoist clone API with TypeSpec

## Table of contents

*   [Introduction](#introduction)
*   [Prerequisites](#prerequisites)
*   [Step 0: Generate service stub code from TypeSpec](#step-0-generate-service-code-with-typespec)
*   [Step 1: Create a new asp.net core web API project](#step-1-create-a-new-aspnet-core-web-api-project)
*   [Step 2: Organize the project structure](#step-2-organize-the-project-structure)
*   [Step 3: Add generated code](#step-3-add-generated-code)
*   [Step 4: Create repository interfaces](#step-4-create-repository-interfaces)
*   [Step 5: Create in-memory repository implementations](#step-5-create-in-memory-repository-implementations)
*   [Step 6: Create service layer implementations](#step-6-create-service-layer-implementations)
*   [Step 7: Create controller helper](#step-7-create-controller-helper)
*   [Step 8: Create concrete controllers](#step-8-create-concrete-controllers)
*   [Step 9: Configure dependency injection](#step-9-configure-dependency-injection)
*   [Step 10: Build and run the application](#step-10-build-and-run-the-application)
*   [Step 11: Test the API with thunder client and a simple html front-end](#step-11-test-the-api-with-thunder-client-and-a-simple-html-front-end)
    *   [Using thunder client](#using-thunder-client)
    *   [Using the simple html front-end](#using-the-simple-html-front-end)
*   [Conclusion](#conclusion)

## Introduction

This guide demonstrates how to build a [Todoist](https://todoist.com/) (todo app) API clone using C# and .NET, leveraging service code generated from TypeSpec. We will create a service backend using pre-generated code and AI to generate custom logic for a complete implementation that uses local memory as the backing memory store. Please note, this project is only demonstrating a clone of the backend API and does not include a front-end application. The front-end is a simple HTML/JS/CSS application that interacts with the API to demonstrate basic functionality.

Access to a state-of-the-art AI model is extremely helpful for this task if you don't want to hand-write a lot of code. I used Google's Gemini 2.0 Flash reasoning model, which is free with a Google account and offers a large context window (1M+ tokens). We'll test the API using the Thunder Client extension for VS Code and a simple HTML/JS/CSS front-end.

The TypeSpec file was itself also generated with the help of AI - I used Windows Copilot to scrape the API documentation, Azure OpenAI o1-preview to generate an OpenAPI spec from the scraped documentation, then to generate the TypeSpec file from the OpenAPI spec. I then used GitHub Copilot in VS Code to work through the various build issues in the original TypeSpec file, and once the file was clean, I was able to generate the service code.

## Prerequisites

Before you begin, ensure you have the following installed:

*   **TypeSpec tools:** These are needed to generate the service code. See the [README at the root of the repo](../README.md) for more information.
*   **.NET SDK:** Download and install the .NET SDK from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).
*   **Visual Studio Code (VS Code):** Download and install VS Code from [https://code.visualstudio.com/](https://code.visualstudio.com/).
*   **C# Extension for VS Code:** Install the C# extension in VS Code.
*   **Thunder Client Extension for VS Code:** Install the Thunder Client extension in VS Code.

## Step 0: Generate service stub code from TypeSpec

Refer to the [user-journey](../doc/user-journey.md) for details on how to generate the service code using TypeSpec.

## Step 1: Create a new asp.net core web API project

1.  **Open a terminal:** Open your terminal or command prompt.
2.  **Navigate to a directory:** Navigate to the directory where you want to create your project.
3.  **Create a top-level directory:** Create a new directory for your project named `Getitdone`.
4.  **Create the .NET project:** `cd` into the `GetitDone` directory and run the following command:

    ```bash
    dotnet new webapi -o Getitdone.Service
    ```

    This command creates a new ASP.NET Core Web API project in a folder named `Getitdone.Service`.

## Step 2: Organize the project structure

1.  **Open the project:** Open the `Getitdone.Service` folder in VS Code.
2.  **Create folders:** Create the following folders within the project:
    *   `Controllers`
    *   `Helpers`
    *   `Repositories`
    *   `Repositories/InMemory`
    *   `Services`

## Step 3: Add generated code

1.  **Copy generated files:** Copy the `generated` files folder from your TypeSpec project (located under `servers\aspnet\generated`) to the `GetitDone.Service` folder. Your `GetitDone.Service` folder should now look like this:

    ```text
    GetitDone.Service/
    ┣ Controllers/
    ┣ generated/
    ┣ Helpers/
    ┣ Repositories/
    ┣ Services/
    ┣ Getitdone.Service.csproj
    ┗ Program.cs
    ```

## Step 4: Create repository interfaces

1.  **Create files:** Create the following files in the `Repositories` folder:
    *   `ICommentRepository.cs`
    *   `ILabelRepository.cs`
    *   `IProjectRepository.cs`
    *   `ISectionRepository.cs`
    *   `ITodoItemRepository.cs`

2.  **Add code:** Use the following prompts with an AI agent to generate the code for each file. As the AI generates the code, copy and paste it into the appropriate file in your project.

    *   **Prompt for `ICommentRepository.cs`:**

        ```
        Create a C# interface named `ICommentRepository` in the namespace `Getitdone.Service.Repositories`. It should have the following methods:
        - `Task<Comment?> GetByIdAsync(string id)`
        - `Task<IEnumerable<Comment>> GetAllAsync(string? todoitemId, string? projectId)`
        - `Task<Comment> AddAsync(Comment comment)`
        - `Task<Comment> UpdateAsync(Comment comment)`
        - `Task DeleteAsync(string id)`
        Use the `Getitdone.Service.Models` namespace for the `Comment` type.
        ```

    *   **Prompt for `ILabelRepository.cs`:**

        ```
        Create a C# interface named `ILabelRepository` in the namespace `Getitdone.Service.Repositories`. It should have the following methods:
        - `Task<Label?> GetByIdAsync(string id)`
        - `Task<IEnumerable<Label>> GetAllAsync()`
        - `Task<Label> AddAsync(Label label)`
        - `Task<Label> UpdateAsync(Label label)`
        - `Task DeleteAsync(string id)`
        Use the `Getitdone.Service.Models` namespace for the `Label` type.
        ```

    *   **Prompt for `IProjectRepository.cs`:**

        ```
        Create a C# interface named `IProjectRepository` in the namespace `Getitdone.Service.Repositories`. It should have the following methods:
        - `Task<Project?> GetByIdAsync(string id)`
        - `Task<IEnumerable<Project>> GetAllAsync()`
        - `Task<Project> AddAsync(Project project)`
        - `Task<Project> UpdateAsync(Project project)`
        - `Task DeleteAsync(string id)`
        - `Task<IEnumerable<Collaborator>> GetCollaboratorsAsync(string projectId)`
        Use the `Getitdone.Service.Models` namespace for the `Project` and `Collaborator` types.
        ```

    *   **Prompt for `ISectionRepository.cs`:**

        ```
        Create a C# interface named `ISectionRepository` in the namespace `Getitdone.Service.Repositories`. It should have the following methods:
        - `Task<Section?> GetByIdAsync(string id)`
        - `Task<IEnumerable<Section>> GetAllAsync(string projectId)`
        - `Task<Section> AddAsync(Section section)`
        - `Task<Section> UpdateAsync(Section section)`
        - `Task DeleteAsync(string id)`
        Use the `Getitdone.Service.Models` namespace for the `Section` type.
        ```

    *   **Prompt for `ITodoItemRepository.cs`:**

        ```
        Create a C# interface named `ITodoItemRepository` in the namespace `Getitdone.Service.Repositories`. It should have the following methods:
        - `Task<TodoItem?> GetByIdAsync(string id)`
        - `Task<IEnumerable<TodoItem>> GetAllAsync()`
        - `Task<TodoItem> AddAsync(TodoItem todoItem)`
        - `Task<TodoItem> UpdateAsync(TodoItem todoItem)`
        - `Task DeleteAsync(string id)`
        Use the `Getitdone.Service.Models` namespace for the `TodoItem` type.
        ```

## Step 5: Create in-memory repository implementations

1.  **Create files:** Create the following files in the `Repositories/InMemory` folder:
    *   `InMemoryCommentRepository.cs`
    *   `InMemoryLabelRepository.cs`
    *   `InMemoryProjectRepository.cs`
    *   `InMemorySectionRepository.cs`
    *   `InMemoryTodoItemRepository.cs`

2.  **Add code:** Use the following prompts with an AI agent to generate the code for each file:

    *   **Prompt for `InMemoryCommentRepository.cs`:**

        ```
        Create a C# class named `InMemoryCommentRepository` in the namespace `Getitdone.Service.Repositories.InMemory` that implements the `ICommentRepository` interface. Use a `ConcurrentDictionary<string, Comment>` to store the data. Implement all the methods of the interface using the in-memory dictionary.
        Use the `Getitdone.Service.Models` namespace for the `Comment` type.
        ```

    *   **Prompt for `InMemoryLabelRepository.cs`:**

        ```
        Create a C# class named `InMemoryLabelRepository` in the namespace `Getitdone.Service.Repositories.InMemory` that implements the `ILabelRepository` interface. Use a `ConcurrentDictionary<string, Label>` to store the data. Implement all the methods of the interface using the in-memory dictionary.
        Use the `Getitdone.Service.Models` namespace for the `Label` type.
        ```

    *   **Prompt for `InMemoryProjectRepository.cs`:**

        ```
        Create a C# class named `InMemoryProjectRepository` in the namespace `Getitdone.Service.Repositories.InMemory` that implements the `IProjectRepository` interface. Use a `ConcurrentDictionary<string, Project>` to store the project data and a `ConcurrentDictionary<string, List<Collaborator>>` to store the collaborators. Implement all the methods of the interface using the in-memory dictionaries.
        Use the `Getitdone.Service.Models` namespace for the `Project` and `Collaborator` types.
        ```

    *   **Prompt for `InMemorySectionRepository.cs`:**

        ```
        Create a C# class named `InMemorySectionRepository` in the namespace `Getitdone.Service.Repositories.InMemory` that implements the `ISectionRepository` interface. Use a `ConcurrentDictionary<string, Section>` to store the data. Implement all the methods of the interface using the in-memory dictionary.
        Use the `Getitdone.Service.Models` namespace for the `Section` type.
        ```

    *   **Prompt for `InMemoryTodoItemRepository.cs`:**

        ```
        Create a C# class named `InMemoryTodoItemRepository` in the namespace `Getitdone.Service.Repositories.InMemory` that implements the `ITodoItemRepository` interface. Use a `ConcurrentDictionary<string, TodoItem>` to store the data. Implement all the methods of the interface using the in-memory dictionary.
        Use the `Getitdone.Service.Models` namespace for the `TodoItem` type.
        ```

## Step 6: Create service layer implementations

1.  **Create files:** Create the following files in the `Services` folder:
    *   `CommentOpsOperations.cs`
    *   `CommentsOperations.cs`
    *   `LabelOpsOperations.cs`
    *   `LabelsOperations.cs`
    *   `ProjectOpsOperations.cs`
    *   `ProjectsOperations.cs`
    *   `SectionOpsOperations.cs`
    *   `SectionsOperations.cs`
    *   `SharedLabelsOperations.cs`
    *   `TodoItemOpsOperations.cs`
    *   `TodoItemsOperations.cs`

2.  **Add code:** Use the following prompts with an AI agent to generate the code for each file:

    *   **Prompt for `CommentOpsOperations.cs`:**

        ```
        Create a C# class named `CommentOpsOperations` in the namespace `Getitdone.Service.Services` that implements the `ICommentOpsOperations` interface. Inject an `ICommentRepository` through the constructor. Implement all the methods of the interface, including error handling with try-catch blocks. Log exceptions to the console.
        Use the `Getitdone.Service.Models` namespace for the model types and `Getitdone.Service` namespace for the interface.
        ```

    *   **Prompt for `CommentsOperations.cs`:**

        ```
        Create a C# class named `CommentsOperations` in the namespace `Getitdone.Service.Services` that implements the `ICommentsOperations` interface. Inject an `ICommentRepository` through the constructor. Implement all the methods of the interface, including error handling with try-catch blocks. Log exceptions to the console.
        Use the `Getitdone.Service.Models` namespace for the model types and `Getitdone.Service` namespace for the interface.
        ```

    *   **Prompt for `LabelOpsOperations.cs`:**

        ```
        Create a C# class named `LabelOpsOperations` in the namespace `Getitdone.Service.Services` that implements the `ILabelOpsOperations` interface. Inject an `ILabelRepository` through the constructor. Implement all the methods of the interface, including error handling with try-catch blocks. Log exceptions to the console.
        Use the `Getitdone.Service.Models` namespace for the model types and `Getitdone.Service` namespace for the interface.
        ```

    *   **Prompt for `LabelsOperations.cs`:**

        ```
        Create a C# class named `LabelsOperations` in the namespace `Getitdone.Service.Services` that implements the `ILabelsOperations` interface. Inject an `ILabelRepository` through the constructor. Implement all the methods of the interface, including error handling with try-catch blocks. Log exceptions to the console.
        Use the `Getitdone.Service.Models` namespace for the model types and `Getitdone.Service` namespace for the interface.
        ```

    *   **Prompt for `ProjectOpsOperations.cs`:**

        ```
        Create a C# class named `ProjectOpsOperations` in the namespace `Getitdone.Service.Services` that implements the `IProjectOpsOperations` interface. Inject an `IProjectRepository` through the constructor. Implement all the methods of the interface, including error handling with try-catch blocks. Log exceptions to the console.
        Use the `Getitdone.Service.Models` namespace for the model types and `Getitdone.Service` namespace for the interface.
        ```

    *   **Prompt for `ProjectsOperations.cs`:**

        ```
        Create a C# class named `ProjectsOperations` in the namespace `Getitdone.Service.Services` that implements the `IProjectsOperations` interface. Inject an `IProjectRepository` through the constructor. Implement all the methods of the interface, including error handling with try-catch blocks. Log exceptions to the console.
        Use the `Getitdone.Service.Models` namespace for the model types and `Getitdone.Service` namespace for the interface.
        ```

    *   **Prompt for `SectionOpsOperations.cs`:**

        ```
        Create a C# class named `SectionOpsOperations` in the namespace `Getitdone.Service.Services` that implements the `ISectionOpsOperations` interface. Inject an `ISectionRepository` through the constructor. Implement all the methods of the interface, including error handling with try-catch blocks. Log exceptions to the console.
        Use the `Getitdone.Service.Models` namespace for the model types and `Getitdone.Service` namespace for the interface.
        ```

    *   **Prompt for `SectionsOperations.cs`:**

        ```
        Create a C# class named `SectionsOperations` in the namespace `Getitdone.Service.Services` that implements the `ISectionsOperations` interface. Inject an `ISectionRepository` through the constructor. Implement all the methods of the interface, including error handling with try-catch blocks. Log exceptions to the console.
        Use the `Getitdone.Service.Models` namespace for the model types and `Getitdone.Service` namespace for the interface.
        ```

    *   **Prompt for `SharedLabelsOperations.cs`:**

        ```
        Create a C# class named `SharedLabelsOperations` in the namespace `Getitdone.Service.Services` that implements the `ISharedLabelsOperations` interface. Inject an `ILabelRepository` through the constructor. Implement all the methods of the interface, including error handling with try-catch blocks. Log exceptions to the console.
        Use the `Getitdone.Service.Models` namespace for the model types and `Getitdone.Service` namespace for the interface.
        ```

    *   **Prompt for `TodoItemOpsOperations.cs`:**

        ```
        Create a C# class named `TodoItemOpsOperations` in the namespace `Getitdone.Service.Services` that implements the `ITodoItemOpsOperations` interface. Inject an `ITodoItemRepository` through the constructor. Implement all the methods of the interface, including error handling with try-catch blocks. Log exceptions to the console.
        Use the `Getitdone.Service.Models` namespace for the model types and `Getitdone.Service` namespace for the interface.
        ```

    *   **Prompt for `TodoItemsOperations.cs`:**

        ```
        Create a C# class named `TodoItemsOperations` in the namespace `Getitdone.Service.Services` that implements the `ITodoItemsOperations` interface. Inject an `ITodoItemRepository` through the constructor. Implement all the methods of the interface, including error handling with try-catch blocks. Log exceptions to the console.
        Use the `Getitdone.Service.Models` namespace for the model types and `Getitdone.Service` namespace for the interface.
        ```

## Step 7: Create controller helper

1.  **Create file:** Create a file named `ControllerHelpers.cs` in the `Helpers` folder.

2.  **Add code:** Use the following prompt with an AI agent to generate the code for the file:

    *   **Prompt for `ControllerHelpers.cs`:**

        ```
        Create a static C# class named `ControllerHelpers` in the namespace `Getitdone.Service.Helpers`. It should have a static method named `HandleErrorResponse` that takes an `Exception` as a parameter and returns an `IActionResult`. The method should return a `NotFound` response for `KeyNotFoundException` and an `InternalServerError` response for other exceptions. Use the `Getitdone.Service.Models` namespace for the `ErrorResponse` type.
        ```

## Step 8: Create concrete controllers

1.  **Create files:** Create the following files in the `Controllers` folder:
    *   `CommentOpsController.cs`
    *   `CommentsController.cs`
    *   `LabelOpsController.cs`
    *   `LabelsController.cs`
    *   `ProjectOpsController.cs`
    *   `ProjectsController.cs`
    *   `SectionOpsController.cs`
    *   `SectionsController.cs`
    *   `SharedLabelsController.cs`
    *   `TodoItemOpsController.cs`
    *   `TodoItemsController.cs`

2.  **Add code:** For each of these files, use the following prompt with an AI, replacing `[ControllerName]` with the actual name of the controller (e.g., `CommentOpsController`):

    ```
    Create a C# class named `[ControllerName]` in the namespace `Getitdone.Service.Controllers` that inherits from the corresponding generated controller base class (e.g., `CommentOpsOperationsControllerBase`). Inject the corresponding service interface (e.g., `ICommentOpsOperations`) through the constructor. Implement all the methods of the base class, using the injected service and the `ControllerHelpers.HandleErrorResponse` method for error handling.
    Use the `Getitdone.Service.Models` namespace for the model types, `Getitdone.Service.Helpers` for the `ControllerHelpers` class, and `Getitdone.Service` for the service interface.
    ```

## Step 9: Configure dependency injection

1.  **Open `Program.cs`:** Open the `Program.cs` file in your project.
2.  **Add DI registrations:** Add the following code to register the repositories and services with the dependency injection container:

    ```csharp
    using Getitdone.Service.Repositories;
    using Getitdone.Service.Repositories.InMemory;
    using Getitdone.Service.Services;
    using Getitdone.Service;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // Add CORS configuration
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    // Register Repositories
    builder.Services.AddSingleton<ICommentRepository, InMemoryCommentRepository>();
    builder.Services.AddSingleton<ILabelRepository, InMemoryLabelRepository>();
    builder.Services.AddSingleton<IProjectRepository, InMemoryProjectRepository>();
    builder.Services.AddSingleton<ISectionRepository, InMemorySectionRepository>();
    builder.Services.AddSingleton<ITodoItemRepository, InMemoryTodoItemRepository>();

    // Register Services
    builder.Services.AddScoped<ICommentOpsOperations, CommentOpsOperations>();
    builder.Services.AddScoped<ICommentsOperations, CommentsOperations>();
    builder.Services.AddScoped<ILabelOpsOperations, LabelOpsOperations>();
    builder.Services.AddScoped<ILabelsOperations, LabelsOperations>();
    builder.Services.AddScoped<IProjectOpsOperations, ProjectOpsOperations>();
    builder.Services.AddScoped<IProjectsOperations, ProjectsOperations>();
    builder.Services.AddScoped<ISectionOpsOperations, SectionOpsOperations>();
    builder.Services.AddScoped<ISectionsOperations, SectionsOperations>();
    builder.Services.AddScoped<ISharedLabelsOperations, SharedLabelsOperations>();
    builder.Services.AddScoped<ITodoItemOpsOperations, TodoItemOpsOperations>();
    builder.Services.AddScoped<ITodoItemsOperations, TodoItemsOperations>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseCors();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
    ```

## Step 10: Build and run the application

1.  **Open a terminal:** Open your terminal or command prompt.
2.  **Navigate to the project directory:** Navigate to the `Getitdone.Service` directory.
3.  **Build the project:** Run the following command:

    ```bash
    dotnet build
    ```

4.  **Run the project:** After a successful build, run the project:

    ```bash
    dotnet run
    ```

    The console output will display the base URL where your API is running (e.g., `http://localhost:5000` or `https://localhost:7000`).

## Step 11: Test the API with thunder client and a simple html front-end

### Using thunder client

1.  **Open thunder client:** In VS Code, click on the Thunder Client icon in the Activity Bar.
2.  **Create requests:** Use the following user journey to test your API:

    *   **Create a new project:**
        *   **Method:** `POST`
        *   **URL:** `http://localhost:5000/projects` (or your base URL)
        *   **Headers:** `Content-Type: application/json`
        *   **Body:**

            ```json
            {
              "name": "My First Project",
              "color": "#3498db",
              "parentId": null,
              "order": 1,
              "isFavorite": false
            }
            ```

        *   **Verify:** `201 Created` status code and a JSON body with the new project's details, including its `id`.

    *   **Get all projects:**
        *   **Method:** `GET`
        *   **URL:** `http://localhost:5000/projects`
        *   **Verify:** `200 OK` status code and a JSON body with an array of projects.

    *   **Create a new section:**
        *   **Method:** `POST`
        *   **URL:** `http://localhost:5000/sections`
        *   **Headers:** `Content-Type: application/json`
        *   **Body:**

            ```json
            {
              "name": "My First Section",
              "projectId": "{project_id}",
              "order": 1
            }
            ```

            (Replace `{project_id}` with the `id` from the previous step)
        *   **Verify:** `201 Created` status code and a JSON body with the new section's details, including its `id`.

    *   **Get all sections for a project:**
        *   **Method:** `GET`
        *   **URL:** `http://localhost:5000/sections?project_id={project_id}` (Replace `{project_id}` with the project ID)
        *   **Verify:** `200 OK` status code and a JSON body with an array of sections.

    *   **Create a new todo item:**
        *   **Method:** `POST`
        *   **URL:** `http://localhost:5000/todoitems`
        *   **Headers:** `Content-Type: application/json`
        *   **Body:**

            ```json
            {
              "content": "My First Task",
              "description": "This is my first task in this project",
              "due": {
                "date": null,
                "isRecurring": false,
                "datetime": null,
                "string": null,
                "timezone": null
              },
              "labels": [],
              "priority": 1,
              "parentId": null,
              "order": 1,
              "projectId": "{project_id}",
              "sectionId": "{section_id}",
              "assigneeId": null
            }
            ```

            (Replace `{project_id}` and `{section_id}` with the appropriate IDs)
        *   **Verify:** `201 Created` status code and a JSON body with the new todo item's details, including its `id`.

    *   **Get all todo items:**
        *   **Method:** `GET`
        *   **URL:** `http://localhost:5000/todoitems`
        *   **Verify:** `200 OK` status code and a JSON body with an array of todo items.

    *   **Create a new label:**
        *   **Method:** `POST`
        *   **URL:** `http://localhost:5000/labels`
        *   **Headers:** `Content-Type: application/json`
        *   **Body:**

            ```json
            {
              "name": "My First Label",
              "color": "#e74c3c",
              "order": 1,
              "isFavorite": true
            }
            ```

        *   **Verify:** `201 Created` status code and a JSON body with the new label's details, including its `id`.

    *   **Get all labels:**
        *   **Method:** `GET`
        *   **URL:** `http://localhost:5000/labels`
        *   **Verify:** `200 OK` status code and a JSON body with an array of labels.

    *   **Update a todo item with a label:**
        *   **Method:** `POST`
        *   **URL:** `http://localhost:5000/todoitems/{todoitem_id}` (Replace `{todoitem_id}` with the todo item ID)
        *   **Headers:** `Content-Type: application/json`
        *   **Body:**

            ```json
            {
              "content": "My First Task",
              "description": "This is my first task in this project",
              "due": {
                "date": null,
                "isRecurring": false,
                "datetime": null,
                "string": null,
                "timezone": null
              },
              "labels": ["{label_id}"],
              "priority": 1,
              "parentId": null,
              "order": 1,
              "projectId": "{project_id}",
              "sectionId": "{section_id}",
              "assigneeId": null
            }
            ```

            (Replace `{todoitem_id}`, `{label_id}`, `{project_id}`, and `{section_id}` with the appropriate IDs)
        *   **Verify:** `200 OK` status code and a JSON body with the updated todo item.

    *   **Get a todo item:**
        *   **Method:** `GET`
        *   **URL:** `http://localhost:5000/todoitems/{todoitem_id}` (Replace `{todoitem_id}` with the todo item ID)
        *   **Verify:** `200 OK` status code and a JSON body with the todo item.

### Using the simple html front-end

1.  **Locate `index.html`:** The `index.html` file is located in the `Getitdone.Frontend` folder within your project.
2.  **Open in browser:** Open the `index.html` file in your web browser. You can do this by right-clicking the file in VS Code, selecting "Reveal in File Explorer", then double-clicking on the file to open it in your browser.
3.  **Interact with the API:**
    *   Enter data into the input fields.
    *   Click the buttons to trigger API requests.
    *   View the responses in the output area.

**Key points for using the front-end:**

*   **API URL:** Ensure that the `apiUrl` variable in the `index.html` file (located within the `<script>` tag) is set to the correct base URL where your API is running (e.g., `http://localhost:5000`, `https://localhost:7000`, or `http://localhost:5091`). You can find this URL in the console output when you run your API using `dotnet run`.
*   **Input fields:** Use the input fields to provide data for creating projects, sections, todo items, and labels.
*   **Buttons:** Click the buttons to trigger the corresponding API requests.
*   **Output area:** The responses from the API will be displayed in the output area below the buttons.
*   **Basic functionality:** This front-end provides basic functionality for creating and retrieving resources. It is intended for demonstration and testing purposes.

## Conclusion

By following this guide, you have successfully built a Todoist clone API using C# and .NET service code generated from TypeSpec.
