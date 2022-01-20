using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JustBlog_Final.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var userName = Convert.ToString(httpContext.User.Identity.Name);
            if (!string.IsNullOrEmpty(userName))
                using (var context = new ApplicationDbContext())
                {
                    var userRole = (from u in context.Users
                                    from ur in u.Roles
                                    join r in context.Roles on ur.RoleId equals r.Id
                                    where u.UserName == userName
                                    select new
                                    {
                                        r.Name
                                    }).FirstOrDefault();
                    foreach (var role in allowedroles)
                    {
                        if (role == userRole.Name) return true;
                    }
                }


            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Posts" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}