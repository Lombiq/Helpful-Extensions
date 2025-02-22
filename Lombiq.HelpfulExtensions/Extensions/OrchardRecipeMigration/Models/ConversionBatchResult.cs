using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Models;

public class ConversionBatchResult
{
    public int Processed { get; set; }
    public int Total { get; set; }
    public string NextPage { get; set; }
    public IEnumerable<ContentItem> ContentItems { get; set; } = [];
}
