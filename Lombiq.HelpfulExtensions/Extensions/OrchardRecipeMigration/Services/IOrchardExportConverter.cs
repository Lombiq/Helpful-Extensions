﻿using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lombiq.HelpfulExtensions.Extensions.OrchardRecipeMigration.Services;

/// <summary>
/// A converter for the whole Orchard 1 XML export file.
/// </summary>
public interface IOrchardExportConverter
{
    /// <summary>
    /// Updates the collection of <paramref name="contentItems"/> using data from the whole XML file. This is invoked
    /// before the recipe steps are serialized.
    /// </summary>
    Task UpdateContentItemsAsync(XDocument document, IList<ContentItem> contentItems);
}
