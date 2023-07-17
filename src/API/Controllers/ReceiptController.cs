﻿using Application.GetReceipt;
using Application.GetReceipts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Web.Contracts.UseCases.GetReceipts;

namespace API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceiptController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// GET receipts based on previously informeted filters.
        /// Required at least a filter as parameters.
        /// </summary>
        /// <returns>Return a list of receipts based on pre determined filters.</returns>
        [HttpGet]
        [Route("getReceipts", Name = nameof(ReceiptController.GetReceipts))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReceipts([FromRoute] GetReceiptsRequest getReceiptsRequest, CancellationToken cancellationToken)
        {
            var receipts = await _mediator.Send(new GetReceiptsQuery(getReceiptsRequest), cancellationToken);
            return Ok(receipts);
        }

        /// <summary>
        /// GET receipt by Id
        /// Required an Id as parameter.
        /// </summary>
        /// <returns>Return a receipt based on their Id.</returns>
        [HttpGet]
        [Route("getReceipt/{Id:guid}", Name = nameof(GetReceipt))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReceipt([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            var receipts = await _mediator.Send(new GetReceiptQuery(Id), cancellationToken);
            return Ok(receipts);
        }
    }
}