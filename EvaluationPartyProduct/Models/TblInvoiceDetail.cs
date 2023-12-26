using System;
using System.Collections.Generic;

namespace EvaluationPartyProduct.Models;

public partial class TblInvoiceDetail
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public int ProductId { get; set; }

    public decimal Rate { get; set; }

    public int Quantity { get; set; }

    public virtual TblInvoice Invoice { get; set; } = null!;

    public virtual TblProduct Product { get; set; } = null!;
}
