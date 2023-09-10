using OrchardCore.ContentManagement;
using System.Diagnostics.CodeAnalysis;

namespace Lombiq.HelpfulExtensions.Extensions.ContentSets.Models;

[SuppressMessage(
    "Minor Code Smell",
    "S2094:Classes should not be empty",
    Justification = "Only data we need is the field name.")]
public class ContentSetContentPickerField : ContentField
{
}
