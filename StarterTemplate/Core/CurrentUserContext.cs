using System;
using System.Web;
using StarterTemplate.Core.Data;
using StarterTemplate.Core.Domain;
using StarterTemplate.Core.Extensions;

namespace StarterTemplate.Core
{
    public class CurrentUserContext
    {
        private readonly IReadOnlyRepository _repository;

        public CurrentUserContext(IReadOnlyRepository repository)
        {
            _repository = repository;
            AuthenticateFromCurrentHttpContext();
        }

        private string AuthenticatedUsername { get; set; }
        
        public Member Member { get; set; }

        public string Username
        {
            get { return AuthenticatedUsername ?? "anonymous"; }
        }

        public bool IsAuthenticated
        {
            get { return AuthenticatedUsername.HasValue(); }
        }

        public void Authenticate(string username)
        {
            var member = _repository.First<Member>(m => m.Username == username);
            if (member == null)
                throw new ApplicationException("There is no member with the username \"{0}\".".FormatWith(username));

            AuthenticatedUsername = username;
            Member = member;
        }

        private void AuthenticateFromCurrentHttpContext()
        {
            var context = HttpContext.Current;
            var identity = context.User.Identity;

            if (!identity.IsAuthenticated)
                return;

            Authenticate(identity.Name);
        }
    }
}