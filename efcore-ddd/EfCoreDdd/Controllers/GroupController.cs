using EfCoreDdd.Controllers.Requests;
using EfCoreDdd.Infrastructure.Dapper.Repositories;
using EfCoreDdd.Infrastructure.DataAccess;
using EfCoreDdd.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var owner = await _context.Owners.Include(x => x.Groups)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
        if (owner is null)
        {
            return NotFound("Owner is not found");
        }

        var groupName = new GroupName(request.GroupName);

        var group = owner.CreateGroup(groupName);

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(group.Id);
    }

    [HttpDelete("{groupId}")]
    public async Task<IActionResult> RemoveGroup([FromRoute] long groupId, CancellationToken cancellationToken)
    {
        var group = await _context.Groups.Include(x => x.Cards).Include(x => x.Owner).FirstOrDefaultAsync(x => x.Id == groupId, cancellationToken);
        if (group is null)
        {
            return BadRequest("Group is not found");
        }

        group.Remove();

        await _context.SaveChangesAsync(cancellationToken);
        return Ok();
    }

    [HttpGet("{ownerId}")]
    public async Task<IActionResult> GetGroups([FromRoute] long ownerId)
    {
        var repo = new QueryRepository();
        var groups = await repo.GetGroups(ownerId);
        return Ok(groups);
    }
}