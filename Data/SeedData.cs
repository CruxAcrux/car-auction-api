using Microsoft.EntityFrameworkCore;
using CarAuctionApi.Models.Entities;
using System.Security.Cryptography;

namespace CarAuctionApi.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var existingBrands = context.CarBrands.Include(b => b.Models).ToList();

            var brands = new List<CarBrand>
{
    new CarBrand
    {
        Name = "Toyota",
        Models = new List<CarModel>
        {
            new CarModel { Name = "Camry" },
            new CarModel { Name = "RAV4" }
        }
    },
    new CarBrand
    {
        Name = "Honda",
        Models = new List<CarModel>
        {
            new CarModel { Name = "Civic" },
            new CarModel { Name = "CR-V" }
        }
    },
    new CarBrand
    {
        Name = "Ford",
        Models = new List<CarModel>
        {
            new CarModel { Name = "F-150" },
            new CarModel { Name = "Mustang" }
        }
    },
    new CarBrand
    {
        Name = "Chevrolet",
        Models = new List<CarModel>
        {
            new CarModel { Name = "Silverado" },
            new CarModel { Name = "Equinox" }
        }
    },
    new CarBrand
    {
        Name = "Volkswagen",
        Models = new List<CarModel>
        {
            new CarModel { Name = "Golf" },
            new CarModel { Name = "Tiguan" }
        }
    },
    new CarBrand
    {
        Name = "BMW",
        Models = new List<CarModel>
        {
            new CarModel { Name = "3 Series" },
            new CarModel { Name = "X5" }
        }
    },
    new CarBrand
    {
        Name = "Mercedes-Benz",
        Models = new List<CarModel>
        {
            new CarModel { Name = "C-Class" },
            new CarModel { Name = "GLC" }
        }
    },
    new CarBrand
    {
        Name = "Audi",
        Models = new List<CarModel>
        {
            new CarModel { Name = "A4" },
            new CarModel { Name = "Q5" }
        }
    },
    new CarBrand
    {
        Name = "Hyundai",
        Models = new List<CarModel>
        {
            new CarModel { Name = "Tucson" },
            new CarModel { Name = "Elantra" }
        }
    },
    new CarBrand
    {
        Name = "Kia",
        Models = new List<CarModel>
        {
            new CarModel { Name = "Sportage" },
            new CarModel { Name = "Sorento" }
        }
    },
    new CarBrand
    {
        Name = "Nissan",
        Models = new List<CarModel>
        {
            new CarModel { Name = "Rogue" },
            new CarModel { Name = "Altima" }
        }
    },
    new CarBrand
    {
        Name = "Subaru",
        Models = new List<CarModel>
        {
            new CarModel { Name = "Outback" },
            new CarModel { Name = "Forester" }
        }
    },
    new CarBrand
    {
        Name = "Mazda",
        Models = new List<CarModel>
        {
            new CarModel { Name = "CX-5" },
            new CarModel { Name = "3" }
        }
    },
    new CarBrand
    {
        Name = "Volvo",
        Models = new List<CarModel>
        {
            new CarModel { Name = "XC60" },
            new CarModel { Name = "XC90" }
        }
    },
    new CarBrand
    {
        Name = "Tesla",
        Models = new List<CarModel>
        {
            new CarModel { Name = "Model 3" },
            new CarModel { Name = "Model Y" }
        }
    },
    new CarBrand
    {
        Name = "Lexus",
        Models = new List<CarModel>
        {
            new CarModel { Name = "RX" },
            new CarModel { Name = "ES" }
        }
    },
    new CarBrand
    {
        Name = "Jeep",
        Models = new List<CarModel>
        {
            new CarModel { Name = "Grand Cherokee" },
            new CarModel { Name = "Wrangler" }
        }
    },
    new CarBrand
    {
        Name = "Land Rover",
        Models = new List<CarModel>
        {
            new CarModel { Name = "Range Rover" },
            new CarModel { Name = "Discovery" }
        }
    },
    new CarBrand
    {
        Name = "Porsche",
        Models = new List<CarModel>
        {
            new CarModel { Name = "911" },
            new CarModel { Name = "Cayenne" }
        }
    },
    new CarBrand
    {
        Name = "Jaguar",
        Models = new List<CarModel>
        {
            new CarModel { Name = "F-PACE" },
            new CarModel { Name = "XE" }
        }
    }
};

            foreach (var brand in brands)
            {
                var existingBrand = existingBrands.FirstOrDefault(b => b.Name == brand.Name);

                if (existingBrand == null)
                {
                    // Add new brand
                    context.CarBrands.Add(brand);
                }
                else
                {
                    // Update models for existing brand
                    foreach (var model in brand.Models)
                    {
                        if (!existingBrand.Models.Any(m => m.Name == model.Name))
                        {
                            model.CarBrandId = existingBrand.Id;
                            context.CarModels.Add(model);
                        }
                    }
                }
            }

            context.SaveChanges();
        }
    }
}