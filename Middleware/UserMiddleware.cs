using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using multitenant_app.Context;
using multitenant_app.Models;
using System.Security.Claims;

namespace multitenant_app.Middleware
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UserMiddleware(RequestDelegate next, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task Invoke(HttpContext context)
        {
            var contextUser = context.User;
            if(contextUser.Identity!.IsAuthenticated)
            {
                var userName= contextUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

                if (!string.IsNullOrEmpty(userName))
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContextAdmin>();
                        // Use dbContext here
                        var user = dbContext.Users.FirstOrDefault(x => x.UserName == userName);

                       if(user!=null)
                        {  //You can check user role and based use role can return context

                            //Get connection string and replace database name
                            string connectionString = _configuration.GetConnectionString("DataBaseContextUser").Replace("{DatabaseName}", user.DbName);

                            //Add connection string to context
                            context.Items["UserConnectionString"] = connectionString;
                        }                       
                    }
                }
            }
            await _next(context);
        }
    }
}
