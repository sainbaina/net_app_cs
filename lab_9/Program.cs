using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;


namespace Lab9
{
    class COMFormatter
    {
        static void Main(string[] args)
        {
            Console.WriteLine("a");
        }

        public COMFormatter(string template = @"C:\Users\zharn\Desktop\Lab_9_doc.doc")
        {
            wordapp.Visible = true;

            Object newTemplate = false;
            Object documentType = word.WdNewDocumentType.wdNewBlankDocument;
            Object newTemplate = false;

            wordapp.Documents.Add(template, newTemplate, ref documentType, ref visible);

            worddocuments = wordapp.Documents;
            worddocument = worddocuments.get_Item(1);
            worddocument.Activate();
        }
    }
}
