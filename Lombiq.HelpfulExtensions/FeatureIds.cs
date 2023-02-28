namespace Lombiq.HelpfulExtensions;

public static class FeatureIds
{
    private const string FeatureIdPrefix = "Lombiq.HelpfulExtensions.";

    public const string CodeGeneration = FeatureIdPrefix + nameof(CodeGeneration);
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
    public const string ResetPasswordActivity = Workflows + "." + nameof(ResetPasswordActivity);
}
