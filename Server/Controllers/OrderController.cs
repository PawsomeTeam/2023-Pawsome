using Microsoft.AspNetCore.Mvc;
using PawsomeProject.Server.Data;
using PawsomeProject.Server.Repositories;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository orderRepository;
    private readonly IProductRepository productRepository;

    public OrderController(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
    }
    
    [HttpPost]
    public async Task<ActionResult<OrderItemDto>> PostItem([FromBody] OrderItemAddToDto orderItemAddToDto)
    {
         try
        {
            var newOrderItem = await this.orderRepository.AddItem(orderItemAddToDto);
            if (newOrderItem == null)
            {
                return NoContent();
            }

            var product = await productRepository.GetItem(orderItemAddToDto.ProductId);
            if (product == null)
            {
                throw new Exception(
                    $"Something went wrong when attempting to retrieve product (productID : {orderItemAddToDto.ProductId}");
            }

            var newOrderItemDto = newOrderItem.ConvertToDto(product);
            return CreatedAtAction(nameof(GetItem), new { id = newOrderItem.OrderItemId }, newOrderItem);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
        }
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderItemDto>> GetItem(int id)
    {
        try
        {
            var cartItem = await this.orderRepository.GetItem(id);
            if (cartItem == null)
            {
                return NoContent(); // 204
            }

            var product = await this.productRepository.GetItem(id);
            if (product == null)
            {
                throw new Exception("No product exist in the system");
            }

            var cartItemDto = cartItem.ConvertToDto(product);
            return Ok(cartItemDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
}