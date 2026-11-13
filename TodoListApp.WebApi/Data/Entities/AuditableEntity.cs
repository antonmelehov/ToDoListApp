namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Provides creation and last-update timestamps for an entity.
/// </summary>
internal abstract class AuditableEntity : Entity
{
    /// <summary>
    /// Gets or sets the UTC creation timestamp.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp of the last update.
    /// </summary>
    public DateTime? UpdatedAtUtc { get; set; }
}
