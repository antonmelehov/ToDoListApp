using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Data;

/// <summary>
/// Provides access to the to-do application database.
/// </summary>
internal sealed class TodoListDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for this database context.</param>
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the to-do lists.
    /// </summary>
    public DbSet<TodoListEntity> TodoLists => this.Set<TodoListEntity>();

    /// <summary>
    /// Gets the memberships that grant shared list access.
    /// </summary>
    public DbSet<TodoListMemberEntity> TodoListMembers => this.Set<TodoListMemberEntity>();

    /// <summary>
    /// Gets the tasks belonging to to-do lists.
    /// </summary>
    public DbSet<TodoTaskEntity> TodoTasks => this.Set<TodoTaskEntity>();

    /// <summary>
    /// Gets the reusable task labels.
    /// </summary>
    public DbSet<TagEntity> Tags => this.Set<TagEntity>();

    /// <summary>
    /// Gets the associations between tasks and tags.
    /// </summary>
    public DbSet<TaskTagEntity> TaskTags => this.Set<TaskTagEntity>();

    /// <summary>
    /// Gets the comments added to tasks.
    /// </summary>
    public DbSet<CommentEntity> Comments => this.Set<CommentEntity>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var lists = modelBuilder.Entity<TodoListEntity>();
        _ = lists.Property(list => list.Title).HasMaxLength(200);
        _ = lists.Property(list => list.Description).HasMaxLength(2000);
        _ = lists.Property(list => list.OwnerUserId).HasMaxLength(128);

        var members = modelBuilder.Entity<TodoListMemberEntity>();
        _ = members.HasKey(member => new { member.TodoListId, member.UserId });
        _ = members.Property(member => member.UserId).HasMaxLength(128);
        _ = members.ToTable("TodoListMembers", table =>
        table.HasCheckConstraint(
            "CK_TodoListMembers_AccessLevel",
            "[AccessLevel] IN (1, 2)"));

        _ = members.HasOne(member => member.TodoList)
            .WithMany(list => list.Members)
            .HasForeignKey(member => member.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);

        var tasks = modelBuilder.Entity<TodoTaskEntity>();
        _ = tasks.Property(task => task.Title).HasMaxLength(200);
        _ = tasks.Property(task => task.Description).HasMaxLength(2000);
        _ = tasks.Property(task => task.CreatedByUserId).HasMaxLength(128);
        _ = tasks.Property(task => task.AssigneeUserId).HasMaxLength(128);
        _ = tasks.ToTable("TodoTasks", table =>
        table.HasCheckConstraint(
            "CK_TodoTasks_Status",
            "[Status] IN (0, 1, 2)"));

        _ = tasks.HasOne(task => task.TodoList)
        .WithMany(list => list.Tasks)
        .HasForeignKey(task => task.TodoListId)
        .OnDelete(DeleteBehavior.Cascade);

        var comments = modelBuilder.Entity<CommentEntity>();
        _ = comments.Property(comment => comment.Text).HasMaxLength(2000);
        _ = comments.Property(comment => comment.AuthorUserId).HasMaxLength(128);

        _ = comments.HasOne(comment => comment.TodoTask)
        .WithMany(task => task.Comments)
        .HasForeignKey(comment => comment.TodoTaskId)
        .OnDelete(DeleteBehavior.Cascade);

        var tags = modelBuilder.Entity<TagEntity>();
        _ = tags.Property(tag => tag.Name)
        .HasMaxLength(64)
        .UseCollation("Latin1_General_100_CI_AS_SC");

        _ = tags.HasIndex(tag => tag.Name).IsUnique();

        var taskTags = modelBuilder.Entity<TaskTagEntity>();
        _ = taskTags.HasKey(link => new { link.TodoTaskId, link.TagId });

        _ = taskTags.HasOne(link => link.TodoTask)
        .WithMany(task => task.TaskTags)
        .HasForeignKey(link => link.TodoTaskId)
        .OnDelete(DeleteBehavior.Cascade);

        _ = taskTags.HasOne(link => link.Tag)
        .WithMany(tag => tag.TaskTags)
        .HasForeignKey(link => link.TagId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
