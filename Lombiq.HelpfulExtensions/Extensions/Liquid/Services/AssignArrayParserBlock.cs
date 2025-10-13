using Fluid;
using Fluid.Ast;
using Fluid.Values;
using Lombiq.HelpfulExtensions.Extensions.Liquid.Helpers;
using Lombiq.HelpfulLibraries.OrchardCore.Liquid;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Lombiq.HelpfulExtensions.Extensions.Liquid;

public class AssignArrayParserBlock : ILiquidParserTag
{
    public async ValueTask<Completion> WriteToAsync(
        IReadOnlyList<FilterArgument> argumentsList,
        TextWriter writer,
        TextEncoder encoder,
        TemplateContext context)
    {
        var arguments = await LiquidParserHelpers.EvaluateAsync(argumentsList, context);

        if (arguments.Count >= 1)
        {
            context.SetValue(
                arguments[0].ToStringValue(),
                FluidValue.Create(arguments.Skip(1).ToArray(), context.Options));
        }

        return Completion.Normal;
    }
}
