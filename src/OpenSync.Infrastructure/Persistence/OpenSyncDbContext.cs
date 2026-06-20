using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenSync.Core.Entities;
using OpenSync.Core.ValueObjects;
using OpenSync.Infrastructure.Persistence.Interceptors;

namespace OpenSync.Infrastructure.Persistence;

public class JsonDataConverter : ValueConverter<JsonData, string>
{
    public JsonDataConverter()
        : base(
            v => v.Raw,
            v => new JsonData(v, null))
    {
    }
}

public class OpenSyncDbContext : DbContext
{
    public DbSet<SyncService> SyncServices => Set<SyncService>();
    public DbSet<SyncDocument> SyncDocuments => Set<SyncDocument>();
    public DbSet<SyncList> SyncLists => Set<SyncList>();
    public DbSet<SyncListItem> SyncListItems => Set<SyncListItem>();
    public DbSet<SyncMap> SyncMaps => Set<SyncMap>();
    public DbSet<SyncMapItem> SyncMapItems => Set<SyncMapItem>();
    public DbSet<SyncStream> SyncStreams => Set<SyncStream>();
    public DbSet<SyncChannel> SyncChannels => Set<SyncChannel>();
    public DbSet<ChannelMember> ChannelMembers => Set<ChannelMember>();

    private readonly AuditableEntityInterceptor _auditInterceptor;
    private readonly DomainEventInterceptor _domainEventInterceptor;

    public OpenSyncDbContext(
        DbContextOptions<OpenSyncDbContext> options,
        AuditableEntityInterceptor auditInterceptor,
        DomainEventInterceptor domainEventInterceptor)
        : base(options)
    {
        _auditInterceptor = auditInterceptor;
        _domainEventInterceptor = domainEventInterceptor;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<JsonData>().HaveConversion<JsonDataConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OpenSyncDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor, _domainEventInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}
