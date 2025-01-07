# Generating Implementation Code with AI for GetitDone - the Todoist Clone API

## Introduction

This document provides guidance on using an AI model to generate the implementation code for the Todoist clone API. This process involves using prompts to instruct the AI to create the necessary C# classes and interfaces for the repository, service, and controller layers. This document is intended for users who want to understand the AI-assisted code generation process, or who want to customize the implementation beyond what is provided in the main guide.

**Note:** The main guide assumes you are using pre-generated implementation code. This document is for those who want to explore the AI-assisted code generation process.

## Prerequisites

Before you begin, ensure you have the following:

* **Access to AI Models:** You will need access to a capable AI model with a large context window. Examples include Google Gemini 2.0 Flash, OpenAI's GPT models, or similar. You'll also need access to Windows Copilot in your web browser and GitHub Copilot in Visual Studio Code.
* **TypeSpec-Generated Stub Code:** You should have already generated the service stub code from your TypeSpec file, as described in the main guide.
* **Basic Understanding of C# and .NET:** Familiarity with C# syntax and .NET concepts is helpful.
* **Visual Studio Code (VS Code):** VS Code is recommended for code editing.

## Generating the TypeSpec File

To generate the TypeSpec file, follow these steps:

1. **Scrape the Todoist API Documentation:**
    - Use Windows Copilot to scrape the [Todoist API documentation](https://developer.todoist.com/rest/v2/#overview).
        - **Prompt:** "Extract all the API routes, models, and responses from this page."
        - **Response:** Copilot provided a list of API routes, models, and responses, including endpoints for projects, sections, tasks, comments, and labels.

2. **Generate an OpenAPI Specification:**
    - Use Azure OpenAI o1-preview to generate an OpenAPI spec from the scraped documentation.

3. **Generate a Draft TypeSpec File:**
    - Use Azure OpenAI o1-preview again to generate a draft TypeSpec file from the OpenAPI spec.

4. **Refine the TypeSpec File:**
    - Use GitHub Copilot in VS Code to work through the various errors in the TypeSpec code. Once the file is clean, you can generate the service stub code.

### Example Prompts and Responses

Here are the distilled prompts and responses used during the interaction with Copilot:

1. **Scraping the Documentation:**
    - **Prompt:** "Extract all the API routes, models, and responses from this page."
    - **Response:** Copilot provided a list of API routes, models, and responses, including endpoints for projects, sections, tasks, comments, and labels.

2. **Generating the OpenAPI Spec:**
    - **Prompt:** "Generate an OpenAPI spec from the extracted API routes, models, and responses."
    - **Response:** Copilot generated an OpenAPI specification based on the provided API details.

3. **Generating the TypeSpec File:**
    - **Prompt:** "Generate a TypeSpec file from the OpenAPI spec."
    - **Response:** Copilot created a draft TypeSpec file, which was then refined using GitHub Copilot.

## Rationale for Using Different AI Models/Tools

Different AI models and tools were used for various steps in the process to leverage their unique strengths:

1. **Windows Copilot:**
    - **Reason:** Efficiently scrape structured data from web pages.
    - **Usage:** Scraping the Todoist API documentation.

2. **Azure OpenAI o1-preview:**
    - **Reason:** Generate structured specifications (OpenAPI and TypeSpec) from unstructured data.
    - **Usage:** Generating the OpenAPI spec and draft TypeSpec file.

3. **GitHub Copilot:**
    - **Reason:** Assist with code completion and error resolution in an integrated development environment.
    - **Usage:** Refining the TypeSpec file in VS Code.

## Step 1: Generate Repository Interfaces

1.  **Create files:** Create the following files in the `Repositories` folder of your `Getitdone.Service` project:
    *   `ICommentRepository.cs`
    *   `ILabelRepository.cs`
    *   `IProjectRepository.cs`
    *   `ISectionRepository.cs`
    *   `ITodoItemRepository.cs`

2.  **Use AI Prompts:** Use the following prompts with your AI agent to generate the code for each file. As the AI generates the code, copy and paste it into the appropriate file in your project.

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

## Step 2: Generate In-Memory Repository Implementations

1.  **Create files:** Create the following files in the `Repositories/InMemory` folder:
    *   `InMemoryCommentRepository.cs`
    *   `InMemoryLabelRepository.cs`
    *   `InMemoryProjectRepository.cs`
    *   `InMemorySectionRepository.cs`
    *   `InMemoryTodoItemRepository.cs`

2.  **Use AI Prompts:** Use the following prompts with your AI agent to generate the code for each file:

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

## Step 3: Generate Service Layer Implementations

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

2.  **Use AI Prompts:** Use the following prompts with your AI agent to generate the code for each file:

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

## Step 4: Generate Controller Helper

1.  **Create file:** Create a file named `ControllerHelpers.cs` in the `Helpers` folder.

2.  **Use AI Prompt:** Use the following prompt with your AI agent to generate the code for the file:

    *   **Prompt for `ControllerHelpers.cs`:**

        ```
        Create a static C# class named `ControllerHelpers` in the namespace `Getitdone.Service.Helpers`. It should have a static method named `HandleErrorResponse` that takes an `Exception` as a parameter and returns an `IActionResult`. The method should return a `NotFound` response for `KeyNotFoundException` and an `InternalServerError` response for other exceptions. Use the `Getitdone.Service.Models` namespace for the `ErrorResponse` type.
        ```

## Step 5: Generate Concrete Controllers

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

2.  **Use AI Prompts:** For each of these files, use the following prompt with an AI, replacing `[ControllerName]` with the actual name of the controller (e.g., `CommentOpsController`):

    ```
    Create a C# class named `[ControllerName]` in the namespace `Getitdone.Service.Controllers` that inherits from the corresponding generated controller base class (e.g., `CommentOpsOperationsControllerBase`). Inject the corresponding service interface (e.g., `ICommentOpsOperations`) through the constructor. Implement all the methods of the base class, using the injected service and the `ControllerHelpers.HandleErrorResponse` method for error handling.
    Use the `Getitdone.Service.Models` namespace for the model types, `Getitdone.Service.Helpers` for the `ControllerHelpers` class, and `Getitdone.Service` for the service interface.
    ```

## Step 6: Review and Refine

After generating the code with AI, it's crucial to:

*   **Review the code:** Carefully examine the generated code for correctness, efficiency, and adherence to best practices.
*   **Test thoroughly:** Test the generated code with unit tests and integration tests to ensure it functions as expected.
*   **Refine as needed:** Modify the code to address any issues or to add custom logic.

## Conclusion

By following these steps, you can use an AI model to generate the implementation code for your Todoist clone API. Remember that AI-generated code should always be reviewed and tested thoroughly. This approach can significantly speed up development, but it's important to maintain control over the quality and correctness of your codebase.

