using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;

namespace ArticlesSQLite
{
   // Aquesta classe conté el context (configuració de la base de dades) de l'aplicació
   public class ArticlesDbContext : DbContext
   {
      protected readonly IConfiguration configuration;

      // Entitats (taules) de la base de dades
      public DbSet<Article> Articles { get; set; }
      public DbSet<Familia> Families { get; set; }

		// Constructor de la classe
      public ArticlesDbContext(IConfiguration _configuration) {
         configuration = _configuration;
      }

      // Configurar aquest context per a utilitzar el servidor SQLite
      // https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/connection-strings
      protected override void OnConfiguring(DbContextOptionsBuilder options) {
         // Cal editar l'arxiu appsettings per configurar el nom del fitxer de la base de dades SQLite
         string fitxerSQLite = configuration.GetConnectionString("SQLiteFile");

         // Prepara la cadena de connexió per a SQLite
         // https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/connection-strings
         // Nota: Per aquest projecte, la base de dades es guardarà en una subcarpeta de l'aplicació,
         //       i per obtenir la ruta absoluta es pot fer servir Environment.CurrentDirectory
         var sqliteConnection = new SqliteConnectionStringBuilder() {
            DataSource = Path.Combine(Environment.CurrentDirectory, fitxerSQLite),
            Mode = SqliteOpenMode.ReadWriteCreate,
            ForeignKeys = true,
            RecursiveTriggers = false
         };

         // Configura l'Entity Framework per utilitzar la bd SQLite amb la cadena de connexió que hem creat
         options.UseSqlite(sqliteConnection.ToString());
      }

      // Aquest mètode permet aplicar la configuració addicional de la base de dades i de les taules
      protected override void OnModelCreating(ModelBuilder modelBuilder) {
         // Crida primer el mètode base
         base.OnModelCreating(modelBuilder);
			// Aplica la configuració addicional de cada taula
			modelBuilder.ApplyConfiguration(new Articles_Config());
			modelBuilder.ApplyConfiguration(new Familia_Config());
      }

   }

}
