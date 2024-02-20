using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserContext.Core.Interface;

namespace UserContext.Infrastructure.WebAPI
{
    public class WebAPIUserContext : IUserContext
    {
        //To be implemented when user has been setup (user is important ofcourse :))
        public string? UserName => "static-user";
    }
}
