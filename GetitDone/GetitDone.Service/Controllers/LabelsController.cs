using Getitdone.Service.Controllers;
using Getitdone.Service.Helpers;
using Getitdone.Service.Models;
using Getitdone.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Getitdone.Service.Controllers
{
    [ApiController]
    public partial class LabelsController : LabelsOperationsControllerBase
    {
        internal override ILabelsOperations LabelsOperationsImpl { get; }

        public LabelsController(ILabelsOperations labelsOperations)
        {
            LabelsOperationsImpl = labelsOperations;
        }

        public override async Task<IActionResult> GetPersonalLabels()
        {
            try
            {
                var result = await LabelsOperationsImpl.GetPersonalLabelsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }

        public override async Task<IActionResult> CreateLabel(CreateLabelRequest body)
        {
            try
            {
                var result = await LabelsOperationsImpl.CreateLabelAsync(body);
                return CreatedAtAction(nameof(GetPersonalLabels), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return ControllerHelpers.HandleErrorResponse(ex);
            }
        }
    }
}