using Common.Models;

namespace Common.Repositories;

public class DataSeeder
{
    public static void SeedGpuData(OrderDbContext context)
    {
        if (context.Products.Any()) return;

        context.Products.AddRange(
            new Product { Name = "NVIDIA GeForce RTX 4090", Amount = 100 },
            new Product { Name = "AMD Radeon RX 7900 XTX", Amount = 150 },
            new Product { Name = "NVIDIA GeForce RTX 3080", Amount = 200 },
            new Product { Name = "AMD Radeon RX 6800 XT", Amount = 120 },
            new Product { Name = "NVIDIA GeForce GTX 1660 Ti", Amount = 180 },
            new Product { Name = "NVIDIA Titan V", Amount = 50 },
            new Product { Name = "NVIDIA GeForce RTX 3070", Amount = 220 },
            new Product { Name = "AMD Radeon RX 5700 XT", Amount = 140 },
            new Product { Name = "NVIDIA Quadro RTX 8000", Amount = 30 },
            new Product { Name = "AMD Radeon RX 5600 XT", Amount = 160 },
            new Product { Name = "NVIDIA GeForce GTX 1080 Ti", Amount = 90 },
            new Product { Name = "NVIDIA RTX A5000", Amount = 80 },
            new Product { Name = "NVIDIA GeForce GTX 1070", Amount = 250 },
            new Product { Name = "AMD Radeon RX 480", Amount = 110 },
            new Product { Name = "NVIDIA GeForce RTX 3060 Ti", Amount = 170 },
            new Product { Name = "AMD Radeon RX 6700 XT", Amount = 130 },
            new Product { Name = "NVIDIA Titan Xp", Amount = 60 },
            new Product { Name = "NVIDIA GeForce GTX 1650 Super", Amount = 190 },
            new Product { Name = "AMD Radeon Vega 64", Amount = 140 }
        );

        context.SaveChanges();
    }
}