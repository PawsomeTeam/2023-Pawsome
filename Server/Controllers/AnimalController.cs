using System.Text.RegularExpressions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using PawsomeProject.Server.Repositories;
using PawsomeProject.Server.Models;
using Microsoft.AspNetCore.Mvc;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalController : ControllerBase
{
    private IAnimalRepository _animalRepository;
    private readonly string blobConnectionString;
    private readonly string animalContainerName;

    public AnimalController(IConfiguration configuration, IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
        blobConnectionString = configuration.GetValue<string>("BlobConnectionString");
        animalContainerName = configuration.GetValue<string>("AnimalContainerName");
    }

    [HttpGet]
    public async Task<ActionResult<List<Animal>>> GetAllAnimals()
    {
        return await _animalRepository.GetAllAnimals();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> GetAnimalById(int id)
    {
        var animal = await _animalRepository.GetAnimalById(id);
        if (animal == null)
        {
            return NotFound();
        }

        return animal;
    }

    [HttpPost]
    public async Task<ActionResult<AnimalDto>> AddAnimal(AnimalDto animalDto)
    {
        try
        {
            var newAnimal = await _animalRepository.AddAnimal(animalDto);
            if (newAnimal == null)
            {
                return NoContent();
            }

            var newAnimalDto = new AnimalDto
            {
                Id = newAnimal.Id,
                Name = newAnimal.Name,
                Description = newAnimal.Description,
                Age = newAnimal.Age,
                Main_Image_Url = newAnimal.Main_Image_Url,
                Images = new List<ImageDto>()
            };

            foreach (var image in newAnimal.Images)
            {
                var newImageDto = new ImageDto
                {
                    Id = image.Id,
                    URL = image.URL,
                    Type = image.Type
                };
                newAnimalDto.Images.Add(newImageDto);
            }

            return CreatedAtAction(nameof(GetAnimalById), new { id = newAnimalDto.Id }, newAnimalDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // [HttpPost("image/{id}")]
    //    public async Task<ActionResult<Animal>> AddAnimalImage(IFormFile file, int id)
    // {
    // 	//research how to upload a file in blazor
    // 	//research how to persist a IFormFile in azure blob storage
    //
    // 	//upload to blob storage
    // 	var uploadUrl = "";
    // 	var animal = await _animalRepository.GetAnimalById(id);
    // 	var animalDto = new AnimalDto();
    // 	// animal.Main_Image_Url = uploadUrl;
    //
    // 	await _animalRepository.UpdateAnimal(animalDto);
    //
    // 	return Ok(animalDto);
    //    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnimal(int id, AnimalDto animal)
    {
        if (id != animal.Id)
        {
            return BadRequest();
        }

        await _animalRepository.UpdateAnimal(animal);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
        try
        {
            var images = await _animalRepository.GetImages(id);
            string filename = "";

            foreach (var image in images)
            {
                Match match = Regex.Match(image.URL, @"([^/]+\.[^/]+)$");
                filename = match.Groups[1].Value;
                await DeleteImage(filename);
                await _animalRepository.DeleteImage(image.Id);
            }

            await _animalRepository.DeleteAnimal(id);

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
                var container = new BlobContainerClient(blobConnectionString, animalContainerName);
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

    [Route("[action]/{file}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteImage(string file)
    {
        try
        {
            var container = new BlobContainerClient(blobConnectionString, animalContainerName);

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
}