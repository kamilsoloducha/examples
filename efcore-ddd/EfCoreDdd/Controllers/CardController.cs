using EfCoreDdd.Controllers.Requests;
using EfCoreDdd.Infrastructure.DataAccess;
using EfCoreDdd.Model.Commands;
using EfCoreDdd.Model.Entities;
using EfCoreDdd.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpdateCard = EfCoreDdd.Controllers.Requests.UpdateCard;

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
        var group = await _context.Groups.Include(x => x.Cards)
            .FirstOrDefaultAsync(x => x.Id == request.GroupId, cancellationToken);
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

    [HttpDelete("{cardId}")]
    public async Task<IActionResult> RemoveCard([FromRoute] long cardId, CancellationToken cancellationToken)
    {
        var card = await _context.Cards.FirstOrDefaultAsync(x => x.Id == cardId, cancellationToken);
        if (card is null)
        {
            return BadRequest("Card not found");
        }

        card.Remove();
        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCard([FromBody] UpdateCard request, CancellationToken cancellationToken)
    {
        var card = await _context.Cards.FirstOrDefaultAsync(x => x.Id == request.CardId, cancellationToken);
        if (card is null)
        {
            return BadRequest("Card not found");
        }

        var updateCardCommand = new Model.Commands.UpdateCard(
            new Model.Commands.Side(new Label(request.Front.Label), new Example(request.Front.Example),
                request.Front.Comment, request.Front.UseAsQuestion),
            new Model.Commands.Side(new Label(request.Back.Label), new Example(request.Back.Example),
                request.Back.Comment, request.Back.UseAsQuestion),
            request.IsTicked);

        card.Update(updateCardCommand);

        _context.Set<Card>().Attach(card);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}