using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Database.QueryStore;

namespace Questao5.Application.Handlers
{
    public class SaldoHandler
    {
        private readonly IQueryStore _queryStore;

        public SaldoHandler(IQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public async Task<SaldoResponse> Handle(SaldoRequest request, CancellationToken cancellationToken)
        {
            var account = await _queryStore.GetAccountById(request.AccountId);
            if (account == null)
                return new SaldoResponse { Success = false, Message = "INVALID_ACCOUNT" };

            if (!account.Ativo)
                return new SaldoResponse { Success = false, Message = "INACTIVE_ACCOUNT" };

            var credits = await _queryStore.GetCreditsByAccountId(request.AccountId);
            var debits = await _queryStore.GetDebitsByAccountId(request.AccountId);

            var balance = credits.Sum(c => c.Valor) - debits.Sum(d => d.Valor);

            return new SaldoResponse
            {
                Success = true,
                AccountNumber = account.Numero,
                AccountHolderName = account.Nome,
                Balance = balance
            };
        }
    }
}
