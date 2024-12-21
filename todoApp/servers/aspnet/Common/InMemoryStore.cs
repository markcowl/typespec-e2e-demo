// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Todo.Service.Common
{
    public interface IResourceStore<Key, Model> where Model : class where Key : notnull
    {
        Task<Model> CreateAsync(Key id, Model model);
        Task<Model?> RetrieveAsync(Key id);
        Task<Model?> UpdateAsync(Key id, Model model);
        Task<bool> DeleteAsync(Key id);
        Task<Model[]> ListAsync(int? offset, int? limit);
    }
    public class InMemoryStore<Key, Model> : IResourceStore<Key, Model> where Model :class where Key : notnull 
    {
        private readonly Dictionary<Key, Model> _store = new();

        public Task<Model> CreateAsync(Key id, Model model)
        {
            _store[id] = model;
            return Task.FromResult(model);
        }

        public Task<Model?> RetrieveAsync(Key id)
        {
            if (_store.TryGetValue(id, out Model? model))
            {
                return Task.FromResult<Model?>(model);
            }
            return Task.FromResult<Model?>(null);
        }

        public Task<Model?> UpdateAsync(Key id, Model model)
        {
            if (_store.ContainsKey(id))
            {
                _store[id] = model!;
                return Task.FromResult<Model?>(model);
            }
            return Task.FromResult<Model?>(null);
        }

        public Task<bool> DeleteAsync(Key id)
        {
            return Task.FromResult(_store.Remove(id));
        }

        public Task<Model[]> ListAsync(int? offset, int? limit)
        {
            return Task.FromResult(_store.Values.Skip(offset ?? 0).Take(limit ?? 40).ToArray());
        }
    }
}
