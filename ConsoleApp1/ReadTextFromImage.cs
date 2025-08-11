using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace ConsoleApp1
{
    internal class ReadTextFromImage
    {
        public async Task ReadTextFromImageAsync()
        {
            // Set your Azure endpoint and API key
            string endpoint = "ENDPOINT URL";
            string apiKey = "apiKey";

            // Build the path to the image in the Data folder
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "sample-image.png");

            if (!File.Exists(imagePath))
            {
                Console.WriteLine($"Image not found at: {imagePath}");
                return;
            }

            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

             FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);

            // Use the prebuilt OCR model
            AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "prebuilt-read",
                stream
            );

            var result = operation.Value;

            Console.WriteLine("=== OCR Extracted Text ===");
            foreach (var page in result.Pages)
            {
                foreach (var line in page.Lines)
                {
                    Console.WriteLine(line.Content);
                }
            }
        }
    }
}
