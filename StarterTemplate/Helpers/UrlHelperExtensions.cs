using System.Web.Mvc;

namespace StarterTemplate.Helpers
{
    public static class UrlHelperExtensions
    {
        public static string ToHome(this UrlHelper helper)
        {
            return helper.Action("Index", "Home", new { area = "" });
        }

        public static string ToAbout(this UrlHelper helper)
        {
            return helper.Action("About", "Home", new { area = "" });
        }

        public static string ToContact(this UrlHelper helper)
        {
            return helper.Action("Contact", "Home", new { area = "" });
        }

        public static string ToLogIn(this UrlHelper helper)
        {
            return helper.Action("LogIn", "Auth", new { area = "" });
        }

        public static string ToSignUp(this UrlHelper helper)
        {
            return helper.Action("SignUp", "Auth", new { area = "" });
        }

        public static string ToLogOut(this UrlHelper helper)
        {
            return helper.Action("LogOut", "Auth", new { area = "" });
        }
    }
}