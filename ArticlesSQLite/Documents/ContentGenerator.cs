using System.Collections.Generic;
using System.Text;
using Telerik.Documents.Primitives;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace ArticlesSQLite.Documents
{
    public class ContentGenerator
    {
        ArticlesDbContext ArticlesDbContext { get; set; }
        public ContentGenerator(ArticlesDbContext articlesDbContext) {
            ArticlesDbContext = articlesDbContext;
        }
        private const string LoremIpsumText = @"Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat facer possim assum. Typi non habent claritatem insitam; est usus legentis in iis qui facit eorum claritatem. Investigationes demonstraverunt lectores legere me lius quod ii legunt saepius. Claritas est etiam processus dynamicus, qui sequitur mutationem consuetudium lectorum. Mirum est notare quam littera gothica, quam nunc putamus parum claram, anteposuerit litterarum formas humanitatis per seacula quarta decima et quinta decima. Eodem modo typi, qui nunc nobis videntur parum clari, fiant sollemnes in futurum. ";
        private const string GoldenSpiralApproximationText = @"A golden spiral can be approximated by first starting with a rectangle for which the ratio between its length and width is the golden ratio.";

        public static string GetParagraphText(int repeatCount)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < repeatCount; i++)
            {
                builder.Append(LoremIpsumText);
            }

            return builder.ToString();
        }

        public Table GetFamiliesTableAsync()
        {
            Table table = new();

            RgbColor headerColor = new RgbColor(79, 129, 189);
            RgbColor bordersColor = new RgbColor(149, 179, 215);
            RgbColor alternatingRowColor = new RgbColor(219, 229, 241);

            Border border = new Border(1, BorderStyle.Single, bordersColor);

            table.Borders = new TableBorders(border);
            table.DefaultCellProperties.Borders = new TableCellBorders(border, border, border, border);
            table.DefaultCellProperties.Padding = new Thickness(2);

            TableRow headerRow = table.Rows.AddTableRow();
            TableCell headerCell = headerRow.Cells.AddTableCell();
            headerCell.Borders = new TableCellBorders(new Border(BorderStyle.None));
            headerCell.ColumnSpan = 2;
            Block headerBlock = headerCell.Blocks.AddBlock();
            headerBlock.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;

            TableRow quartersRow = table.Rows.AddTableRow();
            TableCell codiCell = quartersRow.Cells.AddTableCell();
            codiCell.Background = headerColor;
            codiCell.Borders = new TableCellBorders(border, border, border, border, null, border);

            Block codiBlock = codiCell.Blocks.AddBlock();
            codiBlock.GraphicProperties.FillColor = RgbColors.White;
            codiBlock.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
            codiBlock.InsertText("CodiFamilia");

            TableCell descCell = quartersRow.Cells.AddTableCell();
            descCell.Background = headerColor;
            descCell.Borders = new TableCellBorders(border, border, border, border, null, border);

            Block descBlock = descCell.Blocks.AddBlock();
            descBlock.GraphicProperties.FillColor = RgbColors.White;
            descBlock.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
            descBlock.InsertText("Descripció");

            List<Familia> families = ArticlesDbContext.Families.ToList();
            for (int i = 0; i < families.Count; i++)
            {
                RgbColor rowColor = i % 2 == 0 ? alternatingRowColor : RgbColors.White;
                Familia familia = families[i];

                TableRow familiaRow = table.Rows.AddTableRow();

                TableCell codiCellAux = familiaRow.Cells.AddTableCell();
                codiCellAux.Background = rowColor;
                Block codiBlockAux = codiCellAux.Blocks.AddBlock();
                codiBlockAux.InsertText(familia.CodiFamilia);

                TableCell descCellAux = familiaRow.Cells.AddTableCell();
                descCellAux.Background = rowColor;
                Block descBlockAux = descCellAux.Blocks.AddBlock();
                descBlockAux.InsertText(familia.Descripcio);
            }

            return table;
        }

        public Table GetArticlesTableAsync(List<Article> articles)
        {
            Table table = new();

            RgbColor headerColor = new RgbColor(79, 129, 189);
            RgbColor bordersColor = new RgbColor(149, 179, 215);
            RgbColor alternatingRowColor = new RgbColor(219, 229, 241);

            Border border = new Border(1, BorderStyle.Single, bordersColor);

            table.Borders = new TableBorders(border);
            table.DefaultCellProperties.Borders = new TableCellBorders(border, border, border, border);
            table.DefaultCellProperties.Padding = new Thickness(2);

            TableRow headerRow = table.Rows.AddTableRow();
            TableCell headerCell = headerRow.Cells.AddTableCell();
            headerCell.Borders = new TableCellBorders(new Border(BorderStyle.None));
            headerCell.ColumnSpan = 6;

            Block headerBlock = headerCell.Blocks.AddBlock();
            headerBlock.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;

            TableRow quartersRow = table.Rows.AddTableRow();

            TableCell codiCell = quartersRow.Cells.AddTableCell();
            codiCell.Background = headerColor;
            codiCell.Borders = new TableCellBorders(border, border, border, border, null, border);

            Block codiBlock = codiCell.Blocks.AddBlock();
            codiBlock.GraphicProperties.FillColor = RgbColors.White;
            codiBlock.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
            codiBlock.InsertText("CodiArticle");

            TableCell descCell = quartersRow.Cells.AddTableCell();
            descCell.Background = headerColor;
            descCell.Borders = new TableCellBorders(border, border, border, border, null, border);

            Block descBlock = descCell.Blocks.AddBlock();
            descBlock.GraphicProperties.FillColor = RgbColors.White;
            descBlock.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
            descBlock.InsertText("Descripció");

            TableCell envasCell = quartersRow.Cells.AddTableCell();
            envasCell.Background = headerColor;
            envasCell.Borders = new TableCellBorders(border, border, border, border, null, border);

            Block envasBlock = envasCell.Blocks.AddBlock();
            envasBlock.GraphicProperties.FillColor = RgbColors.White;
            envasBlock.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
            envasBlock.InsertText("Envàs");

            TableCell pesCell = quartersRow.Cells.AddTableCell();
            pesCell.Background = headerColor;
            pesCell.Borders = new TableCellBorders(border, border, border, border, null, border);

            Block pesBlock = pesCell.Blocks.AddBlock();
            pesBlock.GraphicProperties.FillColor = RgbColors.White;
            pesBlock.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
            pesBlock.InsertText("Pes");

            TableCell preuvendaCell = quartersRow.Cells.AddTableCell();
            preuvendaCell.Background = headerColor;
            preuvendaCell.Borders = new TableCellBorders(border, border, border, border, null, border);

            Block preuvendaBlock = preuvendaCell.Blocks.AddBlock();
            preuvendaBlock.GraphicProperties.FillColor = RgbColors.White;
            preuvendaBlock.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
            preuvendaBlock.InsertText("Preu venda");

            //TableCell obsCell = quartersRow.Cells.AddTableCell();
            //obsCell.Background = headerColor;
            //obsCell.Borders = new TableCellBorders(border, border, border, border, null, border);

            //Block obsBlock = envasCell.Blocks.AddBlock();
            //obsBlock.GraphicProperties.FillColor = RgbColors.White;
            //obsBlock.HorizontalAlignment = Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center;
            //obsBlock.InsertText("Observacions");

            for (int i = 0; i < articles.Count; i++)
            {
                RgbColor rowColor = i % 2 == 0 ? alternatingRowColor : RgbColors.White;
                Article article = articles[i];

                TableRow familiaRow = table.Rows.AddTableRow();

                TableCell codiCellAux = familiaRow.Cells.AddTableCell();
                codiCellAux.Background = rowColor;
                Block codiBlockAux = codiCellAux.Blocks.AddBlock();
                codiBlockAux.InsertText(article.CodiArticle);

                TableCell descCellAux = familiaRow.Cells.AddTableCell();
                descCellAux.Background = rowColor;
                Block descBlockAux = descCellAux.Blocks.AddBlock();
                descBlockAux.InsertText(article.Descripcio);

                TableCell envasCellAux = familiaRow.Cells.AddTableCell();
                envasCellAux.Background = rowColor;
                Block envasBlockAux = envasCellAux.Blocks.AddBlock();
                envasBlockAux.InsertText(article.Envas.ToString());

                TableCell pesCellAux = familiaRow.Cells.AddTableCell();
                pesCellAux.Background = rowColor;
                Block pesBlockAux = pesCellAux.Blocks.AddBlock();
                pesBlockAux.InsertText(article.Pes.ToString());

                TableCell preuCellAux = familiaRow.Cells.AddTableCell();
                preuCellAux.Background = rowColor;
                Block preuBlockAux = preuCellAux.Blocks.AddBlock();
                preuBlockAux.InsertText(article.PreuVenda.ToString());

                //TableCell obsCellAux = familiaRow.Cells.AddTableCell();
                //obsCellAux.Background = rowColor;
                //Block obsBlockAux = descCellAux.Blocks.AddBlock();
                //obsBlockAux.InsertText(article.Observacions);
            }

            return table;
        }

        private static Stream GetResourceStream(string relativePath)
        {
            return File.OpenRead(relativePath);
        }
    }
}
