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
