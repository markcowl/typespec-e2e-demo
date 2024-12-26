# Test for TodoApp client

## How to run the test

First, run the following command to start the server:
```
dotnet run --project <RepoRoot>/todoApp/servers/aspnet/Todo.csproj
```

Create and activate your own Python virtual environment ([reference](https://docs.python.org/3/library/venv.html)), and install the library with:
```
cd <RepoRoot>/todoApp/clients/python
python setup.py install
```
Then, run the following command to install necessary dependencies for test:

```
cd <RepoRoot>/todoApp/clients/python
pip install -r dev_requirements.txt
```

Then, run test:
```
cd <RepoRoot>/todoApp/clients/python/test
pytest test
```