# Build a Todoist clone API with TypeSpec

## Table of contents

*   [Introduction](#introduction)
*   [Project Overview](#project-overview)
    *   [Repository Interfaces](#repository-interfaces)
    *   [In-Memory Repository Implementations](#in-memory-repository-implementations)
    *   [Service Layer Implementations](#service-layer-implementations)
    *   [Controller Helper](#controller-helper)
    *   [Concrete Controllers](#concrete-controllers)
    *   [Dependency Injection Configuration](#dependency-injection-configuration)
*   [Prerequisites](#prerequisites)
*   [Step 1: Generate service stub code from TypeSpec](#step-1-generate-service-stub-code-from-typespec)
*   [Step 2: Add generated stub code to existing webapi service project](#step-2-add-generated-stub-code-to-existing-webapi-service-project)
*   [Step 3: Build and run the application](#step-3-build-and-run-the-application)
*   [Step 4: Test the API with thunder client and a simple html front-end](#step-4-test-the-api-with-thunder-client-and-a-simple-html-front-end)
    *   [Using thunder client](#using-thunder-client)
    *   [Using the simple html front-end](#using-the-simple-html-front-end)
*   [Step 5: Using the Client Example Application](#step-5-using-the-client-example-application)
*   [Step 6: Using the CLI Tool](#step-6-using-the-cli-tool)

## Introduction

This guide demonstrates how to build a [Todoist](https://todoist.com/) (todo app) API clone using C# and .NET, leveraging service code generated from TypeSpec. We will create a service backend using TypeSpec-generated stub code and a pre-existing implementation generated using AI. The TypeSpec file was itself also generated with the help of AI.  
For a walkthrough on how I used AI to generate the TypeSpec file and the implementation code, refer to the [getitdone-implementation-ai](../doc/getitdone/getitdone-implementation-ai.md) document.

Please note, this project is only demonstrating a clone of the backend API and does not include a front-end application. The front-end is a simple HTML/JS/CSS application that interacts with the API to demonstrate basic functionality. A more sophisticated front end using the JavaScript/TypeScript client code bundled into a browser experience is a work in progress.

## Project Overview

The project repository includes all necessary files and implementations for a proof-of-concept of a C# webapi backend, using local memory as the backing memory storage and the service stub code generated from TypeSpec. Below is an overview of the included components and their locations within the project.

### Repository Interfaces

The `Repositories` folder contains the following repository interfaces:

* `ICommentRepository.cs`
* `ILabelRepository.cs`
* `IProjectRepository.cs`
* `ISectionRepository.cs`
* `ITodoItemRepository.cs`

These interfaces define the contracts for interacting with the data layer.

### In-Memory Repository Implementations

The `Repositories/InMemory` folder contains the in-memory repository implementations:

* `InMemoryCommentRepository.cs`
* `InMemoryLabelRepository.cs`
* `InMemoryProjectRepository.cs`
* `InMemorySectionRepository.cs`
* `InMemoryTodoItemRepository.cs`

These classes provide concrete implementations for the repository interfaces using in-memory data storage.

### Service Layer Implementations

The `Services` folder contains the service layer implementations:

* `CommentOpsOperations.cs`
* `CommentsOperations.cs`
* `LabelOpsOperations.cs`
* `LabelsOperations.cs`
* `ProjectOpsOperations.cs`
* `ProjectsOperations.cs`
* `SectionOpsOperations.cs`
* `SectionsOperations.cs`
* `SharedLabelsOperations.cs`
* `TodoItemOpsOperations.cs`
* `TodoItemsOperations.cs`

These classes implement the business logic for the API.

### Controller Helper

The `Helpers` folder contains the controller helper:

* `ControllerHelpers.cs`

This class provides helper methods for handling errors in the controllers.

### Concrete Controllers

The `Controllers` folder contains the concrete controllers:

* `CommentOpsController.cs`
* `CommentsController.cs`
* `LabelOpsController.cs`
* `LabelsController.cs`
* `ProjectOpsController.cs`
* `ProjectsController.cs`
* `SectionOpsController.cs`
* `SectionsController.cs`
* `SharedLabelsController.cs`
* `TodoItemOpsController.cs`
* `TodoItemsController.cs`

These classes implement the API endpoints using the service layer and the controller helper.

### Dependency Injection Configuration

The `Program.cs` file includes the necessary code to configure dependency injection for the repositories and services. Here is the relevant code snippet:

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

This configuration ensures that the repositories and services are properly registered with the dependency injection container.

## Prerequisites

Before you begin, ensure you have the following installed:

*   **TypeSpec tools:** These are needed to generate the service code. See the [README at the root of the repo](../README.md) for more information.
*   **.NET SDK:** Download and install the .NET SDK from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).
*   **Visual Studio Code (VS Code):** Download and install VS Code from [https://code.visualstudio.com/](https://code.visualstudio.com/).
*   **C# Extension for VS Code:** Install the C# extension in VS Code.
*   **Thunder Client Extension for VS Code:** Install the Thunder Client extension in VS Code.

## Step 1: Generate service stub code from TypeSpec

This step will generate the basic structure and interfaces for your API. The implementation will be provided by the existing code in the repository.

Open a terminal window and navigate to your `typespec-e2e-demo\GetitDone` directory. Run the following commands to generate the service code:
```
npm install

# This should generate openapi and client code for csharp/typescript as well as server code for csharp. 

npx tsp compile .
```

After running the above commands, your `GetitDone` directory should look like this:
```text
GetitDone/
┣ clients/
┣ Getitdone.ClientExample/
┣ GetitDone.Frontend/
┣ GetitDone.Service/
┣ node_modules/
┣ openapi/
┣ servers/
┣ .gitignore
┣ API_documentation.md
┣ main.tsp
┣ package-lock.json
┣ package.json
┣ README.md
┗ tspconfig.yaml
```

## Step 2: Add generated stub code to existing webapi service project

Copy the `generated` files folder from `GetitDone\servers\aspnet\generated`) to the `GetitDone.Service` folder. Your `GetitDone.Service` folder should now look like this:

```text
GetitDone.Service/
┣ Controllers/
┣ generated/
┣ Helpers/
┣ Properties/
┣ Repositories/
┣ Services/
┣ appsettings.Development.json
┣ appsettings.json
┣ Getitdone.Service.csproj
┣ Getitdone.Service.http
┗ Program.cs
```
This `generated` folder contains the stub code, including the controller base classes, generated from your TypeSpec file.

## Step 3: Build and run the application

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

    The console output will display the base URL where your API is running (e.g., `http://localhost:5091`).

## Step 4: Test the API with thunder client and a simple html front-end

### Using thunder client

1.  **Open thunder client:** In VS Code, click on the Thunder Client icon in the Activity Bar.
2.  **Create requests:** Use the following user journey to test your API:

    *   **Create a new project:**
        *   **Method:** `POST`
        *   **URL:** `http://localhost:5091/projects` (or your base URL)
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
        *   **URL:** `http://localhost:5091/projects`
        *   **Verify:** `200 OK` status code and a JSON body with an array of projects.

    *   **Create a new section:**
        *   **Method:** `POST`
        *   **URL:** `http://localhost:5091/sections`
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
        *   **URL:** `http://localhost:5091/sections?project_id={project_id}` (Replace `{project_id}` with the project ID)
        *   **Verify:** `200 OK` status code and a JSON body with an array of sections.

    *   **Create a new todo item:**
        *   **Method:** `POST`
        *   **URL:** `http://localhost:5091/todoitems`
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
        *   **URL:** `http://localhost:5091/todoitems`
        *   **Verify:** `200 OK` status code and a JSON body with an array of todo items.

    *   **Create a new label:**
        *   **Method:** `POST`
        *   **URL:** `http://localhost:5091/labels`
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
        *   **URL:** `http://localhost:5091/labels`
        *   **Verify:** `200 OK` status code and a JSON body with an array of labels.

    *   **Update a todo item with a label:**
        *   **Method:** `POST`
        *   **URL:** `http://localhost:5091/todoitems/{todoitem_id}` (Replace `{todoitem_id}` with the todo item ID)
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
        *   **URL:** `http://localhost:5091/todoitems/{todoitem_id}` (Replace `{todoitem_id}` with the todo item ID)
        *   **Verify:** `200 OK` status code and a JSON body with the todo item.

### Using the simple html front-end

1.  **Locate `index.simple.html`:** The `index.simple.html` file is located in the `Getitdone.Frontend` folder within your project.
2.  **Open in browser:** Open the `index.simple.html` file in your web browser. You can do this by right-clicking the file in VS Code, selecting "Reveal in File Explorer", then double-clicking on the file to open it in your browser.
3.  **Interact with the API:**
    *   Enter data into the input fields.
    *   Click the buttons to trigger API requests.
    *   View the responses in the output area.

**Key points for using the front-end:**

*   **API URL:** Ensure that the `apiUrl` variable in the `index.simple.html` file (located within the `<script>` tag) is set to the correct base URL where your API is running (e.g., `http://localhost:5091`). You can find this URL in the console output when you run your API using `dotnet run`.
*   **Input fields:** Use the input fields to provide data for creating projects, sections, todo items, and labels.
*   **Buttons:** Click the buttons to trigger the corresponding API requests.
*   **Output area:** The responses from the API will be displayed in the output area on the right of the screen.
*   **Basic functionality:** This front-end provides basic functionality for creating and retrieving resources. It is intended for demonstration and testing purposes.

## Step 5: Using the Client Example Application

The `Getitdone.ClientExample` project is a console application that demonstrates how to use the generated C# client code to interact with the API. **The client code must be generated from your TypeSpec file before building this project, as described in step 1.** This application can be used to test the API once the backend service is up and running.

### Prerequisites

*   Ensure that the API service is running, as described in [Step 3: Build and run the application](#step-3-build-and-run-the-application).
*   **Ensure that the client code has been generated from your TypeSpec file.**

### Building the Client

In a terminal, navigate to the `Getitdone\clients\csharp\` directory. Run the following command to build the client example application:

```bash
dotnet build
```

### Running the Client

In your terminal, navigate to the `Getitdone.ClientExample` directory and run the following command:

```bash
dotnet run
```

### Expected Output

The console application will perform a series of API calls, including:

*   Retrieving existing projects, sections, todo items, and labels.
*   Creating new projects, sections, todo items, and labels.
*   Retrieving comments for todo items.
*   Creating new comments for todo items.

The output will display the details of the retrieved and created resources, including their names and IDs.

### Code Overview

The `Program.cs` file in the `Getitdone.ClientExample` project demonstrates the following:

*   **Creating a client instance:** It initializes a `GetitdoneClient` with the base URL of the API.
*   **Accessing API clients:** It uses the `GetProjectsClient()`, `GetSectionsClient()`, `GetTodoItemsClient()`, `GetCommentsClient()`, and `GetLabelsClient()` methods to get specific API clients.
*   **Making API calls:** It uses the methods of the API clients to make requests to the API, such as `GetProjectsAsync()`, `CreateSectionAsync()`, `GetTodoItemsAsync()`, etc.
*   **Using the Model Factory:** It uses the `GetitdoneClientModelFactory` to create request objects for the API.
*   **Handling responses:** It processes the responses from the API and prints the results to the console.

This example provides a basic demonstration of how to use the generated client code to interact with the API. You can modify this code to perform more complex operations or to test specific scenarios.

## Step 6: Using the CLI Tool

The `GetitDone.CLI` project is a command-line interface tool that allows you to interact with the Getitdone API directly from your terminal. This tool provides a convenient way to manage your projects, todo items, and other resources.

### Prerequisites

*   Ensure that the API service is running, as described in [Step 3: Build and run the application](#step-3-build-and-run-the-application).
*   **Ensure that the client code has been generated from your TypeSpec file.**
*   Ensure that the CLI project has a reference to the generated client code.

### Building the CLI

In a terminal, navigate to the `Getitdone\GetitDone.CLI\` directory. Run the following command to build the CLI application:

```bash
dotnet build
```

### Running the CLI

In your terminal, navigate to the `Getitdone\GetitDone.CLI\` directory and run the following command:

```bash
dotnet run <command> [options]
```

Replace `<command>` and `[options]` with the desired command and options.

### Available Commands

The CLI supports the following commands:

*   **`list-projects`**: Lists all projects.
    *   **Options:**
        *   `--api-endpoint`: Specifies the API endpoint (defaults to `https://api.Getitdone.com`).
*   **`create-project`**: Creates a new project.
    *   **Options:**
        *   `--name`: (Required) The name of the project.
        *   `--color`: The color of the project.
        *   `--api-endpoint`: Specifies the API endpoint (defaults to `https://api.Getitdone.com`).
*   **`get-project`**: Gets a specific project by ID.
    *   **Options:**
        *   `--id`: (Required) The ID of the project.
        *   `--api-endpoint`: Specifies the API endpoint (defaults to `https://api.Getitdone.com`).
*   **`delete-project`**: Deletes a specific project by ID.
    *   **Options:**
        *   `--id`: (Required) The ID of the project.
        *   `--api-endpoint`: Specifies the API endpoint (defaults to `https://api.Getitdone.com`).
*   **`delete-all-projects`**: Deletes all projects.
    *   **Options:**
        *   `--api-endpoint`: Specifies the API endpoint (defaults to `https://api.Getitdone.com`).
*   **`list-todos`**: Lists all todo items.
    *   **Options:**
        *   `--api-endpoint`: Specifies the API endpoint (defaults to `https://api.Getitdone.com`).
*   **`create-todo`**: Creates a new todo item.
    *   **Options:**
        *   `--content`: (Required) The content of the todo item.
        *   `--api-endpoint`: Specifies the API endpoint (defaults to `https://api.Getitdone.com`).
*   **`get-todo`**: Gets a specific todo item by ID.
    *   **Options:**
        *   `--id`: (Required) The ID of the todo item.
        *   `--api-endpoint`: Specifies the API endpoint (defaults to `https://api.Getitdone.com`).
*   **`delete-todo`**: Deletes a specific todo item by ID.
    *   **Options:**
        *   `--id`: (Required) The ID of the todo item.
        *   `--api-endpoint`: Specifies the API endpoint (defaults to `https://api.Getitdone.com`).

### Examples

*   **List all projects:**
*   ```bash
    dotnet run list-projects --api-endpoint http://localhost:5091
    ```

*   **Create a new project:**

    ```bash
    dotnet run create-project --name "My New Project" --color "blue" --api-endpoint http://localhost:5091
    ```

*   **Get a specific project:**

    ```bash
    dotnet run get-project --id "your-project-id" --api-endpoint http://localhost:5091
    ```

*   **Delete a specific project:**

    ```bash
    dotnet run delete-project --id "your-project-id" --api-endpoint http://localhost:5091
    ```

*   **Delete all projects:**

    ```bash
    dotnet run delete-all-projects --api-endpoint http://localhost:5091
    ```

*   **List all todo items:**

    ```bash
    dotnet run list-todos --api-endpoint http://localhost:5091
    ```

*   **Create a new todo item:**

    ```bash
    dotnet run create-todo --content "My new task" --api-endpoint http://localhost:5091
    ```

*   **Get a specific todo item:**

    ```bash
    dotnet run get-todo --id "your-todo-id" --api-endpoint http://localhost:5091
    ```

*   **Delete a specific todo item:**

    ```bash
    dotnet run delete-todo --id "your-todo-id" --api-endpoint http://localhost:5091
    ```
