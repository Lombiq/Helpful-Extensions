using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lombiq.HelpfulExtensions.Extensions.Trumbowyg.Constants;

public static class TrumbowygResourceNames
{
    public const string Prism = nameof(Prism);
    public const string PrismCoyTheme = nameof(PrismCoyTheme);
    public const string PrismLineHighlight = nameof(PrismLineHighlight);
    public const string TrumbowygHighlight = nameof(TrumbowygHighlight);
    public const string TrumbowygHighlightExtension = nameof(TrumbowygHighlightExtension);
    public const string MarkupTemplating = "markup-templating";
}

public static class PrismLanguageNames
{
    public const string Csharp = nameof(Csharp);
    public const string Docker = nameof(Docker);
    public const string Json = nameof(Json);
    public const string Markdown = nameof(Markdown);
    public const string Sql = nameof(Sql);
    public const string Liquid = nameof(Liquid);
    public const string Regex = nameof(Regex);
    public const string Powershell = nameof(Powershell);

    public static readonly IReadOnlyList<string> AllLanguage =
        typeof(PrismLanguageNames)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(fieldInfo => fieldInfo.IsLiteral && fieldInfo.FieldType == typeof(string))
            .Select(fieldInfo => (string)fieldInfo.GetRawConstantValue()!)
            .ToArray();
}
