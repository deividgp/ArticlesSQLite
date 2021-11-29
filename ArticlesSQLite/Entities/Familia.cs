using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticlesSQLite
{
   public partial class Familia
	{
		#region Camps
		[MaxLength(2)]
		[Required] // NOT NULL
		public string CodiFamilia { get; set; }

		[MaxLength(30)]
		[Required] // NOT NULL
		public string Descripcio { get; set; }
		#endregion

		// * Les relacions están explicades al capítol "8 Configuring relationships"
		//   del llibre "Entity Framework Core in Action, 2nd Edition"
		#region Relacions (Claus Forànies)
		// No n'hi ha en aquesta entitat
		#endregion

		#region Propietats de Navegació (Claus Forànies Inverses)
		[InverseProperty("FK_Articles_Families")] // La propietat de la classe Article té el mateix nom
		public virtual ObservableCollection<Article> FK_Articles_Families { get; } = new ObservableCollection<Article>();
		#endregion

		#region Constructors
		// Constructor per Defecte
		public Familia() { }
		#endregion
	}

	public partial class Familia_Config : IEntityTypeConfiguration<Familia>
	{
		public Familia_Config() { }

		public void Configure(EntityTypeBuilder<Familia> entity) {
			#region Configuració de les Claus i Indexos de la taula
			// Claus i Indexos de la taula:
			entity.HasKey(e => e.CodiFamilia).HasName("PK_Familia");
			entity.HasIndex(e => e.Descripcio).HasDatabaseName("K_Familia_Descripcio");
			#endregion
		}
	}
}
