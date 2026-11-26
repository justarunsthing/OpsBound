using Microsoft.EntityFrameworkCore;

namespace OpsBound.Persistence;

public sealed class OpsBoundDbContext(DbContextOptions<OpsBoundDbContext> options) : DbContext(options)
{
    public const int EmbeddingDimensions = 1536;

    public DbSet<Conversation> Conversations => Set<Conversation>();

    public DbSet<Message> Messages => Set<Message>();

    public DbSet<ToolInvocation> ToolInvocations => Set<ToolInvocation>();

    public DbSet<Approval> Approvals => Set<Approval>();

    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    public DbSet<ProcessedEvent> ProcessedEvents => Set<ProcessedEvent>();

    public DbSet<ProjectedStock> ProjectedStock => Set<ProjectedStock>();

    public DbSet<OpsException> Exceptions => Set<OpsException>();

    public DbSet<Document> Documents => Set<Document>();

    public DbSet<Chunk> Chunks => Set<Chunk>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.Property(c => c.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(m => m.Role).HasMaxLength(32);
            entity.HasIndex(m => m.ConversationId);
            entity.HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ToolInvocation>(entity =>
        {
            entity.Property(t => t.Name).HasMaxLength(128);
            entity.Property(t => t.ResultHash).HasMaxLength(128);
            entity.HasIndex(t => t.ConversationId);
        });

        modelBuilder.Entity<Approval>(entity =>
        {
            entity.Property(a => a.Type).HasMaxLength(128);
            entity.Property(a => a.Status).HasMaxLength(32);
            entity.Property(a => a.DecidedBy).HasMaxLength(200);
            entity.HasIndex(a => a.Status);
        });

        modelBuilder.Entity<AuditEvent>(entity =>
        {
            entity.Property(a => a.Kind).HasMaxLength(128);
            entity.HasIndex(a => a.ConversationId);
            entity.HasIndex(a => a.ApprovalId);
        });

        modelBuilder.Entity<ProcessedEvent>(entity =>
        {
            entity.HasKey(e => e.EventId);
            entity.Property(e => e.EventId).HasMaxLength(128);
            entity.Property(e => e.Topic).HasMaxLength(128);
        });

        modelBuilder.Entity<ProjectedStock>(entity =>
        {
            entity.HasKey(s => new { s.StoreId, s.Sku });
            entity.Property(s => s.StoreId).HasMaxLength(64);
            entity.Property(s => s.Sku).HasMaxLength(64);
            entity.Property(s => s.LastEventId).HasMaxLength(128);
        });

        modelBuilder.Entity<OpsException>(entity =>
        {
            entity.ToTable("Exceptions");
            entity.Property(e => e.Type).HasMaxLength(128);
            entity.Property(e => e.Sku).HasMaxLength(64);
            entity.Property(e => e.CustomerId).HasMaxLength(64);
            entity.Property(e => e.Status).HasMaxLength(32);
            entity.Property(e => e.SourceEventId).HasMaxLength(128);
            entity.HasIndex(e => e.Status);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.Property(d => d.FileName).HasMaxLength(512);
            entity.Property(d => d.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Chunk>(entity =>
        {
            entity.Property(c => c.Embedding).HasColumnType($"vector({EmbeddingDimensions})");
            entity.HasIndex(c => new { c.DocumentId, c.Ordinal }).IsUnique();
            entity.HasOne(c => c.Document)
                .WithMany(d => d.Chunks)
                .HasForeignKey(c => c.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
