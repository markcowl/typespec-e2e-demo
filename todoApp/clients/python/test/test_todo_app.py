import pytest
from corehttp.credentials import ServiceKeyCredential
from todo import TodoClient
from todo.models import (
    User,
    TodoItem,
    TodoItemPatch,
    TodoAttachment,
    ToDoItemMultipartRequest,
)
from corehttp.exceptions import ResourceNotFoundError
import os


@pytest.fixture
def client():
    return TodoClient("http://localhost:5244", ServiceKeyCredential("dummy"))


def test_users_create(client):
    user = client.users.create(
        User(username="John Doe", email="test@example.com", password="p@ssw0rd")
    )
    assert user.username == "John Doe"
    assert user.email == "test@example.com"


def test_todo_items_create_json(client):
    todo_item = client.todo_items.create_json(
        item=TodoItem(
            title="Buy milk",
            status="InProgress",
            assigned_to=10,
            description="Need to buy milk",
        ),
        attachments=[
            TodoAttachment(
                filename="test.jpg", media_type="image/jpeg", contents=b"test"
            )
        ],
    )
    assert todo_item.title == "Buy milk"
    assert todo_item.status == "InProgress"
    assert todo_item.assigned_to == 10
    assert todo_item.description == "Need to buy milk"


@pytest.mark.skip
def test_todo_items_create_form(client):
    current_file_path = os.path.dirname(__file__)
    image_path = os.path.join(current_file_path, "image.jpg")
    todo_item = client.todo_items.create_form(
        ToDoItemMultipartRequest(
            item=TodoItem(
                title="Feed pet",
                status="InProgress",
                assigned_to=10,
                description="Need to feed pet",
            ),
            attachments=[open(image_path, "rb")],
        )
    )
    print(f"Todo item {todo_item.id} created")
    assert todo_item.title == "Buy milk"
    assert todo_item.status == "InProgress"
    assert todo_item.assigned_to == 10
    assert todo_item.description == "Need to buy milk"


def test_todo_item_get(client):
    todo_item = client.todo_items.get(0)
    assert todo_item.title == "Buy milk"
    assert todo_item.status == "InProgress"
    assert todo_item.assigned_to == 10
    assert todo_item.description == "Need to buy milk"


def test_todo_item_get_invalid(client):
    with pytest.raises(ResourceNotFoundError) as exc_info:
        client.todo_items.get(1)
    error = exc_info.value.response.json()
    assert error["code"] == "not-found"


def test_todo_item_update(client):
    updated_item = client.todo_items.update(
        0, TodoItemPatch(title="Buy milk and bread", status="Completed", assigned_to=20)
    )
    assert updated_item.title == "Buy milk and bread"
    assert updated_item.status == "Completed"
    assert updated_item.assigned_to == 20


def test_todo_item_list(client):
    todo_items = client.todo_items.list()
    for item in todo_items:
        assert item.title == "Buy milk and bread"
        assert item.status == "Completed"


def test_todo_item_delete(client):
    client.todo_items.delete(0)
