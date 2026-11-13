namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents an association between a task and a tag.
/// </summary>
internal sealed class TaskTagEntity
{
    /// <summary>
    /// Gets or sets the task identifier.
    /// </summary>
    public Guid TodoTaskId { get; set; }

    /// <summary>
    /// Gets or sets the task associated with the tag.
    /// </summary>
    public TodoTaskEntity TodoTask { get; set; } = null!;

    /// <summary>
    /// Gets or sets the tag identifier.
    /// </summary>
    public Guid TagId { get; set; }

    /// <summary>
    /// Gets or sets the tag associated with the task.
    /// </summary>
    public TagEntity Tag { get; set; } = null!;
}
