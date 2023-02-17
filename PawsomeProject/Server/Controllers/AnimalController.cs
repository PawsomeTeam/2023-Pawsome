using PawsomeProject.Server.Repositories;
using PawsomeProject.Server.Models;
using Microsoft.AspNetCore.Mvc;

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
