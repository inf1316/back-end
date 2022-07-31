using QvaCar.Seedwork.Domain;

namespace QvaCar.Domain.Identity
{
    public class UserSubscriptionLevel : Enumeration
    {
        public static readonly UserSubscriptionLevel Free = new(1, "Free");
        public static readonly UserSubscriptionLevel Paid = new(2, "Paid");
        public UserSubscriptionLevel(int id, string name) : base(id, name) { }
    }
}