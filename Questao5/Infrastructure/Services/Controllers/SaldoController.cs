using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Queries.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaldoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaldoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("saldo/{accountId}")]
        public async Task<IActionResult> GetSaldo(string accountId)
        {
            var request = new SaldoRequest { AccountId = accountId };
            var response = await _mediator.Send(request);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
