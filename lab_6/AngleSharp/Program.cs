using System;
using System.IO;
using System.Text;
using AngleSharp;
using AngleSharp.Dom;
using HtmlAgilityPack;
using App;

namespace LAB_6
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string url = "https://2droida.ru/collection/smartfony";

            List<string> links = await Parser.GetLinks(url);
            
            foreach (var link in links)
            {
                await Parser.GetDocument(link);
            }
        }

    }

    class Parser
    {
        public static async Task GetDocument(string url)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                
                IConfiguration config = Configuration.Default.WithDefaultLoader();

                IBrowsingContext context = BrowsingContext.New(config);

                IDocument doc = await context.OpenAsync(url);

                Smartphone smartphone = new Smartphone();

                IElement name = doc.QuerySelector("h1.product__title.heading");
                smartphone.Name = name?.TextContent;

                IElement price = doc.QuerySelector("div.property:nth-child(2) > div:nth-child(3)"); //("span.product__price-cur");
                smartphone.Price = price?.TextContent;

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

                Console.WriteLine(smartphone.GetInfo());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public static async Task<List<string>> GetLinks(string url)
        {
            Console.WriteLine("Начанием загрузку главной страницы");
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument doc = await context.OpenAsync(url);

            Console.WriteLine("Начанием считывание главной страницы");
            IEnumerable<IElement> aElements = doc.All.Where(block =>
                block.LocalName == "a"
                && block.ParentElement.LocalName == "div"
                && block.ParentElement.ClassList.Contains("product-preview__title"));
            
            List<string> output = new List<string>();
            foreach (IElement a in aElements.ToList()) output.Add($"https://2droida.ru{a.GetAttribute("href")}");
            Console.WriteLine("Считывание главной страницы завершено");

            return output;
        }
    }
}

