using System.Text.Json;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class Seed
{
    public static async Task SeedUserAsync(AppDbContext appDbContext)
    {
        if (await appDbContext.UserEntities.AnyAsync()) return;
        
        var userData = await File.ReadAllTextAsync("D:/Code_Tests/projects/Integration_Tests/Integration_Tests/Infrastructure/Data/UserSeed.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        var users = JsonSerializer.Deserialize<List<UserEntity>>(userData, options);

        if (users is null) return;
        
        await appDbContext.UserEntities.AddRangeAsync(users);
        
        await appDbContext.SaveChangesAsync();
    }

    public static async Task SeedUsers(AppDbContext appDbContext)
    {
        if  (appDbContext.UserEntities.Any()) return;
        
        var userData = await File.ReadAllTextAsync("D:/Code_Tests/projects/Integration_Tests/Integration_Tests/Infrastructure/Data/UserSeed.json");
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        var users = JsonSerializer.Deserialize<List<UserEntity>>(userData, options);
        
        if (users is null) return;
        
        appDbContext.UserEntities.AddRange(users);
        
        appDbContext.SaveChanges();
    }
}