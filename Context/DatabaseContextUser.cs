using Microsoft.EntityFrameworkCore;
using multitenant_app.Models.UserModels;

namespace multitenant_app.Context
{
    public class DatabaseContextUser : DbContext
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private readonly string? _connectionString;
        private readonly IConfiguration? _configuration;

        //For Dymanic Tenant Connection
        public DatabaseContextUser(DbContextOptions<DatabaseContextUser> options, IHttpContextAccessor? httpContextAccessor, IConfiguration? configuration) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        //For Manual Migration (create and update database)
        public DatabaseContextUser(DbContextOptions<DatabaseContextUser> options, string? connectionString) : base(options)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Using when add manual migration

            //string manulConnectionString = _configuration.GetConnectionString("DataBaseContextDefault"); ;
            //if (!string.IsNullOrEmpty(manulConnectionString))
            //{
            //    optionsBuilder.UseSqlServer(manulConnectionString);
            //    return;
            //}

            //Using when creating and updating user database on register and login
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseSqlServer(_connectionString);
                return;
            }

            //Check if HttpContext is available
            if (_httpContextAccessor?.HttpContext?.Items["UserConnectionString"] == null)
            {
                //Can write custom exception
                throw new InvalidOperationException("HttpContext is not available. Ensure the application is correctly configured.");
            }

            //Get connection string from Http request
            var connectionString = _httpContextAccessor.HttpContext.Items["UserConnectionString"] as string;
            if (string.IsNullOrEmpty(connectionString))
            {
                //Can write custom exception
                throw new InvalidOperationException("User database connection string not found in HttpContext.");
            }

            //Set connection string
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Products> Products { get; set; }

    }
}
