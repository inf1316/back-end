using IdGen;
using QvaCar.Domain.Chat;

namespace QvaCar.Infraestructure.Chat.Services
{
    public class MessageSecuenceIdGenerator : IMessageSecuenceIdGenerator
    {
        private readonly IdGenerator _generator;

        public MessageSecuenceIdGenerator(IdGenerator generator) => _generator = generator;

        public long GetNextId() => _generator.CreateId();
    }
}
