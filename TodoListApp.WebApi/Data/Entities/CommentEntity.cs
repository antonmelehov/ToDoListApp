namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a comment added to a task.
/// </summary>
internal sealed class CommentEntity : AuditableEntity
{
    /// <summary>
    /// Gets or sets the identifier of the commented task.
    /// </summary>
    public Guid TodoTaskId { get; set; }

    /// <summary>
    /// Gets or sets the task to which this comment belongs.
    /// </summary>
    public TodoTaskEntity TodoTask { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Identity identifier of the comment author.
    /// </summary>
    public required string AuthorUserId { get; set; }

    /// <summary>
    /// Gets or sets the comment text.
    /// </summary>
    public required string Text { get; set; }
}
