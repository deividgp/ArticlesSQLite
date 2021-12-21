using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ArticlesSQLite
{
    public class ArticlesDbContext_SQLite : ArticlesDbContext
    {
        public ArticlesDbContext_SQLite(IConfiguration _configuration) : base(_configuration) { }

        // Configurar aquest context per a utilitzar el servidor SQLite
        // https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/connection-strings
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Cal editar l'arxiu appsettings per configurar el nom del fitxer de la base de dades SQLite
            string fitxerSQLite = configuration.GetConnectionString("SQLite");

            // Prepara la cadena de connexió per a SQLite
            // https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/connection-strings
            // Nota: Per aquest projecte, la base de dades es guardarà en una subcarpeta de l'aplicació,
            //       i per obtenir la ruta absoluta es pot fer servir Environment.CurrentDirectory
            var sqliteConnection = new SqliteConnectionStringBuilder()
            {
                DataSource = Path.Combine(Environment.CurrentDirectory, fitxerSQLite),
                Mode = SqliteOpenMode.ReadWriteCreate,
                ForeignKeys = true,
                RecursiveTriggers = false
            };

            // Configura l'Entity Framework per utilitzar la bd SQLite amb la cadena de connexió que hem creat
            options.UseSqlite(sqliteConnection.ToString());
        }
    }
}
