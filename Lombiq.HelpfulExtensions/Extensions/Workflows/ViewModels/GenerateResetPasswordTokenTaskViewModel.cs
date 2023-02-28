using System.ComponentModel.DataAnnotations;

namespace Lombiq.HelpfulExtensions.Extensions.Workflows.ViewModels;

public class GenerateResetPasswordTokenTaskViewModel
{
    [Required]
    public string UserExpression { get; set; }
    public string ResetPasswordTokenPropertyKey { get; set; }
    public string ResetPasswordUrlPropertyKey { get; set; }
}
