using EfCoreDdd.Controllers.Requests;
using EfCoreDdd.Infrastructure.Dapper.Repositories;
using EfCoreDdd.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreDdd.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class GroupController : ControllerBase
{
    private readonly LocalDbContext _context;

    public GroupController(LocalDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddGroup([FromBody] AddGroup request, CancellationToken cancellationToken)
    {
        var owner = await _context.Owners.FindAsync(new object[] { request.UserId }, cancellationToken);
        if (owner is null)
        {
            return NotFound("Owner is not found");
        }

        var group = owner.CreateGroup(request.GroupName);

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(group.Id);
    }

    [HttpGet("{ownerId}")]
    public async Task<IActionResult> GetGroups([FromRoute] long ownerId)
    {
        var repo = new QueryRepository();
        var groups = await repo.GetGroups(ownerId);
        return Ok(groups);
    }
}