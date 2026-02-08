using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Scaffolded.Entities;

public partial class WalletTransaction
{
    public Guid TransactionId { get; set; }

    public Guid WalletId { get; set; }

    public decimal Amount { get; set; }

    public string SourceType { get; set; } = null!;

    public Guid? SourceId { get; set; }

    public string? Metadata { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CoinConversion> CoinConversions { get; set; } = new List<CoinConversion>();

    public virtual Wallet Wallet { get; set; } = null!;
}
