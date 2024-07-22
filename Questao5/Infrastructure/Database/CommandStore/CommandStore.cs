using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore;

public class CommandStore : ICommandStore
{
    private readonly SqliteConnection _connection;

    public CommandStore(SqliteConnection connection)
    {
        _connection = connection;
    }

    public async Task CreateMovement(Movement movement)
    {
        var sql = "INSERT INTO Movimentos (Idmovimento, Idcontacorrente, Datamovimento, Tipomovimento, Valor) VALUES (@Idmovimento, @Idcontacorrente, @Datamovimento, @Tipomovimento, @Valor)";
        await _connection.ExecuteAsync(sql, movement);
    }

    public async Task SaveIdempotencyKey(string idempotencyKey, MovimentacaoRequest request, MovimentacaoResponse response)
    {
        var sql = "INSERT INTO IdempotencyKeys (IdempotencyKey, Request, Response) VALUES (@IdempotencyKey, @Request, @Response)";
        await _connection.ExecuteAsync(sql, new { IdempotencyKey = idempotencyKey, Request = request, Response = response });
    }
}
