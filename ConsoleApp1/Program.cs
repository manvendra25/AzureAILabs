using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.AI.TextAnalytics;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Azure.AI.ContentSafety;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.IO;
using Azure.AI.FormRecognizer.DocumentAnalysis;
namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {     //  TextAnalyticsClassFile Start 
              // Name of your key vault
              //var keyVaultName = "manavkeyvault";

            //// Variables for retrieving the key and endpoint from your key vault
            //// Set these variables to the names you created for your secrets
            //const string keySecretName = "CognitiveServicesKey";
            //const string endpointSecretName = "CognitiveServicesEndpoint";

            //// Endpoint for accessing your key vault
            //var kvUri = $"https://{keyVaultName}.vault.azure.net/";

            //var keyVaultClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            //Console.WriteLine($"Retrieving your secrets from {keyVaultName}.");

            //// Key and endpoint secrets retrieved from your key vault
            //var keySecret = await keyVaultClient.GetSecretAsync(keySecretName);
            //var endpointSecret = await keyVaultClient.GetSecretAsync(endpointSecretName);

            //Console.WriteLine($"Your key secret value is: {keySecret.Value.Value}");
            //Console.WriteLine($"Your endpoint secret value is: {endpointSecret.Value.Value}");
            //Console.WriteLine("Secrets retrieved successfully");

            //call TextAnalyticsClassFile method 
            //TextAnalyticsClassFile.EntityRecognitionExample(keySecret.Value.Value, endpointSecret.Value.Value);
            //  ---------------------TextAnalyticsClassFile END ---------------------
            //  
            //// ---------------------AnalyzeImageAsync start---------------------
            //int imgNum = 1;
            //if (args.Length > 0 && int.TryParse(args[0], out int inputNum) && inputNum >= 1 && inputNum <= 3)
            //{
            //    imgNum = inputNum;
            //}

            //string prediction = await AnalyzeImageAsync(2);

            //Console.WriteLine($"\nTop Prediction: {prediction}");

            ////------------AnalyzeImageAsync  END---------------------

            //// Call the separate method
            //await AnalyzeTextSafety();


            //// call document Intillgance 

            //await ExtractTextFromPdf();

            // call document Intillgance 
            ReadTextFromImage readTextFromImage = new ReadTextFromImage();
            await readTextFromImage.ReadTextFromImageAsync();
            Console.ReadLine();
        }
        static async Task ExtractTextFromPdf()
        {
            // PDF file in project folder: ProjectFolder/Data/sample.pdf
            string localPdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "sample-local-pdf.pdf");

            string endpoint = "endpointURL";
            string apiKey = "Document AI KEY";

            if (!File.Exists(localPdfPath))
            {
                Console.WriteLine($"File not found: {localPdfPath}");
                return;
            }

            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

            var stream = File.OpenRead(localPdfPath);

            var operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-read", stream);

            Console.WriteLine("=== Extracted Text from PDF ===");
            foreach (var page in operation.Value.Pages)
            {
                Console.WriteLine($"Page {page.PageNumber}:");
                foreach (var line in page.Lines)
                {
                    Console.WriteLine(line.Content);
                }
                Console.WriteLine();
            }
        }
        // Separate method for text analysis
        private static async Task AnalyzeTextSafety()
        {
            string endpoint = "";
            string apiKey = "";

            // Text to analyze
            string inputText = "I want to hurt myself";

            var client = new ContentSafetyClient(new Uri(endpoint), new AzureKeyCredential(apiKey));

            var request = new AnalyzeTextOptions(inputText);

            // Call Azure AI Content Safety
            Response<AnalyzeTextResult> response = await client.AnalyzeTextAsync(request);

            Console.WriteLine("=== Content Safety Analysis Result ===");

            foreach (var category in response.Value.CategoriesAnalysis)
            {
                Console.WriteLine($"Category: {category.Category}");
                Console.WriteLine($"Severity: {category.Severity}");
               
                Console.WriteLine("--------------------------------------");
            }

           
        }
        private static async Task<string> AnalyzeImageAsync(int imgNum)
        {
            string predictionUrl = "<EndpointURL>/customvision/v3.0/Prediction/<projectId>/classify/iterations/<publishedIterationName>/url";
            string predictionKey = "YourKEy";

            string imgUrl = $"https://raw.githubusercontent.com/MicrosoftLearning/AI-900-AIFundamentals/main/data/vision/animals/animal-{imgNum}.jpg";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

                var body = $"{{\"url\":\"{imgUrl}\"}}";
                var content = new StringContent(body, Encoding.UTF8, "application/json");

                Console.WriteLine("Analyzing image...");

                HttpResponseMessage response = await client.PostAsync(predictionUrl, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Parse JSON and get the first prediction tagName
                JObject result = JObject.Parse(jsonResponse);
                string tagName = result["predictions"]?[0]?["tagName"]?.ToString();

                return tagName ?? "No prediction found";
            }
        }
    }
}