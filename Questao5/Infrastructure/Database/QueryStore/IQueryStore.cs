namespace Questao5.Infrastructure.Database.QueryStore
{
    using Domain.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQueryStore
    {
        Task<Account> GetAccountById(string accountId);
        Task<IEnumerable<Movement>> GetCreditsByAccountId(string accountId);
        Task<IEnumerable<Movement>> GetDebitsByAccountId(string accountId);
        Task<Movement> GetMovementByIdempotencyKey(string idempotencyKey);
    }
}