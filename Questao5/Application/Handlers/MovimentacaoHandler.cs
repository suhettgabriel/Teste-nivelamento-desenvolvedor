using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;
using System.Threading;
using System.Threading.Tasks;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoHandler : IRequestHandler<MovimentacaoRequest, MovimentacaoResponse>
    {
        private readonly ICommandStore _commandStore;
        private readonly IQueryStore _queryStore;

        public MovimentacaoHandler(ICommandStore commandStore, IQueryStore queryStore)
        {
            _commandStore = commandStore;
            _queryStore = queryStore;
        }

        public async Task<MovimentacaoResponse> Handle(MovimentacaoRequest request, CancellationToken cancellationToken)
        {
            var account = await _queryStore.GetAccountById(request.AccountId);
            if (account == null)
                return new MovimentacaoResponse { Success = false, Message = "INVALID_ACCOUNT" };

            if (!account.Ativo)
                return new MovimentacaoResponse { Success = false, Message = "INACTIVE_ACCOUNT" };

            if (request.Amount <= 0)
                return new MovimentacaoResponse { Success = false, Message = "INVALID_VALUE" };

            if (request.MovementType != "C" && request.MovementType != "D")
                return new MovimentacaoResponse { Success = false, Message = "INVALID_TYPE" };

            var existingIdempotency = await _queryStore.GetMovementByIdempotencyKey(request.IdempotencyKey);
            if (existingIdempotency != null)
                return new MovimentacaoResponse { Success = true, MovementId = existingIdempotency.Idmovimento };

            var newMovementId = Guid.NewGuid().ToString();
            var movement = new Movement
            {
                Idmovimento = newMovementId,
                Idcontacorrente = request.AccountId,
                Datamovimento = DateTime.Now.ToString("dd/MM/yyyy"),
                Tipomovimento = request.MovementType,
                Valor = request.Amount
            };

            await _commandStore.CreateMovement(movement);
            await _commandStore.SaveIdempotencyKey(request.IdempotencyKey, request, new MovimentacaoResponse { Success = true, MovementId = newMovementId });

            return new MovimentacaoResponse { Success = true, MovementId = newMovementId };
        }
    }
}
