using Inlämningsuppgift_1.Api.DTOs.Requests;
using Inlämningsuppgift_1.Core.Commands.Products;
using Inlämningsuppgift_1.Core.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inlämningsuppgift_1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;


        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var result = await _mediator.Send(query);

            return Ok(result.Value);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var query = new GetProductByIdQuery { Id = productId };
            var result = await _mediator.Send(query);

            return Ok(result.Value);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchQuery, [FromQuery] decimal? maxPrice)
        {
            var query = new SearchForProductQuery { Query = searchQuery, MaxPrice = maxPrice };
            var result = await _mediator.Send(query);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest req)
        {
            var command = new CreateProductCommand
            {
                Name = req.Name,
                Price = req.Price,
                Stock = req.Stock
            };
            var result = await _mediator.Send(command);

            return Ok(result.Value);
        }

        [HttpPost("{productId}/stock/increase")]
        public async Task<IActionResult> ChangeStock(int productId, [FromQuery] int amountToIncrease)
        {
            var command = new ChangeStockOfProductCommand
            {
                Id = productId,
                Amount = amountToIncrease
            };

            var result = await _mediator.Send(command);

            return Ok(result.Value);
        }
    }
}
