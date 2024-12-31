# Build a Todoist clone API with TypeSpec

## Table of contents

*   [Introduction](#introduction)
*   [Prerequisites](#prerequisites)
*   [Step 0: Generate service stub code from TypeSpec](#step-0-generate-service-code-with-typespec)
*   [Step 1: Create a new asp.net core web API project](#step-1-create-a-new-aspnet-core-web-api-project)
*   [Step 2: Organize the project structure](#step-2-organize-the-project-structure)
*   [Step 3: Add generated stub code](#step-3-add-generated-code)
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
*   [Step 12: Using the Client Example Application](#step-12-using-the-client-example-application)    
*   [Conclusion](#conclusion)

## Introduction

This guide demonstrates how to build a [Todoist](https://todoist.com/) (todo app) API clone using C# and .NET, leveraging service code generated from TypeSpec. We will create a service backend using TypeSpec-generated stub code and a pre-existing implementation generated using AI. For instructions on using AI to generate the implementation code, refer to the [getitdone-implementation-ai](../doc/getitdone-implementation-ai.md) document.

Please note, this project is only demonstrating a clone of the backend API and does not include a front-end application. The front-end is a simple HTML/JS/CSS application that interacts with the API to demonstrate basic functionality.

The TypeSpec file was itself also generated with the help of AI - I used Windows Copilot to scrape the API documentation, Azure OpenAI o1-preview to generate an OpenAPI spec from the scraped documentation, then to generate the TypeSpec file from the OpenAPI spec. I then used GitHub Copilot in VS Code to work through the various build issues in the original TypeSpec file, and once the file was clean, I was able to generate the service stub code. The repository contains the full implementation for the API, and this guide will focus on using the TypeSpec-generated stub code with that implementation.

## Prerequisites

Before you begin, ensure you have the following installed:

*   **TypeSpec tools:** These are needed to generate the service code. See the [README at the root of the repo](../README.md) for more information.
*   **.NET SDK:** Download and install the .NET SDK from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).
*   **Visual Studio Code (VS Code):** Download and install VS Code from [https://code.visualstudio.com/](https://code.visualstudio.com/).
*   **C# Extension for VS Code:** Install the C# extension in VS Code.
*   **Thunder Client Extension for VS Code:** Install the Thunder Client extension in VS Code.

## Step 0: Generate service stub code from TypeSpec

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

## Step 3: Add generated stub code

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
    This `generated` folder contains the stub code, including the controller base classes, generated from your TypeSpec file.

## Step 4: Create repository interfaces

The repository already contains the implementation for the repository interfaces. The following files are located in the `Repositories` folder:

*   `ICommentRepository.cs`
*   `ILabelRepository.cs`
*   `IProjectRepository.cs`
*   `ISectionRepository.cs`
*   `ITodoItemRepository.cs`

These interfaces define the contracts for interacting with the data layer.

## Step 5: Create in-memory repository implementations

The repository already contains the implementation for the in-memory repositories. The following files are located in the `Repositories/InMemory` folder:

*   `InMemoryCommentRepository.cs`
*   `InMemoryLabelRepository.cs`
*   `InMemoryProjectRepository.cs`
*   `InMemorySectionRepository.cs`
*   `InMemoryTodoItemRepository.cs`

These classes provide the concrete implementations for the repository interfaces, using in-memory data storage.

## Step 6: Create service layer implementations

The repository already contains the implementation for the service layer. The following files are located in the `Services` folder:

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

These classes implement the business logic for the API, using the repository interfaces.

## Step 7: Create controller helper

The repository already contains the implementation for the controller helper. The following file is located in the `Helpers` folder:

*   `ControllerHelpers.cs`

This class provides helper methods for handling errors in the controllers.

## Step 8: Create concrete controllers

The repository already contains the implementation for the concrete controllers. The following files are located in the `Controllers` folder:

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

These classes implement the API endpoints, using the service layer and the controller helper.

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

## Step 12: Using the Client Example Application

The `Getitdone.ClientExample` project is a C# console application that demonstrates how to use the generated client code to interact with the API. **This client code must be generated from your TypeSpec file before building this project.** This application can be used to test the API once the backend service is up and running.

### Prerequisites

*   Ensure that the API service is running, as described in [Step 10: Build and run the application](#step-10-build-and-run-the-application).
*   **Ensure that the client code has been generated from your TypeSpec file.** Refer to the TypeSpec documentation for instructions on generating client code.

### Running the Client

1.  **Open a terminal:** Open your terminal or command prompt.
2.  **Navigate to the client directory:** Navigate to the `Getitdone.ClientExample` directory.
3.  **Run the application:** Run the following command:

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