namespace HerisTemplate.Infrastructure.Common;

public class HerisTemplateDbContext : BaseDbContext
{
    public HerisTemplateDbContext(DbContextOptions<HerisTemplateDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}