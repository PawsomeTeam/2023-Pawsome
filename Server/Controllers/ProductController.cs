using System.Text.RegularExpressions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
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
    private readonly string blobConnectionString;
    private readonly string productContainerName;

    public ProductController(IConfiguration configuration, IProductRepository productRepository)
    {
        this.productRepository = productRepository;
        blobConnectionString = configuration.GetValue<string>("BlobConnectionString");
        productContainerName = configuration.GetValue<string>("ProductContainerName");
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
            var product = await productRepository.GetItem(id);
            if (product == null)
            {
                return BadRequest();
            }

            var productCategory = await productRepository.GetCategory(product.CategoryId);
            var images = await productRepository.GetImages(product.Id);
            product.Images = images;
            var productDto = product.ConvertToDto(productCategory);
            return Ok(productDto);
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
            var newProductItem = await productRepository.CreateItem(productDto);
            if (newProductItem == null)
            {
                return NoContent();
            }

            var newProductCategory = await productRepository.GetCategory(newProductItem.CategoryId);
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
    public async Task<ActionResult> DeleteItem(int id)
    {
        try
        {
            await productRepository.DeleteItem(id);

            var images = await productRepository.GetImages(id);
            string filename = "";

            foreach (var image in images)
            {
                Match match = Regex.Match(image.URL, @"([^/]+\.[^/]+)$");
                filename = match.Groups[1].Value;
                await DeleteImage(filename);
                await productRepository.DeleteImage(image.Id);
            }

            return Ok();
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
            var productItem = await this.productRepository.UpdateItem(id, productDto);
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

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> UploadImage()
    {
        try
        {
            var formCollection = await Request.ReadFormAsync();

            var file = formCollection.Files.First();

            if (file.Length > 0)
            {
                var container = new BlobContainerClient(blobConnectionString, productContainerName);
                var createResponse = await container.CreateIfNotExistsAsync();

                if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                    await container.SetAccessPolicyAsync(PublicAccessType.Blob);


                var invalids = Path.GetInvalidPathChars();
                var newFileName = String
                    .Join("_", file.FileName.Split(invalids, StringSplitOptions.RemoveEmptyEntries))
                    .TrimEnd('.');

                var uploadFileName = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + "_" +
                                     newFileName;
                var blob = container.GetBlobClient(uploadFileName);

                await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                using (var fileStream = file.OpenReadStream())
                {
                    await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
                }

                return Ok(blob.Uri.ToString());
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }

    [HttpDelete("{file}")]
    public async Task<IActionResult> DeleteImage(string file)
    {
        try
        {
            var container = new BlobContainerClient(blobConnectionString, productContainerName);

            if (file != null)
            {
                var blcokBlobClient = container.GetBlockBlobClient(file);
                blcokBlobClient.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
                return Ok("A image deleted");
            }

            return BadRequest("File is empty");
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }

    [Route("[action]/{id}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEImage(int id)
    {
        await productRepository.DeleteImage(id);
        return Ok();
    }

    [Route("[action]")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetCategories()
    {
        try
        {
            var categories = await this.productRepository.GetCategories();
            if (categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
        }
    }
}