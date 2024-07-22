using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore;
using Xunit;

public class SaldoHandlerTests
{
    private readonly IQueryStore _queryStore;
    private readonly SaldoHandler _handler;

    public SaldoHandlerTests()
    {
        _queryStore = Substitute.For<IQueryStore>();
        _handler = new SaldoHandler(_queryStore);
    }

    [Fact]
    public async Task Handle_ValidAccount_ShouldReturnBalance()
    {
        // Arrange
        var request = new SaldoRequest { AccountId = "valid-account-id" };
        var account = new Account { Id = request.AccountId, Numero = 123, Nome = "John Doe", Ativo = true };
        var movements = new[]
        {
            new Movement { Valor = 100.00m, Tipomovimento = "C" },
            new Movement { Valor = 50.00m, Tipomovimento = "D" }
        };

        _queryStore.GetAccountById(request.AccountId).Returns(account);
        _queryStore.GetCreditsByAccountId(request.AccountId).Returns(movements.Where(m => m.Tipomovimento == "C"));
        _queryStore.GetDebitsByAccountId(request.AccountId).Returns(movements.Where(m => m.Tipomovimento == "D"));

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal(account.Numero, response.AccountNumber);
        Assert.Equal(account.Nome, response.AccountHolderName);
        Assert.Equal(50.00m, response.Balance); // 100 - 50 = 50
    }

    [Fact]
    public async Task Handle_InvalidAccount_ShouldReturnError()
    {
        // Arrange
        var request = new SaldoRequest { AccountId = "invalid-account-id" };

        _queryStore.GetAccountById(request.AccountId).Returns((Account)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("INVALID_ACCOUNT", response.Message);
    }
}
