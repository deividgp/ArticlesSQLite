using Telerik.Windows.Documents.Common.FormatProviders;
using pdfProviderNamespace = Telerik.Windows.Documents.Flow.FormatProviders.Pdf;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf;
using Telerik.Windows.Documents.Flow.FormatProviders.Txt;
using Telerik.Windows.Documents.Flow.Model;
using Microsoft.JSInterop;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Editing;
using System.Collections;
using System.Collections.ObjectModel;
using Telerik.Windows.Documents.Flow.TextSearch;

namespace ArticlesSQLite.Data
{
    public class FileConverter
    {
        private IWebHostEnvironment Environment { get; set; }
        private IJSRuntime _js { get; set; }
        private ArticlesDbContext articlesDbContext { get; set; }
        private string[] entitats = {"Familia", "Article" };
        private string[] campsFamilia = {"CodiFamilia", "Descripcio"};
        private string[] campsArticle = { };

        // DI for the environment feature we need - path to the wwwroot folder to read the intial content,
        // and JS runtime for downloading the file to the browser
        public FileConverter(IWebHostEnvironment env, IJSRuntime js, ArticlesDbContext articlesDbContext)
        {
            Environment = env;
            _js = js;
            this.articlesDbContext = articlesDbContext;
        }

        /// <summary>
        /// Import a file from disk and convert it to an HTML string for use in the Editor
        /// </summary>
        /// <returns>An HTML string that is just the contents of the body tag so the editor can work with them. Returns null to denote an error.</returns>
        public string GetHtmlString()
        {
            try
            {
                // read the file - in this sample a hardcoded path
                string path = Path.Combine(Environment.WebRootPath, "JohnGrisham.docx");
                RadFlowDocument document = ReadFile(path);

                // convert the file to HTML
                HtmlFormatProvider provider = new HtmlFormatProvider();
                provider.ExportSettings.StylesExportMode = StylesExportMode.Inline;
                provider.ExportSettings.DocumentExportLevel = DocumentExportLevel.Fragment;
                string html = provider.Export(document);

                // get only the <body> contents
                int bodyStart = html.IndexOf("<body>") + "<body>".Length;
                int bodyEnd = html.IndexOf("</body>");
                string body = html.Substring(bodyStart, bodyEnd - bodyStart);

                return body;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Convert the HTML string to a file, download the generated file in the browser
        /// </summary>
        /// <param name="htmlContent">The HTML content to export</param>
        /// <param name="fileName">The FileName of the downloaded file, its extension is used to fetch the exprot provider</param>
        /// <returns>Returns true if the operation succeeded, false if there was an exception</returns>
        public async Task<bool> ExportAndDownloadHtmlContent(string htmlContent, string fileName)
        {
            try
            {
                // prepare a document with the HTML content that we can use for conversion
                HtmlFormatProvider provider = new HtmlFormatProvider();
                RadFlowDocument document = provider.Import(htmlContent);
                
                // get the provider to export and then download the file
                string mimeType;
                IFormatProvider<RadFlowDocument> exportProvider = GetExportFormatProvider(fileName, out mimeType);
                byte[] exportFileBytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    exportProvider.Export(document, ms);
                    exportFileBytes = ms.ToArray();
                }

                // download the file in the browser
                await FileDownloader.Save(_js, exportFileBytes, mimeType, fileName);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
            return true;
        }

        public async Task MailMerge(string htmlContent)
        {
            try
            {
                
                // prepare a document with the HTML content that we can use for conversion
                HtmlFormatProvider provider = new HtmlFormatProvider();
                RadFlowDocument document = provider.Import(htmlContent);
                RadFlowDocumentEditor editor = new RadFlowDocumentEditor(document);

                foreach (string entitat in entitats)
                {
                    if (entitat == "Familia")
                    {
                        foreach (string campFamilia in campsFamilia)
                        {
                            string match = "[" + entitat + ":" + campFamilia + "]";
                            if (editor.FindAll(match).Count > 0)
                            {
                                FieldInfo field = new(document);
                                List<InlineBase> fieldInlines = new();
                                fieldInlines.Add(field.Start);
                                fieldInlines.Add(new Run(document) { Text = "MERGEFIELD " + campFamilia});
                                fieldInlines.Add(field.Separator);
                                fieldInlines.Add(new Run(document) { Text = "<<"+ campFamilia + ">>" });
                                fieldInlines.Add(field.End);

                                editor.ReplaceText(match, fieldInlines);
                            }
                        }
                    }
                }

                RadFlowDocument mergedDoc = document.MailMerge(getMailMergeData());
                IFormatProvider<RadFlowDocument>  fileFormatProvider = new pdfProviderNamespace.PdfFormatProvider();
                using (MemoryStream ms = new MemoryStream())
                {
                    fileFormatProvider.Export(mergedDoc, ms);
                    await FileDownloader.Save(_js, ms.ToArray(), "application/pdf", "hghgt.pdf");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public async Task ExportMailMerge(string htmlContent)
        {
            try
            {
                List<string> readFields = new List<string>();
                // prepare a document with the HTML content that we can use for conversion
                HtmlFormatProvider provider = new HtmlFormatProvider();
                RadFlowDocument template = provider.Import(htmlContent);
                RadFlowDocument document = template.Clone();
                RadFlowDocumentEditor editor = new RadFlowDocumentEditor(document);

                foreach (string entitat in entitats)
                {
                    if (entitat == "Familia")
                    {
                        foreach (string campFamilia in campsFamilia)
                        {
                            string match = "[" + entitat + ":" + campFamilia + "]";
                            if (editor.FindAll(match).Count > 0)
                            {
                                readFields.Add(entitat + ":" + campFamilia);
                            }
                        }
                    }
                    else
                    {
                        foreach (string campArticle in campsArticle)
                        {
                            string match = "[" + entitat + ":" + campArticle + "]";
                            if (editor.FindAll(match).Count > 0)
                            {
                                readFields.Add(entitat + ":" + campArticle);
                            }
                        }
                    }
                }

                List<Familia> families = getAllData();

                for (int i = 0; i < families.Count; i++)
                {
                    foreach (string readField in readFields)
                    {
                        string[] fields = readField.Split(":");
                        if (i == 0)
                        {
                            editor.ReplaceText("[" + readField + "]", SelectFieldFamilia(families[i], fields[1]));
                        }
                        else
                        {
                            editor.InsertText(SelectFieldFamilia(families[i], fields[1]) + " ");
                        }
                    }
                    editor.InsertParagraph();
                }

                IFormatProvider<RadFlowDocument> fileFormatProvider = new pdfProviderNamespace.PdfFormatProvider();
                using (MemoryStream ms = new MemoryStream())
                {
                    fileFormatProvider.Export(document, ms);
                    await FileDownloader.Save(_js, ms.ToArray(), "application/pdf", "hghgt.pdf");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public IEnumerable getMailMergeData()
        {
            List<Familia> families = new();
            //families.Add(articlesDbContext.Families.First());
            families = articlesDbContext.Families.ToList();
            return families;
        }

        public List<Familia> getAllData()
        {
            List<Familia> families = articlesDbContext.Families.ToList();
            return families;
        }

        public string SelectFieldFamilia(Familia familia, string field)
        {
            switch (field)
            {
                case "CodiFamilia":
                    return familia.CodiFamilia;
                case "Descripcio":
                    return familia.Descripcio;
            }
            return null;
        }

        /// <summary>
        /// Reads the file from the disk as an in-memory document, based on the provided path
        /// </summary>
        /// <param name="path">Path to the file on disk you wish to import</param>
        /// <returns>RadFlowDocument that you can use to convert the contents to other formats</returns>
        RadFlowDocument ReadFile(string path)
        {
            IFormatProvider<RadFlowDocument> fileFormatProvider = GetImportFormatProvider(Path.GetExtension(path));

            RadFlowDocument document;

            using (FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                document = fileFormatProvider.Import(input);
            }

            return document;
        }

        /// <summary>
        /// Get Supported providers for the import operation based on the file extension
        /// </summary>
        /// <param name="extension">the extension of the file we will import, used to discern the provider type</param>
        /// <returns>IFormatProvider<RadFlowDocument> that you can use to read that file and import it as an in-memory document you can convert to other formats</returns>
        IFormatProvider<RadFlowDocument> GetImportFormatProvider(string extension)
        {
            IFormatProvider<RadFlowDocument> fileFormatProvider;
            switch (extension)
            {
                case ".docx":
                    fileFormatProvider = new DocxFormatProvider();
                    break;
                case ".rtf":
                    fileFormatProvider = new RtfFormatProvider();
                    break;
                case ".html":
                    fileFormatProvider = new HtmlFormatProvider();
                    break;
                case ".txt":
                    fileFormatProvider = new TxtFormatProvider();
                    break;
                default:
                    fileFormatProvider = null;
                    break;
            }

            if (fileFormatProvider == null)
            {
                throw new NotSupportedException("The chosen file cannot be read with the supported providers.");
            }

            return fileFormatProvider;
        }

        /// <summary>
        /// Get Supported providers for the export operation based on the file name, also gets its MIME type as an out parameter
        /// </summary>
        /// <param name="fileName">the file name you wish to export, the provider is discerned based on its extensiom</param>
        /// <param name="mimeType">an out parameter with the MIME type for this file so you can download it</param>
        /// <returns>IFormatProvider<RadFlowDocument> that you can use to export the original document to a certain file format</returns>
        IFormatProvider<RadFlowDocument> GetExportFormatProvider(string fileName, out string mimeType)
        {
            // we get both the provider and the MIME type to use only one swtich-case
            IFormatProvider<RadFlowDocument> fileFormatProvider;
            string extension = Path.GetExtension(fileName);
            switch (extension)
            {
                case ".docx":
                    fileFormatProvider = new DocxFormatProvider();
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".rtf":
                    fileFormatProvider = new RtfFormatProvider();
                    mimeType = "application/rtf";
                    break;
                case ".html":
                    fileFormatProvider = new HtmlFormatProvider();
                    mimeType = "text/html";
                    break;
                case ".txt":
                    fileFormatProvider = new TxtFormatProvider();
                    mimeType = "text/plain";
                    break;
                case ".pdf":
                    fileFormatProvider = new pdfProviderNamespace.PdfFormatProvider();
                    mimeType = "application/pdf";
                    break;
                default:
                    fileFormatProvider = null;
                    mimeType = string.Empty;
                    break;
            }

            if (fileFormatProvider == null)
            {
                throw new NotSupportedException("The chosen format cannot be exported with the supported providers.");
            }

            return fileFormatProvider;
        }
    }
}