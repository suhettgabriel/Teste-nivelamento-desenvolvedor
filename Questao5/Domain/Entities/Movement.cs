namespace Questao5.Domain.Entities
{
    public class Movement
    {
        public string Idmovimento { get; set; }
        public string Idcontacorrente { get; set; }
        public DateTime Datamovimento { get; set; }
        public string Tipomovimento { get; set; }
        public decimal Valor { get; set; }
    }
}