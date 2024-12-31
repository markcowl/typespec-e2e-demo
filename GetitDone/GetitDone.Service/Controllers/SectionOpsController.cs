using Getitdone.Service.Controllers;
using Getitdone.Service.Helpers;
using Getitdone.Service.Models;
using Getitdone.Service;
using Microsoft.AspNetCore.Mvc;

namespace Getitdone.Service.Controllers
{
    [Route("sections")]
    [ApiController]
    public partial class SectionOpsController : SectionOpsOperationsControllerBase
    {
        internal override ISectionOpsOperations SectionOpsOperationsImpl { get; }

        public SectionOpsController(ISectionOpsOperations sectionOpsOperations)
        {
            SectionOpsOperationsImpl = sectionOpsOperations;
        }

        public override async Task<IActionResult> GetSection(string sectionId)
        {
            try
            {
                var result = await SectionOpsOperationsImpl.GetSectionAsync(sectionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> UpdateSection(string sectionId, UpdateSectionRequest body)
        {
            try
            {
                var result = await SectionOpsOperationsImpl.UpdateSectionAsync(sectionId, body);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> DeleteSection(string sectionId)
        {
            try
            {
                await SectionOpsOperationsImpl.DeleteSectionAsync(sectionId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }
    }
}