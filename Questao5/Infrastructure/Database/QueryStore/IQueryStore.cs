using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public interface IQueryStore
    {
        Task<Account> GetAccountById(string accountId);
        Task<IEnumerable<Movement>> GetCreditsByAccountId(string accountId);
        Task<IEnumerable<Movement>> GetDebitsByAccountId(string accountId);
        Task<Movement> GetMovementByIdempotencyKey(string idempotencyKey);
    }
}
