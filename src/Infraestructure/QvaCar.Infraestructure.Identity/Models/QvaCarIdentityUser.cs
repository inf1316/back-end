using Microsoft.AspNetCore.Identity;
using System;

namespace QvaCar.Infraestructure.Identity.Models
{
    public class QvaCarIdentityUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Age { get; set; }
        public int ProvinceId { get; set; }
        public int SubscriptionLevelId { get; set; }
        public QvaCarIdentityUserSubscriptionLevel SubscriptionLevel { get; set; }
    }
}
