// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Todo.Service.Common;
using Todo.Service.Models;

namespace Todo.Service.Impl
{
    public class UsersOperations : IUsersOperations
    {
        public UsersOperations(IResourceStore<long, User> userStore)
        {
            _userStore = userStore;
        }
        IResourceStore<long, User> _userStore;
        public async Task<UserCreatedResponse> CreateAsync(User user)
        {
            await _userStore.CreateAsync(user.Id, user);
            return new UserCreatedResponse
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                Token = Guid.NewGuid().ToString(),
                Username = user.Username,
                Validated = user.Validated
            };
        }
    }
}
