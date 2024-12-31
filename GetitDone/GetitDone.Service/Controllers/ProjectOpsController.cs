using Getitdone.Service.Controllers;
using Getitdone.Service.Helpers;
using Getitdone.Service.Models;
using Getitdone.Service;
using Microsoft.AspNetCore.Mvc;

namespace Getitdone.Service.Controllers
{
    [Route("projects")]
    [ApiController]
    public partial class ProjectOpsController : ProjectOpsOperationsControllerBase
    {
        internal override IProjectOpsOperations ProjectOpsOperationsImpl { get; }

        public ProjectOpsController(IProjectOpsOperations projectOpsOperations)
        {
            ProjectOpsOperationsImpl = projectOpsOperations;
        }

        public override async Task<IActionResult> GetProject(string projectId)
        {
            try
            {
                var result = await ProjectOpsOperationsImpl.GetProjectAsync(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> UpdateProject(string projectId, UpdateProjectRequest body)
        {
            try
            {
                var result = await ProjectOpsOperationsImpl.UpdateProjectAsync(projectId, body);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> DeleteProject(string projectId)
        {
            try
            {
                await ProjectOpsOperationsImpl.DeleteProjectAsync(projectId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> GetCollaborators(string projectId)
        {
            try
            {
                var result = await ProjectOpsOperationsImpl.GetCollaboratorsAsync(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }
    }
}