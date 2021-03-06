// <auto-generated />
using System;
using ArticlesSQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ArticlesSQLite.Migrations
{
    [DbContext(typeof(ArticlesDbContext_SQLServer))]
    [Migration("20211221152344_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ArticlesSQLite.Article", b =>
                {
                    b.Property<string>("CodiArticle")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("CodiFamilia")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<byte?>("CodiIVA")
                        .HasColumnType("tinyint");

                    b.Property<decimal>("Comisio")
                        .HasPrecision(4, 2)
                        .HasColumnType("decimal(4,2)");

                    b.Property<bool>("ControlStock")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DataAlta")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataCost")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataPendRebre")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataUltSortida")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descripcio")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Envas")
                        .HasColumnType("int");

                    b.Property<bool>("NoFacturar")
                        .HasColumnType("bit");

                    b.Property<bool>("NoSortirInventaris")
                        .HasColumnType("bit");

                    b.Property<string>("Observacions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Pes")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("PreuCostMig")
                        .HasPrecision(13, 5)
                        .HasColumnType("decimal(13,5)");

                    b.Property<decimal>("PreuCostUltim")
                        .HasPrecision(13, 5)
                        .HasColumnType("decimal(13,5)");

                    b.Property<decimal>("PreuVenda")
                        .HasPrecision(10, 3)
                        .HasColumnType("decimal(10,3)");

                    b.Property<decimal>("StockCost")
                        .HasPrecision(13, 5)
                        .HasColumnType("decimal(13,5)");

                    b.Property<decimal>("StockMinim")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("StockPendRebre")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("StockRuptura")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("CodiArticle")
                        .HasName("PK_Articles");

                    b.HasIndex("CodiFamilia")
                        .HasDatabaseName("K_Article_FK_Families");

                    b.HasIndex("CodiIVA")
                        .HasDatabaseName("K_Article_FK_TipusIVA");

                    b.HasIndex("Descripcio")
                        .HasDatabaseName("K_Article_Descripcio");

                    b.HasIndex("CodiFamilia", "CodiArticle")
                        .HasDatabaseName("K_Article_CodiFamilia_CodiArticle");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("ArticlesSQLite.Familia", b =>
                {
                    b.Property<string>("CodiFamilia")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("Descripcio")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("CodiFamilia")
                        .HasName("PK_Familia");

                    b.HasIndex("Descripcio")
                        .HasDatabaseName("K_Familia_Descripcio");

                    b.ToTable("Families");
                });

            modelBuilder.Entity("ArticlesSQLite.Article", b =>
                {
                    b.HasOne("ArticlesSQLite.Familia", "FK_Articles_Families")
                        .WithMany("FK_Articles_Families")
                        .HasForeignKey("CodiFamilia");

                    b.Navigation("FK_Articles_Families");
                });

            modelBuilder.Entity("ArticlesSQLite.Familia", b =>
                {
                    b.Navigation("FK_Articles_Families");
                });
#pragma warning restore 612, 618
        }
    }
}
