namespace Lombiq.HelpfulExtensions;

public static class FeatureIds
{
    public const string Base = "Lombiq.HelpfulExtensions";
    private const string FeatureIdPrefix = Base + ".";

    public const string CodeGeneration = FeatureIdPrefix + nameof(CodeGeneration);
    public const string ContentSets = FeatureIdPrefix + nameof(ContentSets);
    public const string ContentTypes = FeatureIdPrefix + nameof(ContentTypes);
    public const string Flows = FeatureIdPrefix + nameof(Flows);
    public const string ShapeTracing = FeatureIdPrefix + nameof(ShapeTracing);
    public const string Widgets = FeatureIdPrefix + nameof(Widgets);
    public const string Emails = FeatureIdPrefix + nameof(Emails);
    public const string Security = FeatureIdPrefix + nameof(Security);
    public const string TargetBlank = FeatureIdPrefix + nameof(TargetBlank);
    public const string SiteTexts = FeatureIdPrefix + nameof(SiteTexts);
    public const string OrchardRecipeMigration = FeatureIdPrefix + nameof(OrchardRecipeMigration);
    public const string Workflows = FeatureIdPrefix + nameof(Workflows);
    public const string Trumbowyg = FeatureIdPrefix + nameof(Trumbowyg);
    public const string ResetPasswordActivity = Workflows + "." + nameof(ResetPasswordActivity);
    public const string ResourceManagement = FeatureIdPrefix + nameof(ResourceManagement);
}
