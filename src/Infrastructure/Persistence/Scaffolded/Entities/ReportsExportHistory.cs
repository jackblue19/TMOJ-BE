using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Scaffolded.Entities;

public partial class ReportsExportHistory
{
    public Guid ExportId { get; set; }

    public Guid? GeneratedBy { get; set; }

    public string ReportType { get; set; } = null!;

    public string? FilePath { get; set; }

    public DateTime GeneratedAt { get; set; }

    public virtual User? GeneratedByNavigation { get; set; }
}
