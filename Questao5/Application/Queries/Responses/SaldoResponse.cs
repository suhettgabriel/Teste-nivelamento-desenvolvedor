namespace Questao5.Application.Queries.Responses
{
    public class SaldoResponse
    {
        public int AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string Timestamp { get; set; }
        public decimal Balance { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

}
