namespace EvaluationPartyProduct.DTO
{
    public class InvoiceDetailDTO
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public decimal Rate { get; set; }
        public int Quantity { get; set; }
        public string PartyName { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal Total { get; set; }
    }
}
