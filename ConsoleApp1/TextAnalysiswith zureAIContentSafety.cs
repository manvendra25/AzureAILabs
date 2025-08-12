using Azure.AI.ContentSafety;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class TextAnalysiswith_zureAIContentSafety
    {
        public static async Task AnalyzeTextAsync()
        {
            string inputText = "This is a sample text that might be harmful.";
            string endpoint = "https://YourENDPOINTURL.cognitiveservices.azure.com/";
            string apiKey = "Your apiKey";
            var client = new ContentSafetyClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            var request = new AnalyzeTextOptions(inputText);
            var response = await client.AnalyzeTextAsync(request);

            Console.WriteLine("Text Analysis Results:");

            foreach (var category in response.Value.CategoriesAnalysis)
            {
                Console.WriteLine($"Category: {category.Category}");
                Console.WriteLine($"Severity: {category.Severity}");
                Console.WriteLine();
            }
        }
    }
}
