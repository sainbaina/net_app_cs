using System;
using System.IO;
using System.Text;
using AngleSharp;
using AngleSharp.Dom;
using App;

namespace LAB_6
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string url = "https://2droida.ru/collection/smartfony";

            List<string> links = await Parser.GetLinks(url);
            
            try
            {
                Console.WriteLine(links[0].ToString());
            }
            catch
            {
                Console.WriteLine("Can`t open web-site");
            }
            

            int i = 1;
            foreach (var link in links)
            {
                await Parser.GetDocument(link, i);
                i++;
            }
        }
    }

    class Parser
    {
        public static async Task GetDocument(string url, int id)
        {
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                IConfiguration config = Configuration.Default.WithDefaultLoader();

                IBrowsingContext context = BrowsingContext.New(config);

                IDocument doc = await context.OpenAsync(url);

                Smartphone smartphone = new Smartphone();

                IElement name = doc.QuerySelector("h1.product__title.heading");
                smartphone.Name = name?.TextContent;

                IElement processor = doc.QuerySelector("div.property:nth-child(2) > div:nth-child(3)"); //("span.product__price-cur");
                smartphone.Processor = processor?.TextContent;

                IEnumerable<IElement> shortDescriptionParams = doc.All
                    .Where(block => block.LocalName == "div"
                        && block.ClassList.Contains("property")
                        && block.ParentElement.LocalName == "div"
                        && block.ParentElement.ClassList.Contains("properties-items"));

                foreach (var propertyElement in shortDescriptionParams)
                {
                    string propertyName = propertyElement.QuerySelector(".property__name")?.TextContent;
                    string propertyContent = propertyElement.QuerySelector(".property__content")?.TextContent;

                    smartphone.dict.Add(propertyName, propertyContent);
                }

                smartphone.smartphoneId = id;

                try
                {
                    using (var db_context = new lab_3_database_Context())
                    {
                        // db_context.Smartphones.Add(smartphone);

                        Smartphone smartphone1 = db_context.Smartphones.Find(id);
                        
                        //db_context.Smartphones.Remove(smartphone1);

                        Console.WriteLine(smartphone1.Processor);

                        // db_context.SaveChanges();
                    } 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch
            {
                Console.WriteLine("Can't get document");
            }
        }


        public static async Task<List<string>> GetLinks(string url)
        {
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument doc = await context.OpenAsync(url);

            IEnumerable<IElement> aElements = doc.All.Where(block =>
                block.LocalName == "a"
                && block.ParentElement.LocalName == "div"
                && block.ParentElement.ClassList.Contains("product-preview__title"));
            
            List<string> output = new List<string>();
            foreach (IElement a in aElements.ToList()) output.Add($"https://2droida.ru{a.GetAttribute("href")}");

            return output;
        }
        
    }
}

