namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a reusable label that can be associated with tasks.
/// </summary>
internal sealed class TagEntity : Entity
{
    /// <summary>
    /// Gets or sets the tag name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets the associations between this tag and its tasks.
    /// </summary>
    public ICollection<TaskTagEntity> TaskTags { get; } = [];
}
