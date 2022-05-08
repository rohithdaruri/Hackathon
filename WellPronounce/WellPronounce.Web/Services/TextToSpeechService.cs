using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellPronounce.Web.ApiModels;
using WellPronounce.Web.Entities;
using WellPronounce.Web.Interfaces;

namespace WellPronounce.Web.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly ITextToSpeechRepository _textToSpeechRepository;
        private static string YourSubscriptionKey = "1b3f59b543794337a9d5631a28af911a";
        private static string YourServiceRegion = "centralindia"; //centralindia  //Central India

        public TextToSpeechService(IBlobStorageService blobStorageService, ITextToSpeechRepository textToSpeechRepository)
        {
            _blobStorageService = blobStorageService;
            _textToSpeechRepository = textToSpeechRepository;
        }

        private async Task<byte[]> CallCognitiveService(StandardTextRequestModel standardTextRequestModel)
        {
            var speechConfig = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);
            // The language of the voice that speaks.
            byte[] blob;
            speechConfig.SpeechSynthesisVoiceName = "en-IN-NeerjaNeural"; //en-IN-NeerjaNeural //en-US-JennyNeural

            using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
            {
                string text = standardTextRequestModel.Text;
                var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
                blob = OutputSpeechSynthesisResult(speechSynthesisResult, standardTextRequestModel);
            }

            return blob;
        }

        private byte[] OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, StandardTextRequestModel standardTextRequestModel)
        {
            switch (speechSynthesisResult.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    var audiofile = speechSynthesisResult.AudioData;
                    return audiofile;
                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        // Log Error
                    }
                    return null;
                default:
                    return null;
            }
        }

        public async Task<StandardOutputModel> StandardProcessSaveTextToSpeechData(StandardTextRequestModel standardTextRequestModel)
        {
            try
            {
                var blobPath = "Sample";
                StandardOutputModel response;
                var blob = await CallCognitiveService(standardTextRequestModel);
                //blobPath = await _blobStorageService.UploadFileToBlob(standardTextRequestModel.Text, blob, "");
                response = await _textToSpeechRepository.StandardProcessSaveTextToSpeechData(blobPath, standardTextRequestModel);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<List<SpeechDetail>> GetRecords()
        {
            return await _textToSpeechRepository.GetRecords();
        }
    }
}
