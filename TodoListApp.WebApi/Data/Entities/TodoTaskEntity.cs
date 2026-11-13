namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a task belonging to a to-do list.
/// </summary>
internal sealed class TodoTaskEntity : AuditableEntity
{
    /// <summary>
    /// Gets or sets the identifier of the containing list.
    /// </summary>
    public Guid TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the list containing this task.
    /// </summary>
    public TodoListEntity TodoList { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Identity identifier of the task creator.
    /// </summary>
    public required string CreatedByUserId { get; set; }

    /// <summary>
    /// Gets or sets the Identity identifier of the responsible user.
    /// </summary>
    public required string AssigneeUserId { get; set; }

    /// <summary>
    /// Gets or sets the task title.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the optional task description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the calendar date by which the task is due.
    /// </summary>
    public DateOnly DueDate { get; set; }

    /// <summary>
    /// Gets or sets the task status.
    /// </summary>
    public TodoTaskStatus Status { get; set; }

    /// <summary>
    /// Gets the comments added to this task.
    /// </summary>
    public ICollection<CommentEntity> Comments { get; } = [];

    /// <summary>
    /// Gets the associations between this task and its tags.
    /// </summary>
    public ICollection<TaskTagEntity> TaskTags { get; } = [];
}
