using Microsoft.AspNetCore.Mvc;
using PawsomeProject.Server.Data;
using PawsomeProject.Server.Models;
using PawsomeProject.Server.Repositories;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository productRepository;

    public ProductController(IProductRepository productRepository)
    {
        this.productRepository = productRepository; 
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
    {
        try
        {
            var products = await this.productRepository.GetItems();
            var productCategories = await this.productRepository.GetCategories();
            if (products == null || productCategories == null)
            {
                return NotFound();
            }
            else
            {
                var productDtos = products.ConvertToDto(productCategories);
                return Ok(productDtos);
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetItems(int id)
    {
        try
        {
            var product = await this.productRepository.GetItem(id);
            if (product == null)
            {
                return BadRequest();
            }
            else
            {
                var productCategory = await this.productRepository.GetCategory(product.CategoryId);
                var productDto = product.ConvertToDto(productCategory);
                return Ok(productDto);
            } 
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostItem([FromBody] ProductDto productDto)
    {
        try
        {
            var newProductItem = await this.productRepository.CreateItem(productDto);
            if (newProductItem == null)
            {
                return NoContent();
            }

            var newProductCategory = await this.productRepository.GetCategory(newProductItem.CategoryId);
            var newProductItemDto = newProductItem.ConvertToDto(newProductCategory);

            return CreatedAtAction(nameof(GetItems), new { id = newProductItemDto.Id }, newProductItemDto);
       }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
    {
        try
        {
            var productItem = await this.productRepository.DeleteItem(id);
            if (productItem == null)
            {
                return NotFound();
            }

            var productCategory = await this.productRepository.GetCategory(productItem.CategoryId);
            if (productCategory == null)
            {
                return NotFound();
            }

            var cartItemDto = productItem.ConvertToDto(productCategory);
            return Ok(cartItemDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");

        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<CartItemDto>> UpdateProduct(int id, ProductDto productDto)
    {
        try
        {
            var productItem = await this.productRepository.UpdateItem(id,productDto);
            if (productItem == null)
            {
                return NotFound();
            }
            
            var productCategory = await this.productRepository.GetCategory(productItem.CategoryId);
            if (productCategory == null)
            {
                return NotFound();
            }

            var productItemDto = productItem.ConvertToDto(productCategory);
            return Ok(productItemDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
}       