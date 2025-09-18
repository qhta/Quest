namespace Quest.Data.QDM
{
  public class QuestQDMDbContext : DbContext
  {
    public DbSet<ProjectQuality> ProjectQualities { get; set; }
    public DbSet<DocumentQuality> DocumentQualities { get; set; }
    public DbSet<QualityNode> QualityNodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=quest.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);


    }
  }
}