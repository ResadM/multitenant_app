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
            //Getting user from Http request
            var contextUser = context.User;
            //Check if user is authenticated
            if (contextUser.Identity!.IsAuthenticated)
            {
                //Get user name from claims
                var userName = contextUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

                //Check if user name is not null or empty
                if (!string.IsNullOrEmpty(userName))
                {
                    //Create scope to get database context
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        //Get database context
                        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContextAdmin>();
                        // Use dbContext here
                        var user = dbContext.Users.FirstOrDefault(x => x.UserName == userName);

                       if(user!=null)
                        {  
                            //You can check user role and based use role can return context

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
