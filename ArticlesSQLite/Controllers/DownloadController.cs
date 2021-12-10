using Microsoft.AspNetCore.Mvc;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.Model;
using ArticlesSQLite.Documents;

namespace ArticlesSQLite.Controllers
{
    public class DownloadController : Controller
    {
        private readonly ArticlesDbContext ArticlesDbContext;
        DocumentGenerator documentGenerator;

        public DownloadController(ArticlesDbContext context)
        {
            ArticlesDbContext = context;
        }

        // Downloads the file
        [HttpGet("download/{tipus}")]
        public FileContentResult DownloadFile(string tipus)
        {
            documentGenerator = new(ArticlesDbContext);
            string pathName = Path.GetTempPath() + tipus + ".pdf";
            RadFixedDocument document = null;
            if (tipus == "families")
            {
                document = documentGenerator.GenerateFamiliesDocumentAsync(true);
            }
            else if (tipus == "articles")
            {
                document = documentGenerator.GenerateArticlesDocumentAsync(true);
            }

            PdfFormatProvider provider = new();
            FileContentResult fileContent = new(provider.Export(document), "application/octet-stream");
            fileContent.FileDownloadName = tipus + ".pdf";
            return fileContent;
        }

        // Stores then downloads the file
        public IActionResult StoreDownloadFile(string tipus)
        {
            documentGenerator = new(ArticlesDbContext);
            string pathName = Path.GetTempPath() + tipus + ".pdf";
            RadFixedDocument document = null;
            if (tipus == "families")
            {
                document = documentGenerator.GenerateFamiliesDocumentAsync(true);
            }
            else if (tipus == "articles")
            {
                document = documentGenerator.GenerateArticlesDocumentAsync(true);
            }

            if (System.IO.File.Exists(pathName))
            {
                try
                {
                    System.IO.File.Delete(pathName);
                }
                catch (UnauthorizedAccessException)
                {
                    FileAttributes attributes = System.IO.File.GetAttributes(pathName);
                    if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        attributes &= ~FileAttributes.ReadOnly;
                        System.IO.File.SetAttributes(pathName, attributes);
                        System.IO.File.Delete(pathName);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            PdfFormatProvider provider = new();
            FileStream fs = new(pathName, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose);
            fs.Write(provider.Export(document));
            fs.Position = 0;

            return File(fs, "application/force-download", tipus + ".pdf");
        }
    }
}