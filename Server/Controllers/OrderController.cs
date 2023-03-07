using Microsoft.AspNetCore.Mvc;
using PawsomeProject.Server.Data;
using PawsomeProject.Server.Models;
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
    public async Task<ActionResult<OrderDto>> PostItem([FromBody] OrderDto orderDto)
    {
        Console.WriteLine("Post Item.");

        try
        {
            var newOrderItem = await this.orderRepository.AddItem(orderDto);
            Console.WriteLine("New Order Item. " + newOrderItem.Id);
            if (newOrderItem == null)
            {
                return NoContent();
            }

            var newOrderItemDto = newOrderItem.ConvertToDto();
            return CreatedAtAction(nameof(GetItem), new { id = newOrderItem.Id }, newOrderItem);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
        }
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<OrderDto>> GetItem(string email)
    {
        try
        {
            Console.WriteLine("Controller Id " + email);
            var order = await this.orderRepository.GetItem(email);
            if (order == null)
            {
                return NoContent(); // 204
            }

            Console.WriteLine("Oder Id:" + order.Id);
            var orderItemDto = order.ConvertToDto();
            Console.WriteLine("OderDto Id:" + orderItemDto.Id);
            return Ok(orderItemDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [Route("{email}/GetItems")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetItems(string email)
    {
        try
        {
            var orders = await orderRepository.GetItems(email);
            if (orders == null)
            {
                return NoContent();
            }

            List<OrderDto> orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                orderDtos.Add(order.ConvertToDto());
            }

            return Ok(orderDtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet]
    [Route("AllItems")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> AllItems()
    {
        try
        {
            var orders = await orderRepository.AllItems();
            if (orders == null)
            {
                return NoContent();
            }

            List<OrderDto> orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                orderDtos.Add(order.ConvertToDto());
            }

            return Ok(orderDtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}