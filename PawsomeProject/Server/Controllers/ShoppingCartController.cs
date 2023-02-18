using Microsoft.AspNetCore.Mvc;
using PawsomeProject.Server.Data;
using PawsomeProject.Server.Models;
using PawsomeProject.Server.Repositories;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartRepository shoppingCartRepository;
    private readonly IProductRepository productRepository;

    public ShoppingCartController(IShoppingCartRepository shoppingCartRepository,
        IProductRepository productRepository)
    {
        this.shoppingCartRepository = shoppingCartRepository;
        this.productRepository = productRepository;
    }

    [HttpGet]
    [Route("{userId}/GetItems")]
    public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
    {
        try
        {
            var cartItems = await this.shoppingCartRepository.GetItems(userId);
            if (cartItems == null)
            {
                return NoContent(); // 204
            }

            var products = await this.productRepository.GetItems();

            if (products == null)
            {
                throw new Exception("No products exist in the system");
            }

            var cartItemDto = cartItems.ConvertToDto(products);
            return Ok(cartItemDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CartItemDto>> GetItem(int id)
    {
        try
        {
            var cartItem = await this.shoppingCartRepository.GetItem(id);
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

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemAddToDto cartItemAddToDto)
    {
        try
        {
            var newCartItem = await this.shoppingCartRepository.AddItem(cartItemAddToDto);
            if (newCartItem == null)
            {
                return NoContent();
            }

            var product = await productRepository.GetItem(newCartItem.ProductId);
            if (product == null)
            {
                throw new Exception($"Something went wrong when attempting to retrieve product (productID : {cartItemAddToDto.ProductId}");
            }

            var newCartItemDto = newCartItem.ConvertToDto(product);
            return CreatedAtAction(nameof(GetItem), new { id = newCartItemDto.Id }, newCartItemDto);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
        }
    }
    
}