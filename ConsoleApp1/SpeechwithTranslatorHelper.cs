using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SpeechwithTranslatorHelper
    {
        private readonly string speechKey;
        private readonly string speechRegion;
        private readonly string translatorKey;
        private readonly string translatorRegion;

        public SpeechwithTranslatorHelper(string speechKey, string speechRegion, string translatorKey, string translatorRegion)
        {
            this.speechKey = speechKey;
            this.speechRegion = speechRegion;
            this.translatorKey = translatorKey;
            this.translatorRegion = translatorRegion;
        }

        public async Task RunSpeechWorkflowAsync()
        {
            // 🎙 Step 1: Speech to Text
            string text = await SpeechToTextAsync();
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("No speech detected.");
                return;
            }
            Console.WriteLine($"Original: {text}");

            // 🌍 Step 2: Translate and Speak in multiple languages
            string[] targetLanguages = { "hi", "fr", "es" };
            foreach (var lang in targetLanguages)
            {
                string translated = await TranslateTextAsync(text, lang);
                Console.WriteLine($"[{lang}] {translated}");

                await TextToSpeechAsync(translated, lang);
            }
        }

        private async Task<string> SpeechToTextAsync()
        {
            var config = SpeechConfig.FromSubscription(speechKey, speechRegion);
             var recognizer = new SpeechRecognizer(config);

            Console.WriteLine("Speak into your microphone...");
            var result = await recognizer.RecognizeOnceAsync();

            return result.Reason == ResultReason.RecognizedSpeech ? result.Text : string.Empty;
        }

        private async Task<string> TranslateTextAsync(string text, string targetLang)
        {
            string endpoint = "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0";
            string route = $"&to={targetLang}";

             var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", translatorKey);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", translatorRegion);

            var body = new object[] { new { Text = text } };
            var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(body);

            var response = await client.PostAsync(endpoint + route, new StringContent(requestBody, Encoding.UTF8, "application/json"));
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var result = JArray.Parse(jsonResponse);
            return result[0]["translations"][0]["text"].ToString();
        }

        private async Task TextToSpeechAsync(string text, string languageCode)
        {
            var config = SpeechConfig.FromSubscription(speechKey, speechRegion);

            string voiceName;
            switch (languageCode)
            {
                case "hi":
                    voiceName = "hi-IN-SwaraNeural";
                    break;
                case "fr":
                    voiceName = "fr-FR-DeniseNeural";
                    break;
                case "es":
                    voiceName = "es-ES-ElviraNeural";
                    break;
                default:
                    voiceName = "en-US-JennyNeural";
                    break;
            }
            config.SpeechSynthesisVoiceName = voiceName;

             var synthesizer = new SpeechSynthesizer(config);
            var result = await synthesizer.SpeakTextAsync(text);

            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                Console.WriteLine($"[Speech Output in {languageCode}] Done");
            else
                Console.WriteLine("Speech synthesis failed.");
        }
    }
}
