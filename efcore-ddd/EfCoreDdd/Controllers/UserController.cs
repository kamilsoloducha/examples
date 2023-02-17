using EfCoreDdd.Controllers.Requests;
using EfCoreDdd.Infrastructure.DataAccess;
using EfCoreDdd.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDdd.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class UserController : ControllerBase
{
    private readonly LocalDbContext _context;

    public UserController(LocalDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewOwner([FromBody] AddOwner request, CancellationToken cancellationToken)
    {
        if (await _context.Owners.AnyAsync(x => x.Id == request.UserId, cancellationToken))
        {
            return BadRequest("Owner already exists");
        }
        
        var newOwner = new Owner();
        
        _context.Owners.Attach(newOwner);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Ok(newOwner.Id);
    }
}