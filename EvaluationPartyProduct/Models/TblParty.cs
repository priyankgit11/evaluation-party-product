using System;
using System.Collections.Generic;

namespace EvaluationPartyProduct.Models;

public partial class TblParty
{
    public int Id { get; set; }

    public string PartyName { get; set; } = null!;

    public virtual ICollection<TblAssignParty> TblAssignParties { get; set; } = new List<TblAssignParty>();

    public virtual ICollection<TblInvoice> TblInvoices { get; set; } = new List<TblInvoice>();
}
