using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombinedPdfsByOptional
{
    class Program
    {
        static IEnumerable<string> pdfFiles = new[]    { "C:\\Users\\user\\Desktop\\New Folder\\1.pdf", "C:\\Users\\user\\Desktop\\New Folder\\2.pdf",
                                                       "C:\\Users\\user\\Desktop\\New Folder\\3.pdf","C:\\Users\\user\\Desktop\\New Folder\\4.pdf",
                                                       "C:\\Users\\user\\Desktop\\New Folder\\5.pdf","C:\\Users\\user\\Desktop\\New Folder\\6.pdf"};

        static List<string> combinedPdfs = new List<string>();
        static string combinedPdsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CombinedPdfs");
        static string MSG_INPUT_OPTION = "Please enter combining option : ...";
        static string MSG_SUCCESS_COMBINING = "Combining has done successfully...";
        static int selection;
        static void Main(string[] args)
        {
            CreateDirectory(combinedPdsFolder);

            Console.WriteLine(MSG_INPUT_OPTION);
            selection = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"Selection is : {selection}");

            GetCombinePdfs(selection);
            Console.WriteLine(MSG_SUCCESS_COMBINING);
            Console.ReadLine();
        }
        public static void GetCombinePdfs(int selection)
        {
            int counter = 0;
            string outputFolder = combinedPdsFolder;
            string outputPDFFilePath = string.Empty;
            List<IEnumerable<string>> combinedIndexes = new List<IEnumerable<string>>();

            MoveToOptionalList(combinedIndexes);

            for (int j = 0; j < combinedIndexes.Count; j++)
            {
                counter++;
                PdfSharp.Pdf.PdfDocument output = new PdfSharp.Pdf.PdfDocument();
                foreach (var item in combinedIndexes[j])
                {
                    PdfSharp.Pdf.PdfDocument input = PdfReader.Open(item, PdfDocumentOpenMode.Import);
                    output.Version = input.Version;
                    foreach (PdfSharp.Pdf.PdfPage page in input.Pages)
                    {
                        output.AddPage(page);
                    }
                    outputPDFFilePath = Path.Combine(outputFolder, counter.ToString() + "_combined" + ".pdf");
                    output.Save(outputPDFFilePath);
                }
                combinedPdfs.Add(outputPDFFilePath);
            }
        }
        public static void CreateDirectory(string path)
        {
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static void MoveToOptionalList(List<IEnumerable<string>> combinedIndexes)
        {
            for (int i = 0; i < pdfFiles.Count(); i += selection)
            {
                combinedIndexes.Add(pdfFiles.Skip(i).Take(selection));
            }
        }
    }
}
