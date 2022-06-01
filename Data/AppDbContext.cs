
using Microsoft.EntityFrameworkCore;

namespace Chatty.Data;

using Models;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){}

    public AppDbContext(){}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseSqlite("Data Source=app.db");

    }
    public DbSet<User> Users => Set<User>();

}
