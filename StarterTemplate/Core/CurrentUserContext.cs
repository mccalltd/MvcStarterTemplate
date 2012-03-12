using System.Web;

namespace StarterTemplate.Core
{
    public class CurrentUserContext
    {
        public string Username
        {
            get { return AuthenticatedUsername ?? "anonymous"; }
        }

        public bool IsAuthenticated { get; set; }
        
        private string AuthenticatedUsername { get; set; }

        public static CurrentUserContext FromCurrentHttpContext()
        {
            var context = HttpContext.Current;
            var identity = context.User.Identity;

            if (!identity.IsAuthenticated)
                return new CurrentUserContext();

            return new CurrentUserContext
            {
                AuthenticatedUsername = identity.Name,
                IsAuthenticated = true,
            };
        }
    }
}