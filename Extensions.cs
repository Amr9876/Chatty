
namespace Chatty;

using System.Text;
using Bogus;
using Chatty.Data;
using Microsoft.EntityFrameworkCore;
using Models;

public static class Extensions
{
    
    public static async Task AddDataDestroyerAsync<TDbContext>(this IServiceCollection services) 
        where TDbContext : AppDbContext, new()
    {

        var db = new TDbContext();

        if(db is not null)
        {

            var users = await db.Users.ToListAsync();

            if(users is not null)
            {

                foreach (var user in users)
                {
                    
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Data Destroyed Successfuly!");
                    Console.ResetColor();

                }

            }

        }

    }

    public static async Task AddDataSeederAsync<TDbContext>(this IServiceCollection services) 
        where TDbContext : AppDbContext, new()
    {

        var userFaker = new Faker<User>();

        userFaker.RuleFor(x => x.Id, Guid.NewGuid().ToString())
                 .RuleFor(x => x.FirstName, x => x.Person.FirstName)
                 .RuleFor(x => x.LastName, x => x.Person.LastName)
                 .RuleFor(x => x.FullName, x => x.Person.FullName)
                 .RuleFor(x => x.EmailAddress, x => x.Person.Email)
                 .RuleFor(x => x.Password, x => "123456");

        var db = new TDbContext();

        for (int i = 0; i < 7; i++)
        {
            
            var user = userFaker.Generate();

            if (db.Users.Count() <= 7)
            {

                if(db.Users.Where(x => x.Id == user.Id).Count() > 0)
                {
                    user.Id = Guid.NewGuid().ToString();
                }

                db.Users.Add(user);
                await db.SaveChangesAsync();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(string.Format("{0} add to db successfuly!", user.FirstName));
                Console.ResetColor();

            }

        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Done, Check your Database!!");
        Console.ResetColor();        

    }

}
