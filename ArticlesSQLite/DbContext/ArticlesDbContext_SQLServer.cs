using Microsoft.EntityFrameworkCore;

namespace ArticlesSQLite
{
    public class ArticlesDbContext_SQLServer : ArticlesDbContext
    {
        public ArticlesDbContext_SQLServer(IConfiguration _configuration) : base(_configuration) { }

        // Configurar aquest context per a utilitzar el servidor SQLite
        // https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/connection-strings
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(configuration.GetConnectionString("SQLServer"));
        }
    }
}
