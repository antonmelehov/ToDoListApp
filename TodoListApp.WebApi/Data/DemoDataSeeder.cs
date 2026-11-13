using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Data;

/// <summary>
/// Creates demo data for the local development database.
/// </summary>
internal static class DemoDataSeeder
{
    /// <summary>
    /// Creates any missing demo lists using the exported Identity IDs.
    /// </summary>
    /// <param name="db">The application database context.</param>
    /// <param name="inputPath">The path to the WebApp's demo user ID export.</param>
    /// <returns>A task that completes when the changes are saved.</returns>
    public static async Task SeedAsync(TodoListDbContext db, string inputPath)
    {
        await using var stream = File.OpenRead(inputPath);
        using var document = await JsonDocument.ParseAsync(stream);

        var users = document.RootElement;
        var aliceId = GetUserId(users, "AliceId");
        var carolId = GetUserId(users, "CarolId");
        var bobId = GetUserId(users, "BobId");
        var sharedProjectId = Guid.Parse("8e6f75da-2c89-4b14-b538-f62b4109aa01");
        var privatePlanningId = Guid.Parse("8e6f75da-2c89-4b14-b538-f62b4109aa02");
        var createdAtUtc = DateTime.UtcNow;

        TodoListEntity[] lists =
        [
            new()
            {
                Id = sharedProjectId,
                OwnerUserId = aliceId,
                Title = "Shared project",
                CreatedAtUtc = createdAtUtc,
            },
            new()
            {
                Id = privatePlanningId,
                OwnerUserId = aliceId,
                Title = "Private planning",
                CreatedAtUtc = createdAtUtc,
            },
            new()
            {
                Id = Guid.Parse("8e6f75da-2c89-4b14-b538-f62b4109aa03"),
                OwnerUserId = carolId,
                Title = "Carol's personal list",
                CreatedAtUtc = createdAtUtc,
            },
        ];

        foreach (var list in lists)
        {
            if (await db.TodoLists.FindAsync(list.Id) is null)
            {
                _ = db.TodoLists.Add(list);
            }
        }

        TodoListMemberEntity[] memberships =
        [
            new()
            {
                TodoListId = sharedProjectId,
                UserId = bobId,
                AccessLevel = ListAccessLevel.Viewer,
            },
            new()
            {
                TodoListId = sharedProjectId,
                UserId = carolId,
                AccessLevel = ListAccessLevel.Editor,
            },
            new()
            {
                TodoListId = privatePlanningId,
                UserId = carolId,
                AccessLevel = ListAccessLevel.Editor,
            },
        ];

        foreach (var membership in memberships)
        {
            if (await db.TodoListMembers.FindAsync(
                    membership.TodoListId, membership.UserId) is null)
            {
                _ = db.TodoListMembers.Add(membership);
            }
        }

        var today = DateOnly.FromDateTime(createdAtUtc);

        TodoTaskEntity[] tasks =
        [
            new()
            {
                Id = Guid.Parse("3a5c1fd2-3f8e-4a40-9aa2-1b7d932b1001"),
                TodoListId = sharedProjectId,
                CreatedByUserId = aliceId,
                AssigneeUserId = bobId,
                Title = "Review the project brief",
                DueDate = today.AddDays(-1),
                Status = TodoTaskStatus.NotStarted,
                CreatedAtUtc = createdAtUtc.AddDays(-3),
            },
            new()
            {
                Id = Guid.Parse("3a5c1fd2-3f8e-4a40-9aa2-1b7d932b1002"),
                TodoListId = sharedProjectId,
                CreatedByUserId = aliceId,
                AssigneeUserId = carolId,
                Title = "Draft the project plan",
                DueDate = today.AddDays(3),
                Status = TodoTaskStatus.InProgress,
                CreatedAtUtc = createdAtUtc.AddDays(-3),
            },
            new()
            {
                Id = Guid.Parse("3a5c1fd2-3f8e-4a40-9aa2-1b7d932b1003"),
                TodoListId = privatePlanningId,
                CreatedByUserId = aliceId,
                AssigneeUserId = bobId,
                Title = "Confirm the meeting time",
                DueDate = today.AddDays(1),
                Status = TodoTaskStatus.Completed,
                CreatedAtUtc = createdAtUtc.AddDays(-3),
            },
        ];

        foreach (var task in tasks)
        {
            if (await db.TodoTasks.FindAsync(task.Id) is null)
            {
                _ = db.TodoTasks.Add(task);
            }
        }

        (Guid Id, string Name, Guid[] TaskIds)[] tagSeeds =
        [
            (
                Guid.Parse("5c49f8c0-1963-47e9-b54d-cf85ca00a001"),
                "Project",
                [tasks[0].Id, tasks[1].Id]),
            (
                Guid.Parse("5c49f8c0-1963-47e9-b54d-cf85ca00a002"),
                "Urgent",
                [tasks[0].Id]),
            (
                Guid.Parse("5c49f8c0-1963-47e9-b54d-cf85ca00a003"),
                "Scheduling",
                [tasks[2].Id])
        ];

        foreach (var seed in tagSeeds)
        {
            var tag = await db.Tags.FindAsync(seed.Id)
                      ?? await db.Tags.SingleOrDefaultAsync(existing => existing.Name == seed.Name);

            if (tag is null)
            {
                tag = new TagEntity { Id = seed.Id, Name = seed.Name };
                _ = db.Tags.Add(tag);
            }

            foreach (var taskId in seed.TaskIds)
            {
                if (await db.TaskTags.FindAsync(taskId, tag.Id) is null)
                {
                    _ = db.TaskTags.Add(new TaskTagEntity
                    {
                        TodoTaskId = taskId,
                        TagId = tag.Id,
                    });
                }
            }
        }

        CommentEntity[] comments =
        [
            new()
            {
                Id = Guid.Parse("c274dba0-8341-4f3f-8e6b-5fdba0200001"),
                TodoTaskId = tasks[0].Id,
                AuthorUserId = aliceId,
                Text = "Please flag any unclear requirements in the brief.",
                CreatedAtUtc = createdAtUtc,
            },
            new()
            {
                Id = Guid.Parse("c274dba0-8341-4f3f-8e6b-5fdba0200002"),
                TodoTaskId = tasks[0].Id,
                AuthorUserId = carolId,
                Text = "I added the missing delivery dates to the brief.",
                CreatedAtUtc = createdAtUtc,
            },
            new()
            {
                Id = Guid.Parse("c274dba0-8341-4f3f-8e6b-5fdba0200003"),
                TodoTaskId = tasks[2].Id,
                AuthorUserId = aliceId,
                Text = "The meeting time is confirmed.",
                CreatedAtUtc = createdAtUtc,
            },
        ];

        foreach (var comment in comments)
        {
            if (await db.Comments.FindAsync(comment.Id) is null)
            {
                _ = db.Comments.Add(comment);
            }
        }

        _ = await db.SaveChangesAsync();
    }

    private static string GetUserId(JsonElement users, string propertyName)
    {
        if (users.ValueKind == JsonValueKind.Object
            && users.TryGetProperty(propertyName, out var value)
            && value.ValueKind == JsonValueKind.String
            && value.GetString() is { } userId
            && !string.IsNullOrWhiteSpace(userId)
            && userId.Length <= 128)
        {
            return userId;
        }

        throw new InvalidOperationException(
            $"The demo user ID file must contain a valid '{propertyName}'.");
    }
}
