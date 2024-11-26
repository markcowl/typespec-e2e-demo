// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// <auto-generated />

using System;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Todo.Service.Models;
using Todo.Service;

namespace Todo.Service.Controllers
{
    [ApiController]
    public abstract partial class TodoItemsOperationsControllerBase : ControllerBase
    {

        internal abstract ITodoItemsOperations TodoItemsOperationsImpl { get; }


        [HttpGet]
        [Route("/items")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TodoPage))]
        public virtual async Task<IActionResult> List([FromQuery(Name = "limit")] int limit = 50, [FromQuery(Name = "offset")] int offset = 0)
        {
            var result = await TodoItemsOperationsImpl.ListAsync(limit, offset);
            return Ok(result);
        }


        [HttpPost]
        [Route("/items")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TodoItem))]
        public virtual async Task<IActionResult> Create([FromHeader(Name = "Content-Type")] string contentType = "application/json", Model1 body)
        {
            var result = await TodoItemsOperationsImpl.CreateAsync(contentType, body);
            return Ok(result);
        }


        [HttpGet]
        [Route("/items/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TodoItem))]
        public virtual async Task<IActionResult> Get(long id)
        {
            var result = await TodoItemsOperationsImpl.GetAsync(id);
            return Ok(result);
        }


        [HttpPatch]
        [Route("/items/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TodoItem))]
        public virtual async Task<IActionResult> Update([FromHeader(Name = "Content-Type")] string contentType = "application/merge-patch+json", long id, TodoItemPatch body)
        {
            var result = await TodoItemsOperationsImpl.UpdateAsync(contentType, id, body);
            return Ok(result);
        }


        [HttpDelete]
        [Route("/items/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent, Type = typeof(void))]
        public virtual async Task<IActionResult> Delete(long id)
        {
            await TodoItemsOperationsImpl.DeleteAsync(id);
            return Ok();
        }

    }
}
