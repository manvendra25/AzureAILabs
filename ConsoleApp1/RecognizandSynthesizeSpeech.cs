using Azure.AI.ContentSafety;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace ConsoleApp1
{
    public class RecognizandSynthesizeSpeech
    {
        // 🎙 Recognize Speech from Microphone
       public static async Task RecognizeSpeechAsync()
        {
            string speechKey = "YOUR APP KEY";
            string serviceRegion = "eastus"; // e.g., "eastus"
            var config = SpeechConfig.FromSubscription(speechKey, serviceRegion);
             var recognizer = new SpeechRecognizer(config);

            Console.WriteLine("Speak into your microphone...");
            var result = await recognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                Console.WriteLine($"Recognized: {result.Text}");
            }
            else
            {
                Console.WriteLine($"Speech could not be recognized. Reason: {result.Reason}");
            }
        }

        // 🗣 Synthesize Speech from Text
       public static async Task SynthesizeSpeechAsync()
        {
            string speechKey = "YOUR APP KEY";
            string serviceRegion = "eastus"; // e.g., "eastus"
            var config = SpeechConfig.FromSubscription(speechKey, serviceRegion);
             var synthesizer = new SpeechSynthesizer(config);

            var result = await synthesizer.SpeakTextAsync("Hello! Welcome to Azure AI Speech service.");

            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                Console.WriteLine("Speech synthesized to speaker successfully.");
            }
            else
            {
                Console.WriteLine($"Speech synthesis failed. Reason: {result.Reason}");
            }
        }
    }
}
