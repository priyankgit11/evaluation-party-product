using System;
using System.Collections.Generic;

namespace EvaluationPartyProduct.Models;

public partial class TblProduct
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public virtual ICollection<TblAssignParty> TblAssignParties { get; set; } = new List<TblAssignParty>();

    public virtual ICollection<TblInvoiceDetail> TblInvoiceDetails { get; set; } = new List<TblInvoiceDetail>();

    public virtual ICollection<TblProductRate> TblProductRates { get; set; } = new List<TblProductRate>();
}
