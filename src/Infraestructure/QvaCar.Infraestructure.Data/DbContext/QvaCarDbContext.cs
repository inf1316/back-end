using MediatR;
using Microsoft.EntityFrameworkCore;
using QvaCar.Domain.CarAds;
using QvaCar.Seedwork.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Data
{
    public class QvaCarDbContext : DbContext
    {
        private readonly IMediator _mediator;

        public DbSet<CarAd> Ads { get; set; }

#nullable disable
        public QvaCarDbContext(DbContextOptions<QvaCarDbContext> options, IMediator mediator) : base(options)
        {
            this._mediator = mediator;
        }
#nullable enable

        public async Task ClearDatabaseBeforeTestAsync()
        {
            Ads.ToList().ForEach(x => Ads.Remove(x));
            await SaveChangesAsync(default);          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AddEntitiesConfiguration(modelBuilder);
        }

        private static void AddEntitiesConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CarAdEntityTypeConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await DispatchDomainEventsForTrackedEntities().ConfigureAwait(false);

            return result;
        }
        public override int SaveChanges()
        {
            return SaveChangesAsync(default).GetAwaiter().GetResult();
        }

        private async Task DispatchDomainEventsForTrackedEntities()
        {
            if (_mediator == null)
                throw new NullReferenceException("Mediator is required");

            var entitiesWithEvents = ChangeTracker.Entries<AggregateRoot<Guid>>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToArray();
                entity.ClearEvents();
                foreach (var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent).ConfigureAwait(false);
                }
            }
        }
    }
}
