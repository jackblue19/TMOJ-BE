using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Payment
{
    public Guid PaymentId { get; set; }

    public Guid? UserId { get; set; }

    public string? ProviderName { get; set; }

    public string? ProviderTxId { get; set; }

    public string? BankCode { get; set; }

    public string? PaymentTxn { get; set; }

    public decimal AmountMoney { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public virtual ICollection<CoinConversion> CoinConversions { get; set; } = new List<CoinConversion>();

    public virtual User? User { get; set; }
}
