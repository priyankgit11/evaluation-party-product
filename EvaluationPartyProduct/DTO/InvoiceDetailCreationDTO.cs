namespace EvaluationPartyProduct.DTO
{
    public class InvoiceDetailCreation
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public decimal Rate { get; set; }
        public int Quantity { get; set; }
    }
}
