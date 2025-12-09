using Inlämningsuppgift_1.Api.Attributes;
using Inlämningsuppgift_1.Api.DTOs.Requests;
using Inlämningsuppgift_1.Api.Extensions;
using Inlämningsuppgift_1.Core.Commands.Carts;
using Inlämningsuppgift_1.Core.Queries.Carts;
using Inlämningsuppgift_1.Data.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inlämningsuppgift_1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        [TokenAuth]
        public async Task<IActionResult> AddItem([FromHeader(Name = TokenSettings.TokenName)] string token, [FromBody] AddItemRequest req)
        {
            var user = HttpContext.GetAuthUser();

            var command = new AddToCartCommand
            {
                UserId = user.Id,
                ProductId = req.ProductId,
                Quantity = req.Quantity
            };
            var result = await _mediator.Send(command);
            return Ok(result.Value);
        }

        [HttpGet("me")]
        [TokenAuth]
        public async Task<IActionResult> GetCart([FromHeader(Name = TokenSettings.TokenName)] string token)
        {
            var user = HttpContext.GetAuthUser();

            var detailedCartQuery = new GetDetailedCartFromUserQuery
            {
                UserId = user.Id
            };
            var result = await _mediator.Send(detailedCartQuery);
            return Ok(result.Value);

        }

        [HttpDelete("remove")]
        [TokenAuth]
        public async Task<IActionResult> RemoveItem([FromHeader(Name = TokenSettings.TokenName)] string token, [FromQuery] int productId)
        {
            var user = HttpContext.GetAuthUser();

            var command = new RemoveFromCartCommand
            {
                UserId = user.Id,
                ProductId = productId
            };
            var result = await _mediator.Send(command);
            return Ok(result.Value);

        }

        [HttpDelete("clear")]
        [TokenAuth]
        public async Task<IActionResult> ClearCart([FromHeader(Name = TokenSettings.TokenName)] string token)
        {
            var user = HttpContext.GetAuthUser();

            var command = new ClearCartCommand
            {
                UserId = user.Id
            };
            var result = await _mediator.Send(command);
            return Ok(result.Value);
        }
    }
}
