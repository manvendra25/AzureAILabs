using Azure.AI.TextAnalytics;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class TextAnalyticsClients
    {
        private static void EntityRecognitionExample(string keySecret, string endpointSecret)
        {
            // String to be sent for Named Entity Recognition
            var exampleString = "I had a wonderful trip to Seattle last week.";

            // Create credentials and client
            AzureKeyCredential azureKeyCredential = new AzureKeyCredential(keySecret);
            Uri endpoint = new Uri(endpointSecret);
            var client = new TextAnalyticsClient(endpoint, azureKeyCredential);


            string text = "Satya Nadella is the CEO of Microsoft, living in Redmond. My email is satya@example.com. I love Azure Cognitive Services!";

            // 1️⃣ Language Detection
            var languageResult = client.DetectLanguage(text);
            Console.WriteLine($"Language: {languageResult.Value.Name}");

            // 2️⃣ Sentiment Analysis
            var sentimentResult = client.AnalyzeSentiment(text);
            Console.WriteLine($"Sentiment: {sentimentResult.Value.Sentiment}");
            Console.WriteLine($"Positive: {sentimentResult.Value.ConfidenceScores.Positive:0.00}, " +
                              $"Neutral: {sentimentResult.Value.ConfidenceScores.Neutral:0.00}, " +
                              $"Negative: {sentimentResult.Value.ConfidenceScores.Negative:0.00}");

            // 3️⃣ Key Phrase Extraction
            var keyPhrasesResult = client.ExtractKeyPhrases(text);
            Console.WriteLine("Key Phrases:");
            foreach (var phrase in keyPhrasesResult.Value)
            {
                Console.WriteLine($"  - {phrase}");
            }

            // 4️⃣ Named Entity Recognition
            var entityResult = client.RecognizeEntities(text);
            Console.WriteLine("Entities:");
            foreach (var entity in entityResult.Value)
            {
                Console.WriteLine($"  - {entity.Text} ({entity.Category})");
            }

            // 5️⃣ PII Detection
            var piiResult = client.RecognizePiiEntities(text);
            Console.WriteLine("PII Entities:");
            foreach (var pii in piiResult.Value)
            {
                Console.WriteLine($"  - {pii.Text} ({pii.Category})");
            }

            // 6️⃣ Linked Entity Recognition
            var linkedEntitiesResult = client.RecognizeLinkedEntities(text);
            Console.WriteLine("Linked Entities:");
            foreach (var linkedEntity in linkedEntitiesResult.Value)
            {
                Console.WriteLine($"  - {linkedEntity.Name} (URL: {linkedEntity.Url})");
            }
        }
    }
}
