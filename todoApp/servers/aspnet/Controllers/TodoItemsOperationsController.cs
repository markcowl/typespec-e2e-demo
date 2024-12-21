using Todo.Service.Common;
using Todo.Service.Impl;
using Todo.Service.Models;

namespace Todo.Service.Controllers
{
    public class TodoItemsOperationsController : TodoItemsOperationsControllerBase
    {
        public TodoItemsOperationsController(IResourceStore<long, TodoItem> todos, IResourceStore<long, List<TodoAttachment>> attachments)
        {
            TodoItemsOperationsImpl = new TodoOperations(todos, attachments);
        }
        internal override ITodoItemsOperations TodoItemsOperationsImpl { get; }
    }
}
