using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public interface ICommandStore
    {
        Task CreateMovement(Movement movement);
        Task SaveIdempotencyKey(string idempotencyKey, MovimentacaoRequest request, MovimentacaoResponse response);
    }
}
