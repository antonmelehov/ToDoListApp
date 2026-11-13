namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a to-do list stored in the application database.
/// </summary>
internal sealed class TodoListEntity : AuditableEntity
{
    /// <summary>
    /// Gets or sets the Identity identifier of the list owner.
    /// </summary>
    public required string OwnerUserId { get; set; }

    /// <summary>
    /// Gets or sets the list title.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the optional list description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets the tasks belonging to this list.
    /// </summary>
    public ICollection<TodoTaskEntity> Tasks { get; } = [];

    /// <summary>
    /// Gets the memberships granting shared access to this list.
    /// </summary>
    public ICollection<TodoListMemberEntity> Members { get; } = [];
}
