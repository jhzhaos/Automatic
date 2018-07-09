using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomaticRelease.Controllers
{
    public class AccountAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext authorizationContext)
        {
            var httpContext = authorizationContext.HttpContext;
            var request = httpContext.Request;

            ActionResult actionResult = null;
            string message = string.Empty;
            var user = GetCurrentUser();
            if (user == "")
            {
                String url = request.RawUrl;
                UrlHelper urlHelper = new UrlHelper(request.RequestContext);
                //利用Action 指定的操作名称、控制器名称和路由值生成操作方法的完全限定 URL。
                string returnUrl = urlHelper.Action("LoginIndex", "Account", new { returnUrl = "", message = message });
                actionResult = new RedirectResult(returnUrl);
            }
            authorizationContext.Result = actionResult;
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUser()
        {
            var session = HttpContext.Current.Session["LoginUser"];
            if (session == null)
                return "";
            return session as string;
        }
    }
}