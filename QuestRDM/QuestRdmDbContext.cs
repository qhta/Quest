namespace QuestRDM;

/// <summary>
/// DbContext for QuestRDM project.
/// </summary>
public class QuestRdmDbContext : DbContext
{
  /// <summary>
  /// Set of projects registered in the database.
  /// </summary>
  public DbSet<Project> Projects { get; set; }

  /// <summary>
  /// Configures the database provider and other options for the current <see cref="DbContext"/> instance.
  /// </summary>
  /// <remarks>This method is called by the Entity Framework runtime and is typically overridden to configure
  /// the database provider  and other context-specific options. In this implementation, SQLite is configured as the
  /// database provider.</remarks>
  /// <param name="optionsBuilder">A builder used to configure the options for the <see cref="DbContext"/>. This parameter cannot be null.</param>
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    // Configure SQLite as the database provider
    optionsBuilder.UseSqlite("Data Source=quest_rdm.db");
  }

  /// <summary>
  /// Configures the schema needed for the context before the model is locked down.
  /// </summary>
  /// <param name="modelBuilder"></param>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Configure the Project entity
    modelBuilder.Entity<Project>(entity =>
    {
      entity.HasKey(p => p.ID); // Set ID as the primary key
      entity.Property(p => p.Guid).IsRequired(); // Ensure Guid is required
      entity.Property(p => p.Title).HasMaxLength(255); // Set max length for Title
    });

    // Add additional configurations here if needed

  }
}