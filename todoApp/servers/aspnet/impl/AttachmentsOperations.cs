using Microsoft.AspNetCore.WebUtilities;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Todo.Service.Common;
using Todo.Service.Models;

namespace Todo.Service.Impl
{
    public class AttachmentsOperations : IAttachmentsOperations
    {
        public AttachmentsOperations(IResourceStore<long, List<TodoAttachment>> store)
        {
            _store = store;
        }
        private readonly IResourceStore<long, List<TodoAttachment>> _store;
        public async Task CreateFileAttachmentAsync(long itemId, MultipartReader reader)
        {
            List<TodoAttachment> attachmentList = new();
            do
            {
                var section = await reader.ReadNextSectionAsync();
                if (section == null)
                {
                    break;
                }
                var fileSection = section.AsFileSection();
                if ( fileSection == null || fileSection.FileStream == null)
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
            await CreateOrAddAttachments(itemId, attachmentList.ToArray());
        }

        public async Task CreateJsonAttachmentAsync(long itemId, TodoAttachment contents)
        {
            await CreateOrAddAttachments(itemId, new[] { contents });
        }

        internal async Task CreateOrAddAttachments(long itemId, TodoAttachment[] attachments)
        {
            List<TodoAttachment> attachmentList = new();
            var existing = _store.RetrieveAsync(itemId).Result;
            if (existing != null)
            {
                attachmentList.AddRange(existing);
            }
            attachmentList.AddRange(attachments);
            await _store.CreateAsync(itemId, attachmentList);
        }

        public async Task<PageTodoAttachment> ListAsync(long itemId)
        {
            var result = new PageTodoAttachment();
            var attachments = await _store.RetrieveAsync(itemId);
            if (attachments != null)
            {
                result.Items = attachments.ToArray();
            }
            else result.Items = Array.Empty<TodoAttachment>();
            return result;
        }
        
       
    }
}
