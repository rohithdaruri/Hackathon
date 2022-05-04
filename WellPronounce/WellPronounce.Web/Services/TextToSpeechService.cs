using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellPronounce.Web.ApiModels;
using WellPronounce.Web.Interfaces;

namespace WellPronounce.Web.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly IBlobStorageService _blobStorageService;
        private static string YourSubscriptionKey = "1b3f59b543794337a9d5631a28af911a";
        private static string YourServiceRegion = "centralindia"; //centralindia  //Central India

        public TextToSpeechService(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }


        public async Task<string> CallCognitiveService(TextRequestModel textRequestModel)
        {
            var speechConfig = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);
            string blobPath = String.Empty;
            // The language of the voice that speaks.
            speechConfig.SpeechSynthesisVoiceName = "en-IN-NeerjaNeural"; //en-IN-NeerjaNeural //en-US-JennyNeural

            using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
            {
                string text = textRequestModel.Text;
                var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
                blobPath = await OutputSpeechSynthesisResult(speechSynthesisResult, text);
            }


            return blobPath;
        }


        private async Task<string> OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
        {
            string blobPath = String.Empty;
            switch (speechSynthesisResult.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    var audiofile = speechSynthesisResult.AudioData;
                    blobPath = await _blobStorageService.UploadFileToBlob(text, audiofile, "");
                    break;
                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        // Log Error
                    }
                    break;
                default:
                    break;
            }
            return blobPath;
        }

    }
}
