using Microsoft.EntityFrameworkCore;
using QvaCar.Domain.Chat;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Chat.Repositories
{
    internal class ChatUserRepository : IChatUserRepository
    {
        private readonly QvaCarChatDbContext _dbContext;

        public ChatUserRepository(QvaCarChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ChatUser aggregateRoot, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(aggregateRoot, cancellationToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ChatUser?> GetByIdOrDefaultAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext
                .Users
                .Include(u => u.Channels)
                .ThenInclude(u => u.Messages)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task UpdateAsync(ChatUser aggregateRoot, CancellationToken cancellationToken)
        {
            _dbContext.Update(aggregateRoot);
            await _dbContext.SaveChangesAsync();
        }
    }
}
