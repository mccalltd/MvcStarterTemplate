using System;
using System.IO;
using DevOne.Security.Cryptography.BCrypt;
using StarterTemplate.Core.Data;
using StarterTemplate.Core.Domain;

namespace StarterTemplate.Core.Services
{
    public interface ISecurityService
    {
        Member ValidateLogin(string usernameOrEmailAddress, string password);
        Member SignUp(string username, string emailAddress, string password);
        string ResetPassword(string emailAddress);
        void ChangePassword(string newPassword);
    }

    public class SecurityService : ISecurityService
    {
        private readonly IRepository _repository;
        private readonly CurrentUserContext _currentUserContext;

        public SecurityService(
            IRepository repository, 
            CurrentUserContext currentUserContext
            )
        {
            _repository = repository;
            _currentUserContext = currentUserContext;
        }

        public Member ValidateLogin(string usernameOrEmailAddress, string password)
        {
            var member = _repository.First<Member>(m => m.Username == usernameOrEmailAddress || m.EmailAddress == usernameOrEmailAddress);
            if (member == null)
                throw new ApplicationException("The username or email address or password is incorrect.");

            if (!BCryptHelper.CheckPassword(password, member.PasswordHash))
                throw new ApplicationException("The username or email address or password is incorrect.");

            member.LastLoginDate = DateTime.Now;
            
            _repository.SaveChanges();

            return member;
        }

        public Member SignUp(string username, string emailAddress, string password)
        {
            var member = new Member
            {
                Username = username,
                EmailAddress = emailAddress,
                PasswordHash = HashPassword(password),
                LastLoginDate = DateTime.Now
            };

            _repository.Add(member);
            _repository.SaveChanges();

            return member;
        }

        public string ResetPassword(string emailAddress)
        {
            var member = _repository.First<Member>(m => m.EmailAddress == emailAddress);
            if (member == null)
                throw new ApplicationException("That email address is not registered.");

            var newPassword = Path.GetRandomFileName().Replace(".", "");
            member.PasswordHash = HashPassword(newPassword);
            member.MustChangePasswordOnNextLogin = true;

            _repository.SaveChanges();

            return newPassword;
        }

        public void ChangePassword(string newPassword)
        {
            var member = _repository.First<Member>(m => m.EmailAddress == _currentUserContext.Username);
            if (member == null)
                throw new ApplicationException("No member info could be found for this account.");

            member.PasswordHash = HashPassword(newPassword);
            member.MustChangePasswordOnNextLogin = false;

            _repository.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            return BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt(12));
        }
    }
}
