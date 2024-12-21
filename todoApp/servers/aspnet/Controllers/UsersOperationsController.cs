// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Todo.Service.Common;
using Todo.Service.Impl;
using Todo.Service.Models;

namespace Todo.Service.Controllers
{
    public class UsersOperationsController : UsersOperationsControllerBase
    {
        public UsersOperationsController(IResourceStore<long, User> userStore)
        {
            UsersOperationsImpl = new UsersOperations(userStore);
        }
        internal override IUsersOperations UsersOperationsImpl { get; }
    }

}
