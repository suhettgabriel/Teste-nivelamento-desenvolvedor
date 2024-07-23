using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;
using Xunit;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoHandlerTests
    {
        private readonly ICommandStore _commandStore;
        private readonly IQueryStore _queryStore;
        private readonly MovimentacaoHandler _handler;

        public MovimentacaoHandlerTests()
        {
            _commandStore = Substitute.For<ICommandStore>();
            _queryStore = Substitute.For<IQueryStore>();
            _handler = new MovimentacaoHandler(_commandStore, _queryStore);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldCreateMovement()
        {
            // Arrange
            var request = new MovimentacaoRequest
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                AccountId = "valid-account-id",
                Amount = 100.00m,
                MovementType = "C"
            };

            _queryStore.GetAccountById(request.AccountId).Returns(new Account { Id = request.AccountId, Ativo = true });
            _queryStore.GetMovementByIdempotencyKey(request.IdempotencyKey).Returns((Movement)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.MovementId);
            await _commandStore.Received(1).CreateMovement(Arg.Is<Movement>(m => m.Valor == request.Amount));
            await _commandStore.Received(1).SaveIdempotencyKey(request.IdempotencyKey, request, response);
        }

        [Fact]
        public async Task Handle_InvalidAccount_ShouldReturnError()
        {
            // Arrange
            var request = new MovimentacaoRequest
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                AccountId = "invalid-account-id",
                Amount = 100.00m,
                MovementType = "C"
            };

            _queryStore.GetAccountById(request.AccountId).Returns((Account)null);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(response.Success);
            Assert.Equal("INVALID_ACCOUNT", response.Message);
        }
    }
}