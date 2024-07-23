using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class QueryStore : IQueryStore
    {
        private readonly SqliteConnection _connection;

        public QueryStore(SqliteConnection connection)
        {
            _connection = connection;
        }

        public async Task<Account> GetAccountById(string accountId)
        {
            var sql = "SELECT * FROM contacorrente WHERE idcontacorrente = @AccountId";
            return (await _connection.QueryAsync<Account>(sql, new { AccountId = accountId })).FirstOrDefault();
        }

        public async Task<IEnumerable<Movement>> GetCreditsByAccountId(string accountId)
        {
            var sql = "SELECT * FROM movimento WHERE idcontacorrente = @AccountId AND tipomovimento = 'C'";
            return await _connection.QueryAsync<Movement>(sql, new { AccountId = accountId });
        }

        public async Task<IEnumerable<Movement>> GetDebitsByAccountId(string accountId)
        {
            var sql = "SELECT * FROM movimento WHERE idcontacorrente = @AccountId AND tipomovimento = 'D'";
            return await _connection.QueryAsync<Movement>(sql, new { AccountId = accountId });
        }

        public async Task<Movement> GetMovementByIdempotencyKey(string idempotencyKey)
        {
            var sql = "SELECT * FROM idempotencia WHERE chave_idempotencia = @IdempotencyKey";
            return (await _connection.QueryAsync<Movement>(sql, new { IdempotencyKey = idempotencyKey })).FirstOrDefault();
        }
    }
}