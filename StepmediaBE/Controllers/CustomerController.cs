using System.Threading.Tasks;
using FAI.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StepmediaBE.Infrastructure;
using StepmediaBE.OrderService.CommandHandlers.Customers;

namespace Metatrade.OrderService.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly StepmediaContext _context;

    public CustomerController(IMediator mediator, StepmediaContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _context.Set<Customer>().ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand request)
    {
        await _mediator.Send(request);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCustomerCommand request)
    {
        request.SetCustomerId(id);
        await _mediator.Send(request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteCustomerCommand {CustomerId = id});
        return NoContent();
    }
}