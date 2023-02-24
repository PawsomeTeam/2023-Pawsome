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
	public async Task<ActionResult<Animal>> AddAnimal(Animal animal)
	{
		await _animalRepository.AddAnimal(animal);
		return CreatedAtAction(nameof(GetAnimalById), new { id = animal.Id }, animal);
	}

	[HttpPost("image/{id}")]
    public async Task<ActionResult<Animal>> AddAnimalImage(IFormFile file, int id)
	{
		//research how to upload a file in blazor
		//research how to persist a IFormFile in azure blob storage

		//upload to blob storage
		var uploadUrl = "";
		var animal = await _animalRepository.GetAnimalById(id);
		animal.Main_Image_Url = uploadUrl;

		await _animalRepository.UpdateAnimal(animal);

		return Ok(animal);
    }


    [HttpPut("{id}")]
	public async Task<IActionResult> UpdateAnimal(int id, Animal animal)
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
