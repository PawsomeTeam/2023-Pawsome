using Microsoft.EntityFrameworkCore;
using PawsomeProject.Server.Data;
using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly PawsomeDBContext _dbContext;

        public AnimalRepository(PawsomeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Animal>> GetAllAnimals()
        {
            return await _dbContext.Animals.ToListAsync();
        }

        public async Task<Animal> GetAnimalById(int id)
        {
            return await _dbContext.Animals.FindAsync(id);
        }

        public async Task<List<Image>> GetImages(int id)
        {
            var images = await _dbContext.Images.Where(o => o.Animal.Id == id).ToListAsync();
            return images;
        }

        public async Task<Animal> AddAnimal(AnimalDto animalDto)
        {
            var newAnimal = new Animal
            {
                Name = animalDto.Name,
                Description = animalDto.Description,
                Type = animalDto.Type,
                Age = animalDto.Age,
                Price = animalDto.Price,
                Main_Image_Url = animalDto.Main_Image_Url,
                Images = new List<Image>(),
                Reservation_Date = animalDto.Reservation_Date,
                Date_adopted = animalDto.Date_adopted
            };
            foreach (var image in animalDto.Images)
            {
                Image newImage = new Image
                {
                    URL = image.URL,
                    Type = image.Type
                };
                newAnimal.Images.Add(newImage);
            }

            if (newAnimal != null)
            {
                var result = await _dbContext.Animals.AddAsync(newAnimal);
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }

        public async Task<Animal> UpdateAnimal(int id, AnimalDto animal)
        {
            var updateAnimal = await _dbContext.Animals.FindAsync(id);
            // var images = new List<Image>();
            var images = await  _dbContext.Images.Where(o => o.Animal.Id == id).ToListAsync();

            foreach (var image in animal.Images)
            {
                if (!images.Exists(o => o.Id == image.Id))
                {
                    images.Add(new Image
                    {
                        URL = image.URL,
                        Type = image.Type
                    });
                }
            }

            if (updateAnimal != null)
            {
                updateAnimal.Name = animal.Name;
                updateAnimal.Description = animal.Description;
                updateAnimal.Price = animal.Price;
                updateAnimal.Main_Image_Url = animal.Main_Image_Url;
                updateAnimal.Age = animal.Age;
                updateAnimal.Images = images;
                updateAnimal.Type = animal.Type;
                updateAnimal.Reservation_Date = animal.Reservation_Date;
                updateAnimal.Date_adopted = animal.Date_adopted;


                await _dbContext.SaveChangesAsync();
                return updateAnimal;
            }

            return null;
        }

        public async Task DeleteAnimal(int id)
        {
            var animal = await _dbContext.Animals.FindAsync(id);
            if (animal != null)
            {
                _dbContext.Animals.Remove(animal);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteImage(int id)
        {
            var item = await _dbContext.Images.FindAsync(id);
            if (item != null)
            {
                _dbContext.Images.Remove(item);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}