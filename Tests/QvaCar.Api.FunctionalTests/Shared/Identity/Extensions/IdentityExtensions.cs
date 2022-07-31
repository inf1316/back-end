using System;
using System.Dynamic;
using System.Text;

namespace QvaCar.Api.FunctionalTests.Shared.Identity
{
    public static class IdentityExtensions
    {
        public static string ConvertToAccessToken(this TestApiUser user)
        {
            dynamic data = new ExpandoObject();
            data.sub = user.Id;
            data.subscription_level = user.SubscriptionLevel.Name;
            var token = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            string tokenEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
            return tokenEncoded;
        }
    }
}
