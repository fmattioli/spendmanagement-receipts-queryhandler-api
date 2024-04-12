using Contracts.Web.Receipt.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Receipts.ReadModel.Application.Claims;
using Receipts.ReadModel.Application.Queries.Receipt.GetRecurringReceipts;
using Receipts.ReadModel.Application.Queries.Receipt.GetVariableReceipts;
using Receipts.ReadModel.CrossCutting.Filters;

namespace Receipts.ReadModel.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Authorize]
    public class ReceiptController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// GET variable expenses based on previously informed filters.
        /// Required at least one filter as parameters.
        /// </summary>
        /// <returns>Returns a list of expenses receipts based on predetermined filters.</returns>
        [HttpGet]
        [Route("getVariableReceipts", Name = nameof(GetVariableReceipts))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ClaimsAuthorize(ClaimTypes.Receipt, "Read")]
        public async Task<IActionResult> GetVariableReceipts([FromRoute] GetVariableReceiptsRequest getVariableReceiptsRequest, CancellationToken cancellationToken)
        {
            var receipts = await _mediator.Send(new GetVariableReceiptsQuery(getVariableReceiptsRequest), cancellationToken);
            return Ok(receipts);
        }

        /// <summary>
        /// GET recurring expenses based on previously informed filters.
        /// Required at least one filter as parameters.s
        /// </summary>
        /// <returns>Returns a list of expenses receipts based on predetermined filters.</returns>
        [HttpGet]
        [Route("getRecurringReceipts", Name = nameof(GetRecurringReceipts))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ClaimsAuthorize(ClaimTypes.Receipt, "Read")]
        public async Task<IActionResult> GetRecurringReceipts([FromRoute] GetRecurringReceiptsRequest getRecurringReceiptsRequest, CancellationToken cancellationToken)
        {
            var receipts = await _mediator.Send(new GetRecurringReceiptsQuery(getRecurringReceiptsRequest), cancellationToken);
            return Ok(receipts);
        }
    }
}
