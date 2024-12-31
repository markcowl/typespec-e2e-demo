using Getitdone.Service.Controllers;
using Getitdone.Service.Helpers;
using Getitdone.Service.Models;
using Getitdone.Service;
using Microsoft.AspNetCore.Mvc;

namespace Getitdone.Service.Controllers
{
    [Route("labels")]
    [ApiController]
    public partial class LabelOpsController : LabelOpsOperationsControllerBase
    {
        internal override ILabelOpsOperations LabelOpsOperationsImpl { get; }

        public LabelOpsController(ILabelOpsOperations labelOpsOperations)
        {
            LabelOpsOperationsImpl = labelOpsOperations;
        }

        public override async Task<IActionResult> GetPersonalLabel(string labelId)
        {
            try
            {
                var result = await LabelOpsOperationsImpl.GetPersonalLabelAsync(labelId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> UpdateLabel(string labelId, UpdateLabelRequest body)
        {
            try
            {
                var result = await LabelOpsOperationsImpl.UpdateLabelAsync(labelId, body);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> DeleteLabel(string labelId)
        {
            try
            {
                await LabelOpsOperationsImpl.DeleteLabelAsync(labelId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }
    }
}