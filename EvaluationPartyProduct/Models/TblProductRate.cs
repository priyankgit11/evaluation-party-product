using System;
using System.Collections.Generic;

namespace EvaluationPartyProduct.Models;

public partial class TblProductRate
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public decimal Rate { get; set; }

    public virtual TblProduct Product { get; set; } = null!;
}
