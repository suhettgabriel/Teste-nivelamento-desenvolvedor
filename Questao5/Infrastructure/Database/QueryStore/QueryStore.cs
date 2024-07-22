using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore;

public class QueryStore : IQueryStore
{
    private readonly SqliteConnection _connection;

    public QueryStore(SqliteConnection connection)
    {
        _connection = connection;
    }

    public async Task<Account> GetAccountById(string accountId)
    {
        var sql = "SELECT * FROM ContasCorrentes WHERE Idconta = @AccountId";
        return (await _connection.QueryAsync<Account>(sql, new { AccountId = accountId })).FirstOrDefault();
    }

    public async Task<IEnumerable<Movement>> GetCreditsByAccountId(string accountId)
    {
        var sql = "SELECT * FROM Movimentos WHERE Idcontacorrente = @AccountId AND Tipomovimento = 'C'";
        return await _connection.QueryAsync<Movement>(sql, new { AccountId = accountId });
    }

    public async Task<IEnumerable<Movement>> GetDebitsByAccountId(string accountId)
    {
        var sql = "SELECT * FROM Movimentos WHERE Idcontacorrente = @AccountId AND Tipomovimento = 'D'";
        return await _connection.QueryAsync<Movement>(sql, new { AccountId = accountId });
    }

    public async Task<Movement> GetMovementByIdempotencyKey(string idempotencyKey)
    {
        var sql = "SELECT * FROM IdempotencyKeys WHERE IdempotencyKey = @IdempotencyKey";
        return (await _connection.QueryAsync<Movement>(sql, new { IdempotencyKey = idempotencyKey })).FirstOrDefault();
    }
}
