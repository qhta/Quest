using Microsoft.EntityFrameworkCore;

namespace QuestRDM;

/// <summary>
/// Initializes the QuestRDM database with schema and seed data.
/// </summary>
public static class QuestRdmDbInitializer
{
  /// <summary>
  /// Initializes the database by applying pending migrations and seeding initial data if the database is empty.
  /// </summary>
  /// <remarks>This method ensures that the database schema is up to date by applying any pending migrations. 
  /// If the database contains no projects, it seeds the database with default data.</remarks>
  public static void Initialize()
  {
    using var db = new QuestRdmDbContext();

    // Ensure database and schema exist (applies pending migrations)
    db.Database.Migrate();

    // Seed initial data only if empty
    if (!db.Projects.Any())
    {
      db.Projects.AddRange(
        new Project { Guid = Guid.NewGuid(), Title = "Sample Project 1" }
      );

      db.SaveChanges();
    }
  }
}