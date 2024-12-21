using Todo.Service.Common;
using Todo.Service.Impl;
using Todo.Service.Models;

namespace Todo.Service.Controllers
{
    public class AttachmentsOperationsController : AttachmentsOperationsControllerBase
    {
        public AttachmentsOperationsController(IResourceStore<long, List<TodoAttachment>> store) {
            AttachmentsOperationsImpl = new AttachmentsOperations(store);
        }
        internal override IAttachmentsOperations AttachmentsOperationsImpl { get;  }
    }
}
