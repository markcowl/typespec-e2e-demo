using Getitdone.Service.Controllers;
using Getitdone.Service.Helpers;
using Getitdone.Service.Models;
using Getitdone.Service;
using Microsoft.AspNetCore.Mvc;

namespace Getitdone.Service.Controllers
{
    [Route("comments")]
    [ApiController]
    public partial class CommentOpsController : CommentOpsOperationsControllerBase
    {
        internal override ICommentOpsOperations CommentOpsOperationsImpl { get; }

        public CommentOpsController(ICommentOpsOperations commentOpsOperations)
        {
            CommentOpsOperationsImpl = commentOpsOperations;
        }

        public override async Task<IActionResult> GetComment(string commentId)
        {
            try
            {
                var result = await CommentOpsOperationsImpl.GetCommentAsync(commentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> UpdateComment(string commentId, UpdateCommentRequest body)
        {
            try
            {
                var result = await CommentOpsOperationsImpl.UpdateCommentAsync(commentId, body);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> DeleteComment(string commentId)
        {
            try
            {
                await CommentOpsOperationsImpl.DeleteCommentAsync(commentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }
    }
}