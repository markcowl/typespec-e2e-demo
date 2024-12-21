// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.WebUtilities;
using Todo.Service.Common;
using Todo.Service.Models;

namespace Todo.Service.Impl
{
    public class TodoOperations : ITodoItemsOperations
    {
        public TodoOperations( IResourceStore<long, TodoItem> todos, IResourceStore<long, List<TodoAttachment>> attachments)
        {
            _attachmentStore = attachments;
            _todoStore = todos;
        }
        IResourceStore<long, TodoItem> _todoStore;
        IResourceStore<long, List<TodoAttachment>> _attachmentStore;
        public async Task<TodoItem> CreateFormAsync(MultipartReader reader)
        {
            var section = await reader.ReadNextSectionAsync();
            if (section == null || (section.ContentType != null && !section.ContentType.Contains("json")))
            {
                throw new BadHttpRequestException("Invalid multipart request - no json part");
            }
            var item = await section.Body.AsJsonAsync<TodoItem>();
            if (item == null)
            {
                throw new BadHttpRequestException("Invalid json for stream");
            }
            item.CreatedAt = DateTimeOffset.UtcNow;
            item.UpdatedAt = DateTimeOffset.UtcNow;
            await _todoStore.CreateAsync(item.Id, item);
            List<TodoAttachment> attachmentList = new();
            do
            {
                section = await reader.ReadNextSectionAsync();
                if (section == null)
                {
                    break;
                }
                var fileSection = section.AsFileSection();
                if (fileSection == null || fileSection.FileStream == null)
                {
                    throw new BadHttpRequestException("Must contain files");
                }
                attachmentList.Add(new TodoAttachment
                {
                    Filename = fileSection.FileName,
                    MediaType = fileSection.Section.ContentType,
                    Contents = await fileSection.FileStream.ReadAllAsync()
                });
            }
            while (true);
            if (attachmentList.Count > 0)
            {
                await _attachmentStore.CreateAsync(item.Id, attachmentList);
            }

            return item;
        }

        public async Task<TodoItem> CreateJsonAsync(TodoItem item, TodoAttachment[]? attachments)
        {
            item.CreatedAt = DateTimeOffset.UtcNow;
            item.UpdatedAt = DateTimeOffset.UtcNow;
            await _todoStore.CreateAsync(item.Id, item);
            if (attachments != null && attachments.Length > 0)
            {
                await _attachmentStore.CreateAsync(item.Id, attachments.ToList());
            }
            return item;
        }

        public async Task DeleteAsync(long id)
        {
            await _todoStore.DeleteAsync(id);
        }

        public async Task<TodoItem> GetAsync(long id)
        {
            var result = await _todoStore.RetrieveAsync(id);
            if (result == null)
            {
                throw new KeyNotFoundException($"{id} not found");
            }
            return result;
        }

        public async Task<TodoPage> ListAsync(int? limit, int? offset)
        {
           var result = new TodoPage();
            result.Items = await _todoStore.ListAsync(offset, limit);
            return result;
        }

        public async Task<TodoItem> UpdateAsync(long id, TodoItemPatch patch)
        {
            var result = await _todoStore.RetrieveAsync(id);
            if (result == null)
            {
                throw new KeyNotFoundException($"{id} not found");
            }
            result.Title = patch.Title ?? result.Title;
            result.Status = patch.Status ?? result.Status;
            result.AssignedTo = patch.AssignedTo;      
            result.Description = patch.Description ?? result.Description;
            result.UpdatedAt = DateTimeOffset.UtcNow;
            await _todoStore.UpdateAsync(id, result);
            return result;
        }
    }
}
