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

	public AnimalController(IAnimalRepository animalRepository)
	{
		_animalRepository = animalRepository;
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
				Main_Image_Url = ""
			};
			return CreatedAtAction(nameof(GetAnimalById), new { id = newAnimalDto.Id }, newAnimalDto);
		}
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
	}

	[HttpPost("image/{id}")]
    public async Task<ActionResult<Animal>> AddAnimalImage(IFormFile file, int id)
	{
		//research how to upload a file in blazor
		//research how to persist a IFormFile in azure blob storage

		//upload to blob storage
		var uploadUrl = "";
		var animal = await _animalRepository.GetAnimalById(id);
		var animalDto = new AnimalDto();
		// animal.Main_Image_Url = uploadUrl;

		await _animalRepository.UpdateAnimal(animalDto);

		return Ok(animalDto);
    }


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
		await _animalRepository.DeleteAnimal(id);
		return NoContent();
	}
}
