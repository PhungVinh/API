using AccountManagement.Common;
using AccountManagement.Constant;
using AccountManagement.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;
using System.Security.Claims;

namespace AccountManagement.ViewModels
{
    public class TokenExtensionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        //public IDistributedCache distributedCache { get; set; }
        AccountCommon accountCommon = new AccountCommon();

        public TokenExtensionAttribute()
        {
           
        }
        
        /// <summary>
        /// custom authorize trong .Net core api phải implement AuthorizeAttribute và IAuthorizationFilter
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var services = context.HttpContext.RequestServices;
            var Dis = (IDistributedCache)services.GetService(typeof(IDistributedCache));
            // Tìm trong mảng Filter xem có AllowAnonymous không ?
            var check = context.Filters.Any(x => x is IAllowAnonymousFilter);
            if (check)
            {
                return;
            }
            // Get token
            string token = context.HttpContext.Request.Headers[AccountConstant.Authorization];
            if (!String.IsNullOrEmpty(token))
            {
                var acc = new AccountDA(Dis, null, null);
                try
                {
                    token = token.Length != 0 ? token.Replace(AccountConstant.BearerReplace, string.Empty) : string.Empty;
                    token = accountCommon.MD5Hash(token);
                    string str = acc.GetStringCache(AccountConstant.ListLogoutToken);
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (str.Contains(token))
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

        }
    }
}
