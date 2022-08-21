using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Controllers;

[Route("[controller]")]
public class DataController
{
    private readonly LocalDbContext _dbContext;


    public DataController(LocalDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet("init")]
    public async Task Initialize()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.EnsureCreatedAsync();
        var itemId = 1;
        var faker = new Faker<Item>()
            .StrictMode(true)
            .RuleFor(x => x.Id, f => itemId++)
            .RuleFor(x => x.FirstName, (f, u) => f.Name.FirstName())
            .RuleFor(x => x.Surname, (f, u) => f.Name.LastName())
            .RuleFor(x => x.Birthday, (f, u) => f.Date.Between(new DateTime(1990, 1, 1), new DateTime(2000,12,31)));

        var items = faker.GenerateLazy(100000);
        await _dbContext.Items.AddRangeAsync(items);
        await _dbContext.SaveChangesAsync();
    }

    [HttpGet("all")]
    public IAsyncEnumerable<Item> GetItems()
    {
        return _dbContext.Items.AsAsyncEnumerable();
    }
    
    [HttpGet("strict")]
    public async IAsyncEnumerable<Item> GetStrictItems()
    {
        await foreach (var item in _dbContext.Items
                           .Where(x => x.Surname.ToLower().Contains("ski") && x.FirstName.ToLower().Contains("a") &&
                                       x.Birthday > new DateTime(1995, 1, 1) && x.Birthday < new DateTime(1998, 12, 31))
                           .OrderBy(x => x.FirstName)
                           .AsAsyncEnumerable())
        {
            await Task.Delay(20);
            yield return item;
        }
    }

    public Item Map(Item item)
    {
        
        return item;
    }
}

public class Item
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public DateTime Birthday { get; set; }
}

public class LocalDbContext : DbContext
{
    public DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite(@"DataSource=local.db;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ItemEntityConfiguration());
    }
}

public class ItemEntityConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
    }
}