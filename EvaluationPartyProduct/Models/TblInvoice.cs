using System;
using System.Collections.Generic;

namespace EvaluationPartyProduct.Models;

public partial class TblInvoice
{
    public int Id { get; set; }

    public int PartyId { get; set; }

    public DateTime Date { get; set; }

    public virtual TblParty Party { get; set; } = null!;

    public virtual ICollection<TblInvoiceDetail> TblInvoiceDetails { get; set; } = new List<TblInvoiceDetail>();
}
