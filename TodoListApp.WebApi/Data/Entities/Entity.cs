namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Provides an identifier for an application entity.
/// </summary>
internal abstract class Entity
{
    /// <summary>
    /// Gets or sets the entity identifier.
    /// </summary>
    public Guid Id { get; set; }
}
