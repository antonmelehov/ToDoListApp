namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Defines the access granted to a user through list sharing.
/// </summary>
internal enum ListAccessLevel
{
    /// <summary>
    /// Allows reading the list and its contents.
    /// </summary>
    Viewer = 1,

    /// <summary>
    /// Allows collaborating on tasks within the list.
    /// </summary>
    Editor = 2,
}
