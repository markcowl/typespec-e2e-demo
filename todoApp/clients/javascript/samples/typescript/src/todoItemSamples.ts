import { TodoClient, TodoItem } from '@notabrand/todo-non-branded';

// Load the .env file if it exists  
import * as dotenv from "dotenv";
dotenv.config();

const endpoint = process.env["ENDPOINT"] || "<endpoint>";

const client = new TodoClient(endpoint, 
  { key: "dummy" },
  {
  allowInsecureConnection: true
});

async function main() {
  const item: TodoItem = {
    title: "Buy milk",
    createdBy: 1,
    assignedTo: 10,
    description: "Need to buy milk",
    status: "InProgress",
    createdAt: new Date(),
    updatedAt: new Date(),
    id: 1
  };

  // create a user
  const user = await client.users.create({
    username: "Harry Potter",
    email: "harry@email.com",
    password: "password",
    id: 0,
  });
  console.log(user);

  // get a non-exist todo item
  try {
    await client.todoItems.get(0);
  } catch (error) {
    console.log((error as any).code);
  }

  // - create a todo item
  const createdTodoItem = await client.todoItems.createJson(item);
  console.log(createdTodoItem);

  // - get the created todo item
  const getTodoItem = await client.todoItems.get(createdTodoItem.id);
  console.log(getTodoItem);

  // update a todo item
  const updatedTodoItem = await client.todoItems.update(createdTodoItem.id, {
    title: "Buy milk and bread",
    assignedTo: 2,
    description: "Need to buy milk and eggs from the store"
  });
  console.log(updatedTodoItem);

  // - list all available todo items
  const iter = client.todoItems.list({ limit: 1, offset: 0 });
  const result = [];
  for await (const item of iter.byPage()) {
    result.push(...item);
  }
  console.log(result);

  // - delete the todo item
  const deleteTodoItem = await client.todoItems.delete(createdTodoItem.id);
  console.log(deleteTodoItem);

  // delete when it does not exist should get a 404
  try {
    await client.todoItems.delete(0);
  } catch (error) {
    console.log((error as any).code);
  }
}

main().catch(console.error);
