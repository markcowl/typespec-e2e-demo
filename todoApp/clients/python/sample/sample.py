from todo import TodoClient
from corehttp.credentials import ServiceKeyCredential
from corehttp.exceptions import ResourceNotFoundError
from todo.models import (
    User,
    TodoItem,
    TodoItemPatch,
    TodoAttachment,
    ToDoItemMultipartRequest,
)
import os

todoClient = TodoClient("http://localhost:5244", ServiceKeyCredential("dummy"))

# create a user
user = todoClient.users.create(
    User(username="John Doe", email="test@example.com", password="p@ssw0rd")
)
print(f"created user: {user}")

# try to get a non-existent todo item
try:
    todo_item = todoClient.todo_items.get(0)
except ResourceNotFoundError as exc_info:
    error = exc_info.response.json()
    print(f"Cannot find todo item: {error}")

# create a todo item with json
todo_item = todoClient.todo_items.create_json(
    item=TodoItem(
        title="Buy milk",
        status="InProgress",
        assigned_to=10,
        description="Need to buy milk",
    ),
    attachments=[
        TodoAttachment(filename="test.jpg", media_type="image/jpeg", contents=b"test")
    ],
)
print(f"Todo item {todo_item.id} created")

# # create a todo item with multi part
# current_file_path = os.path.dirname(__file__)
# image_path = os.path.join(current_file_path, "image.jpg")
# todo_item = todoClient.todo_items.create_form(ToDoItemMultipartRequest(item=TodoItem(title="Feed pet", status="InProgress", assigned_to=10, description="Need to feed pet"), attachments=[open(image_path, "rb")]))
# print(f"Todo item {todo_item.id} created")

# get the todo item
retrieved_item = todoClient.todo_items.get(todo_item.id)
print(
    f"Todo item {retrieved_item.id} retrieved, title: {retrieved_item.title}, status: {retrieved_item.status}, assigned_to: {retrieved_item.assigned_to}, description: {retrieved_item.description}"
)

# update the todo item
updated_item = todoClient.todo_items.update(
    todo_item.id,
    TodoItemPatch(title="Buy milk and bread", status="Completed", assigned_to=20),
)
print(
    f"Todo item updated, title: {updated_item.title}, status: {updated_item.status}, assigned_to: {updated_item.assigned_to}, description: {updated_item.description}"
)

# list all available todo items
todo_items = todoClient.todo_items.list()
for item in todo_items:
    print(f"Item title: {item.title}, status: {item.status}")

# delete the todo item
todoClient.todo_items.delete(todo_item.id)
print("Item deleted")
