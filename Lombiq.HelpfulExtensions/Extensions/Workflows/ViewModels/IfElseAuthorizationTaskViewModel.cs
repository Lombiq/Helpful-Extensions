using System.ComponentModel.DataAnnotations;
namespace Lombiq.HelpfulExtensions.Extensions.Workflows.ViewModels;

public class IfElseAuthorizationTaskViewModel
{
    [Required]
    public string Permission { get; set; }

    [Required]
    public string ContentItemIdExpression { get; set; }
}
