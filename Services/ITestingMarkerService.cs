using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Lombiq.HelpfulExtensions.Services
{
    /// <summary>
    /// This service does nothing. However if you inject <see cref="IEnumerable{ITestingMarkerService}"/> to your
    /// service implementation, you will be able to do <c>_isTesting = testingMarkerServices.Any();</c> to tell if the
    /// testing feature is enabled.
    /// </summary>
    [SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Necessary marker interface.")]
    public interface ITestingMarkerService
    {
    }
}
