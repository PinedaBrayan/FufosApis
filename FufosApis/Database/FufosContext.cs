using Appointment.SDK.Backend.Database;
using FufosEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace FufosApis.Database;

public class FufosContext(DbContextOptions options) : StoreContext(options)
{
    public DbSet<User> Users {get; set;}
    public DbSet<Category> Categories {get; set;}
    public DbSet<Product> Products {get; set;}
}