using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Repositories;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<Orders> OrderRequests { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ConfirmedOrder> ConfirmedOrders { get; set; }
    public DbSet<RejectedOrder> RejectedOrders { get; set; }
}