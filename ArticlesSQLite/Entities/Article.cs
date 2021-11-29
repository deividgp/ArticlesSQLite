using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticlesSQLite
{
   public class Article
   {
		#region Camps
		[MaxLength(10)]
		[Required] // NOT NULL
		public string CodiArticle { get; set; }

		[MaxLength(2)]
		public string CodiFamilia { get; set; }

		[MaxLength(50)]
		[Required] // NOT NULL
		public string Descripcio { get; set; }

		[Required] // NOT NULL
		public int Envas { get; set; }

		[Required] // NOT NULL
		public decimal Pes { get; set; }

		public byte? CodiIVA { get; set; }

		public DateTime? DataAlta { get; set; }

		public DateTime? DataUltSortida { get; set; }

		[Required] // NOT NULL
		public bool NoFacturar { get; set; }

		[Required] // NOT NULL
		public bool NoSortirInventaris { get; set; }

		[Required] // NOT NULL
		public decimal StockMinim { get; set; }

		[Required] // NOT NULL
		public decimal StockRuptura { get; set; }

		[Required] // NOT NULL
		public bool ControlStock { get; set; }

		[Required] // NOT NULL
		public decimal StockPendRebre { get; set; }

		public DateTime? DataPendRebre { get; set; }

		[Required] // NOT NULL
		public decimal Comisio { get; set; }

		public DateTime? DataCost { get; set; }

		[Required] // NOT NULL
		public decimal PreuCostMig { get; set; }

		[Required] // NOT NULL
		public decimal StockCost { get; set; }

		[Required] // NOT NULL
		public decimal PreuCostUltim { get; set; }

		[Required] // NOT NULL
		public decimal PreuVenda { get; set; }

		[Required] // NOT NULL
		public string Observacions { get; set; }
		#endregion

		// * Les relacions están explicades al capítol "8 Configuring relationships"
		//   del llibre "Entity Framework Core in Action, 2nd Edition"
		#region Relacions (Claus Forànies)
		[ForeignKey("CodiFamilia")]
		[InverseProperty("FK_Articles_Families")] // La propietat de la classe Familia té el mateix nom
		public Familia FK_Articles_Families { get; set; }
		#endregion

		#region Propietats de Navegació (Claus Forànies Inverses)
		// No n'hi ha en aquesta entitat
		#endregion

		#region Constructors
		// Constructor per Defecte
		public Article() { }
		#endregion
   }

	public partial class Articles_Config : IEntityTypeConfiguration<Article>
	{
		public Articles_Config() { }

		public void Configure(EntityTypeBuilder<Article> entity) {
			#region Configuració dels Camps
			// Configuració amb FluentAPI dels camps que ho necessitin
			entity.Property(e => e.Pes).HasPrecision(10,2);
			entity.Property(e => e.StockMinim).HasPrecision(10,2);
			entity.Property(e => e.StockRuptura).HasPrecision(10,2);
			entity.Property(e => e.StockPendRebre).HasPrecision(10,2);
			entity.Property(e => e.Comisio).HasPrecision(4,2);
			entity.Property(e => e.PreuCostMig).HasPrecision(13,5);
			entity.Property(e => e.StockCost).HasPrecision(13,5);
			entity.Property(e => e.PreuCostUltim).HasPrecision(13,5);
			entity.Property(e => e.PreuVenda).HasPrecision(10,3);
			#endregion

			#region Configuració de les Claus i Indexos de la taula
			// Claus i Indexos de la taula:
			entity.HasKey(e => e.CodiArticle).HasName("PK_Articles") ;
			entity.HasIndex(e => new { e.CodiFamilia, e.CodiArticle }).HasDatabaseName("K_Article_CodiFamilia_CodiArticle");
			entity.HasIndex(e => e.Descripcio).HasDatabaseName("K_Article_Descripcio");
			entity.HasIndex(e => e.CodiIVA).HasDatabaseName("K_Article_FK_TipusIVA");
			entity.HasIndex(e => e.CodiFamilia).HasDatabaseName("K_Article_FK_Families");
			#endregion
		}
	}

}
