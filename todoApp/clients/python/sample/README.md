# Sample for TodoApp client

## Overview

This sample contains a brief usage of the Python generated client for the TodoApp TypeSpec API.

The operations include:
- create a user
- get a non-exist todo item
- create a todo item
- get the created todo item
- list all available todo items
- delete the todo item

## How to run the sample

First, run the following command to start the server:
```
dotnet run --project <RepoRoot>/todoApp/servers/aspnet/Todo.csproj
```

Create and activate your own Python virtual environment ([reference](https://docs.python.org/3/library/venv.html)), and install the library with:
```
cd <RepoRoot>/todoApp/clients/python
python setup.py install
```

Then, run the following command to start the client in the current directory:
```
python sample.py
```