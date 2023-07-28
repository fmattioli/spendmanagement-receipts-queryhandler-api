using Application.Queries.Category.GetCategories;
using Application.Queries.Category.GetCategory;
using Application.Queries.Receipt.GetReceipt;
using Application.Queries.Receipt.GetReceipts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator) => _mediator = mediator;

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

        /// <summary>
        /// GET category by Id
        /// Required an Id as parameter.
        /// </summary>
        /// <returns>Return a category based on their Id.</returns>
        [HttpGet]
        [Route("getCategory/{Id:guid}", Name = nameof(GetCategory))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategory([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            var category = await _mediator.Send(new GetCategoryQuery(Id), cancellationToken);
            return Ok(category);
        }
    }
}
