using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOC.Constant;
using VOC.DataAccess;

namespace VOC.Common
{

    public class TokenExtensionAttributeDemo : AuthorizeAttribute, IAuthorizationFilter
    {
        CommonFunction common = new CommonFunction();
        //public IDistributedCache distributedCache { get; set; }

        public TokenExtensionAttributeDemo()
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
            string token = context.HttpContext.Request.Headers[VOCConstant.Authorization];
            if (!String.IsNullOrEmpty(token))
            {
                var acc = new VOCProcessDA("", Dis);
                try
                {
                    token = token.Length != 0 ? token.Replace(VOCConstant.BearerReplace, string.Empty) : string.Empty;
                    token = common.MD5Hash(token);
                    string str = acc.GetStringAsync(VOCConstant.ListLogoutToken).ToString();
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
