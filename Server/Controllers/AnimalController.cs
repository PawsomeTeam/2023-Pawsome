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
    public async Task<ActionResult<AnimalDto>> GetAnimalById(int id)
    {
        var animal = await _animalRepository.GetAnimalById(id);
        if (animal == null)
        {
            return NotFound();
        }

        var images = await _animalRepository.GetImages(id);
        var animalDto = new AnimalDto
        {
            Id = animal.Id,
            Name = animal.Name,
            Age = animal.Age,
            Price = animal.Price,
            Type = animal.Type,
            Description = animal.Description,
            Main_Image_Url = animal.Main_Image_Url,
            Images = new List<ImageDto>(),
            Reservation_Date = animal.Reservation_Date,
            Date_adopted = animal.Date_adopted
        };

        if (animal.Images != null)
        {
            foreach (var image in animal.Images)
            {
                var newImage = new ImageDto
                {
                    Id = image.Id,
                    URL = image.URL,
                    Type = image.Type
                };
                animalDto.Images.Add(newImage);
            }
        }

        return Ok(animalDto);
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
                Type = newAnimal.Type,
                Age = newAnimal.Age,
                Price = newAnimal.Price,
                Main_Image_Url = newAnimal.Main_Image_Url,
                Images = new List<ImageDto>(),
                Reservation_Date = newAnimal.Reservation_Date,
                Date_adopted = newAnimal.Date_adopted
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


    [HttpPatch("{id:int}")]
    public async Task<ActionResult<AnimalDto>> UpdateAnimal(int id, AnimalDto animalDto)
    {
        try
        {
            var animalItem = await _animalRepository.UpdateAnimal(id, animalDto);
            if (animalItem == null)
            {
                return NotFound();
            }

            var newAnimalDto = new AnimalDto
            {
                Id = animalItem.Id,
                Name = animalItem.Name,
                Description = animalItem.Description,
                Type = animalItem.Type,
                Age = animalItem.Age,
                Price = animalItem.Price,
                Main_Image_Url = animalItem.Main_Image_Url,
                Images = new List<ImageDto>(),
                Reservation_Date = animalItem.Reservation_Date,
                Date_adopted = animalItem.Date_adopted
            };

            foreach (var image in animalItem.Images)
            {
                var newImageDto = new ImageDto
                {
                    Id = image.Id,
                    URL = image.URL,
                    Type = image.Type
                };
                newAnimalDto.Images.Add(newImageDto);
            }

            return Ok(newAnimalDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
    
    [Route("[action]/{id}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEImage(int id)
    {
        await _animalRepository.DeleteImage(id);
        return Ok();
    }
}