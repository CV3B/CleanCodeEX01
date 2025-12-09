using Inlämningsuppgift_1.Api.Attributes;
using Inlämningsuppgift_1.Api.DTOs.Responses;
using Inlämningsuppgift_1.Api.Extensions;
using Inlämningsuppgift_1.Core.Commands.Orders;
using Inlämningsuppgift_1.Core.Queries.Orders;
using Inlämningsuppgift_1.Data.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inlämningsuppgift_1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        [TokenAuth]
        public async Task<IActionResult> CreateOrder([FromHeader(Name = TokenSettings.TokenName)] string token)
        {
            var user = HttpContext.GetAuthUser();

            var command = new CreateOrderFromCartCommand { UserId = user.Id };
            var result = await _mediator.Send(command);

            if (!result.IsSuccess || result.Value == null)
                return BadRequest(result.Error);

            var order = result.Value;
            return Ok(new OrderResponse
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                Total = order.Total,
                Items = order.OrderItems.ConvertAll(item => new OrderItemResponse
                {
                    Id = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price
                })

            });

        }

        [HttpGet("{orderId}")]
        [TokenAuth]
        public async Task<IActionResult> GetOrder(int orderId, [FromHeader(Name = TokenSettings.TokenName)] string token)
        {
            var user = HttpContext.GetAuthUser();

            var getOrderQuery = new GetOrderForUserQuery { OrderId = orderId, UserId = user.Id };
            var result = await _mediator.Send(getOrderQuery);

            if (!result.IsSuccess || result.Value == null)
                return BadRequest(result.Error);

            var order = result.Value;
            return Ok(new OrderResponse
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                Total = order.Total,
                Items = order.OrderItems.ConvertAll(item => new OrderItemResponse
                {
                    Id = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price
                })

            });
        }
    }
}
