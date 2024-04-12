using Contracts.Web.Category.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Receipts.ReadModel.Application.Queries.Category.GetCategories;

namespace Receipts.ReadModel.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CategoryController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// GET categories based on previously informeted filters.
        /// Required at least a filter as parameters.
        /// </summary>
        /// <returns>Return a list of categories based on pre determined filters.</returns>
        [HttpGet]
        [Route("getCategories", Name = nameof(GetCategories))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategories([FromRoute] GetCategoriesRequest getCategoriesRequest, CancellationToken cancellationToken)
        {
            var categories = await _mediator.Send(new GetCategoriesQuery(getCategoriesRequest), cancellationToken);
            return Ok(categories);
        }
    }
}
