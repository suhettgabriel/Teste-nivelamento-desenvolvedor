using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoRequest : IRequest<MovimentacaoResponse>
    {
        public string IdempotencyKey { get; set; }
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public string MovementType { get; set; }
    }

}
