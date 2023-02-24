using PawsomeProject.Server.Models;

namespace PawsomeProject.Server.Data;

public static class ProductSeedData
{
    public static void Initialize(PawsomeDBContext pawsomeDbContext)
    {
        if (!pawsomeDbContext.Products.Any())
        {
            var product01 = new Product
            {
                Name = "Glossier - Beauty Kit",
                Description = "A kit provided by Glossier, containing skin care, hair care and makeup products",
                ImageURL = "/Images/Beauty/Beauty1.png",
                Price = 100,
                Qty = 100,
                CategoryId = 1
            };

            var product02 = new Product
            {
                Name = "Curology - Skin Care Kit",
                Description = "A kit provided by Curology, containing skin care products",
                ImageURL = "/Images/Beauty/Beauty2.png",
                Price = 50,
                Qty = 45,
                CategoryId = 1
            };

            var product03 = new Product
            {
                Name = "Cocooil - Organic Coconut Oil",
                Description = "A kit provided by Curology, containing skin care products",
                ImageURL = "/Images/Beauty/Beauty3.png",
                Price = 20,
                Qty = 30,
                CategoryId = 1
            };

            var product04 = new Product
            {
                Name = "Schwarzkopf - Hair Care and Skin Care Kit",
                Description = "A kit provided by Schwarzkopf, containing skin care and hair care products",
                ImageURL = "/Images/Beauty/Beauty4.png",
                Price = 50,
                Qty = 60,
                CategoryId = 1
            };

            var product05 = new Product
            {
                Name = "Skin Care Kit",
                Description = "Skin Care Kit, containing skin care and hair care products",
                ImageURL = "/Images/Beauty/Beauty5.png",
                Price = 30,
                Qty = 85,
                CategoryId = 1
            };

            var product06 = new Product
            {
                Name = "Air Pods",
                Description = "Air Pods - in-ear wireless headphones",
                ImageURL = "/Images/Electronic/Electronics1.png",
                Price = 100,
                Qty = 120,
                CategoryId = 3
            };

            var product07 = new Product
            {
                Name = "On-ear Golden Headphones",
                Description = "On-ear Golden Headphones - these headphones are not wireless",
                ImageURL = "/Images/Electronic/Electronics2.png",
                Price = 40,
                Qty = 200,
                CategoryId = 3
            };

            var product08 = new Product
            {
                Name = "On-ear Black Headphones",
                Description = "On-ear Black Headphones - these headphones are not wireless",
                ImageURL = "/Images/Electronic/Electronics3.png",
                Price = 40,
                Qty = 300,
                CategoryId = 3
            };

            var product09 = new Product
            {
                Name = "Sennheiser Digital Camera with Tripod",
                Description =
                    "Sennheiser Digital Camera - High quality digital camera provided by Sennheiser - includes tripod",
                ImageURL = "/Images/Electronic/Electronic4.png",
                Price = 600,
                Qty = 20,
                CategoryId = 3
            };

            var product10 = new Product
            {
                Name = "Canon Digital Camera",
                Description = "Canon Digital Camera - High quality digital camera provided by Canon",
                ImageURL = "/Images/Electronic/Electronic5.png",
                Price = 500,
                Qty = 15,
                CategoryId = 3
            };

            var product11 = new Product
            {
                Name = "Nintendo Gameboy",
                Description = "Gameboy - Provided by Nintendo",
                ImageURL = "/Images/Electronic/technology6.png",
                Price = 100,
                Qty = 60,
                CategoryId = 3
            };

            var product12 = new Product
            {
                Name = "Black Leather Office Chair",
                Description = "Very comfortable black leather office chair",
                ImageURL = "/Images/Furniture/Furniture1.png",
                Price = 50,
                Qty = 212,
                CategoryId = 2
            };
            var product13 = new Product
            {
                Name = "Pink Leather Office Chair",
                Description = "Very comfortable pink leather office chair",
                ImageURL = "/Images/Furniture/Furniture2.png",
                Price = 50,
                Qty = 112,
                CategoryId = 2
            };

            var product14 = new Product
            {
                Name = "Lounge Chair",
                Description = "Very comfortable lounge chair",
                ImageURL = "/Images/Furniture/Furniture3.png",
                Price = 70,
                Qty = 90,
                CategoryId = 2
            };
            var product15 = new Product
            {
                Name = "Silver Lounge Chair",
                Description = "Very comfortable Silver lounge chair",
                ImageURL = "/Images/Furniture/Furniture4.png",
                Price = 120,
                Qty = 95,
                CategoryId = 2
            };

            var product16 = new Product
            {
                Name = "Porcelain Table Lamp",
                Description = "White and blue Porcelain Table Lamp",
                ImageURL = "/Images/Furniture/Furniture6.png",
                Price = 15,
                Qty = 100,
                CategoryId = 2
            };

            var product17 = new Product
            {
                Name = "Office Table Lamp",
                Description = "Office Table Lamp",
                ImageURL = "/Images/Furniture/Furniture7.png",
                Price = 20,
                Qty = 73,
                CategoryId = 2
            };

            var product18 = new Product
            {
                Name = "Puma Sneakers",
                Description = "Comfortable Puma Sneakers in most sizes",
                ImageURL = "/Images/Shoes/Shoes1.png",
                Price = 100,
                Qty = 50,
                CategoryId = 3
            };
            
            var products = new Product[]
            {
                product01,
                product02,
                product03,
                product04,
                product05,
                product06,
                product07,
                product08,
                product09,
                product10,
                product11,
                product12,
                product13,
                product14,
                product15,
                product16,
                product17,
                product18,
                
            };

            pawsomeDbContext.AddRange(products);
            pawsomeDbContext.SaveChanges();
        }
    }
}