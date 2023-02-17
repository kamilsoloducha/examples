using EfCoreDdd.Controllers.Requests;
using EfCoreDdd.Infrastructure.DataAccess;
using EfCoreDdd.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreDdd.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class CardController : ControllerBase
{
    private readonly LocalDbContext _context;

    public CardController(LocalDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddCard([FromBody] AddCard request, CancellationToken cancellationToken)
    {
        var group = await _context.Groups.FindAsync(new object[] { request.GroupId }, cancellationToken);
        if (group is null)
        {
            return BadRequest("Group not found");
        }

        var frontLabel = new Label(request.Front);
        var backLabel = new Label(request.Back);
        
        var frontExample = new Example(request.FrontExample);
        var backExample = new Example(request.BackExample);
        
        var card = group.AddCard(frontLabel, backLabel, frontExample, backExample);

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(card.Id);
    }
}