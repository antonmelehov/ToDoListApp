namespace TodoListApp.WebApi.Data.Entities;

/// <summary>
/// Defines the possible states of a to-do task.
/// </summary>
internal enum TodoTaskStatus
{
    /// <summary>
    /// The task has not been started.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// The task is being worked on.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// The task has been completed.
    /// </summary>
    Completed = 2,
}
