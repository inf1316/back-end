using Microsoft.EntityFrameworkCore;
using QvaCar.Domain.Chat;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Chat
{
    public class QvaCarChatDbContext : DbContext
    {
        public DbSet<ChatUser> Users { get; set; }

        public QvaCarChatDbContext(DbContextOptions<QvaCarChatDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AddEntitiesConfiguration(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private static void AddEntitiesConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChatUserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChannelEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChatMessageEntityTypeConfiguration());
        }

        public async Task ClearDatabaseBeforeTestAsync()
        {
            Users.ToList().ForEach(x => Users.Remove(x));
            await SaveChangesAsync(default);
        }
    }
}
