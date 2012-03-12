using System.Web.Mvc;
using StarterTemplate.Core;
using StructureMap;

namespace StarterTemplate.Configuration
{
    public abstract class ApplicationWebViewPage : ApplicationWebViewPage<dynamic> { }

    public abstract class ApplicationWebViewPage<TModel> : WebViewPage<TModel>
    {
        public CurrentUserContext CurrentUserContext
        {
            get { return ObjectFactory.GetInstance<CurrentUserContext>(); }
        }
    }
}