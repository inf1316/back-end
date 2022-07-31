namespace QvaCar.Application.Common.Secutity
{
    public class AnySubscribedUserAttribute : AuthorizeAttribute
    {
        public AnySubscribedUserAttribute() : base() { Policy = "AnySubscribedUser"; }
    }
}
