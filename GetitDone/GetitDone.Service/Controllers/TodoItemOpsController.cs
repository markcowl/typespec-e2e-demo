using Getitdone.Service.Controllers;
using Getitdone.Service.Helpers;
using Getitdone.Service.Models;
using Getitdone.Service;
using Microsoft.AspNetCore.Mvc;

namespace Getitdone.Service.Controllers
{
    [Route("todoitems")]
    [ApiController]
    public partial class TodoItemOpsController : TodoItemOpsOperationsControllerBase
    {
        internal override ITodoItemOpsOperations TodoItemOpsOperationsImpl { get; }

        public TodoItemOpsController(ITodoItemOpsOperations todoItemOpsOperations)
        {
            TodoItemOpsOperationsImpl = todoItemOpsOperations;
        }

        public override async Task<IActionResult> GetTodoItem(string todoitemId)
        {
            try
            {
                var result = await TodoItemOpsOperationsImpl.GetTodoItemAsync(todoitemId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> UpdateTodoItem(string todoitemId, UpdateTodoItemRequest body)
        {
            try
            {
                var result = await TodoItemOpsOperationsImpl.UpdateTodoItemAsync(todoitemId, body);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> CloseTodoItem(string todoitemId)
        {
            try
            {
                await TodoItemOpsOperationsImpl.CloseTodoItemAsync(todoitemId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> ReopenTodoItem(string todoitemId)
        {
            try
            {
                await TodoItemOpsOperationsImpl.ReopenTodoItemAsync(todoitemId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> DeleteTodoItem(string todoitemId)
        {
            try
            {
                await TodoItemOpsOperationsImpl.DeleteTodoItemAsync(todoitemId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }
    }
}