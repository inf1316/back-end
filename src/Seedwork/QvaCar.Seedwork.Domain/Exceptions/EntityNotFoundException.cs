namespace QvaCar.Seedwork.Domain
{

    public class EntityNotFoundException : DomainException
    {
        public EntityNotFoundException(string message) : base(message)
        { }
    }
}
