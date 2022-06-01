
using Chatty.Data;
using Chatty.Models;
using Microsoft.EntityFrameworkCore;

namespace Chatty.Services;

public class UserService
{
    private readonly AppDbContext db;

    public UserService()
    {
        db = new();
    }    

    public async Task<List<User>> GetUsersAsync()
    {
        return await db.Users.ToListAsync();        
    }

    public async Task<User> GetUserAsync(string id)
    {
        var user = await db.Users.SingleOrDefaultAsync(x => x.Id == id);

        if(user is not null) return user;

        return new();
    }
    
}
