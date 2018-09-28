using Microsoft.EntityFrameworkCore;

namespace Occurrence
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions options) : base(options) { }

        public DbSet<SerializedEventEntity> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<SerializedEventEntity>()
                .HasIndex(i => new { i.Stream, i.EventNumber })
                .IsUnique();

            base.OnModelCreating(model);
        }
    }
}
