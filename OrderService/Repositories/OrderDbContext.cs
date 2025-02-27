using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Repositories;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<OrderRequest> OrderRequests { get;}
    public DbSet<Products> Products { get;}
}
