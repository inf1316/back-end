namespace QvaCar.Infraestructure.Identity.Models
{
    public class QvaCarIdentityUserSubscriptionLevel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        protected QvaCarIdentityUserSubscriptionLevel() { }
        public QvaCarIdentityUserSubscriptionLevel(int id, string name)
        {
            Id = id;
            Name = name;
        }

    }
}
