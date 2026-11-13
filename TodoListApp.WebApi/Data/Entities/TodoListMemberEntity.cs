namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Represents a user's shared access to a to-do list.
/// </summary>
internal sealed class TodoListMemberEntity
{
    /// <summary>
    /// Gets or sets the identifier of the shared list.
    /// </summary>
    public Guid TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the list shared with this member.
    /// </summary>
    public TodoListEntity TodoList { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Identity identifier of the member.
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// Gets or sets the member's access to the list.
    /// </summary>
    public ListAccessLevel AccessLevel { get; set; }
}
