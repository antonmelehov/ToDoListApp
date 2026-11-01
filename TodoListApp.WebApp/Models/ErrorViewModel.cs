using System.Diagnostics.CodeAnalysis;

namespace TodoListApp.WebApp.Models;

/// <summary>
/// Contains diagnostic information displayed on the error page.
/// </summary>
[SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Justification = "The view model must be accessible to compiled Razor views.")]
public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets the identifier used to trace the failed request.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether a request identifier is available.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
