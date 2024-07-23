using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> Movimentar([FromBody] MovimentacaoRequest request)
        {
            var response = await _mediator.Send(request);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("API is running");
        }
    }
}