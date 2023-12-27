namespace EvaluationPartyProduct.DTO
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; } = null!;
        public DateTime Date { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
