using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Threading.Tasks;
using WellPronounce.Web.ApiModels;
using WellPronounce.Web.Entities;
using WellPronounce.Web.Interfaces;
using System.Speech.Synthesis;
using Microsoft.Extensions.Logging;

namespace WellPronounce.Web.Services
{
    public class TextToSpeechService : ITextToSpeechService
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly ITextToSpeechRepository _textToSpeechRepository;
        private readonly ILogger<TextToSpeechService> _logger;
        private static string YourSubscriptionKey = "1b3f59b543794337a9d5631a28af911a";
        private static string YourServiceRegion = "centralindia"; //centralindia  //Central India

        public TextToSpeechService(IBlobStorageService blobStorageService, ITextToSpeechRepository textToSpeechRepository, ILogger<TextToSpeechService> logger)
        {
            _blobStorageService = blobStorageService;
            _textToSpeechRepository = textToSpeechRepository;
            _logger = logger;
        }

        private async Task<byte[]> CallCognitiveService(StandardTextRequestModel standardTextRequestModel)
        {
            try
            {
                var speechConfig = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);
                // The language of the voice that speaks.
                byte[] blob;
                speechConfig.SpeechSynthesisVoiceName = "en-IN-NeerjaNeural"; //en-IN-NeerjaNeural //en-US-JennyNeural

                using (var speechSynthesizer = new Microsoft.CognitiveServices.Speech.SpeechSynthesizer(speechConfig))
                {
                    string text = standardTextRequestModel.PreferedName == "" ? standardTextRequestModel.LegalFirstName + " " + standardTextRequestModel.LegalLastName : standardTextRequestModel.PreferedName;
                    var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
                    blob = OutputSpeechSynthesisResult(speechSynthesisResult, standardTextRequestModel);
                }

                return blob;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }           
        }

        private byte[] OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, StandardTextRequestModel standardTextRequestModel)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }           
        }

        public async Task<StandardOutputModel> StandardProcessSaveTextToSpeechData(StandardTextRequestModel standardTextRequestModel)
        {
            try
            {
                if (standardTextRequestModel.LegalFirstName != "" && standardTextRequestModel.LegalLastName != "")
                {
                    var blobPath = new AzureBlobPathModel();
                    StandardOutputModel response;
                    var blob = await CallCognitiveService(standardTextRequestModel);
                    //var phonetics = CreatePhonetics(standardTextRequestModel);
                    var phonetics = CreatePhonetics(standardTextRequestModel);
                    var existing = await _textToSpeechRepository.GetDetailByName(standardTextRequestModel);
                    if (existing == null)
                    {
                        var text = standardTextRequestModel.PreferedName == "" ? standardTextRequestModel.LegalFirstName + " " + standardTextRequestModel.LegalLastName : standardTextRequestModel.PreferedName;
                        blobPath = await _blobStorageService.UploadFileToBlob(text, blob, "");
                        response = await _textToSpeechRepository.StandardProcessSaveTextToSpeechData(blobPath.BlobPath, standardTextRequestModel, phonetics);
                        return response;
                    }
                    else
                    {
                        return existing;
                    }
                }
                else
                {
                    return new StandardOutputModel();
                }            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
            
        }

        public async Task<List<SpeechDetail>> GetRecords()
        {
            return await _textToSpeechRepository.GetRecords();
        }

        #region Phonetics

        public static string recoPhonemes;

        private string CreatePhonetics(StandardTextRequestModel standardTextRequestModel)
        {
            try
            {
                string text = standardTextRequestModel.PreferedName == "" ? standardTextRequestModel.LegalFirstName + " " + standardTextRequestModel.LegalLastName : standardTextRequestModel.PreferedName;
                string MyPronunciation = GetPronunciationFromText(text.Trim()); // Get IPA pronunciations of MyTe
                return MyPronunciation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
           
        }

        private  string GetPronunciationFromText(string MyWord)
        {
            //this is a trick to figure out phonemes used by synthesis engine

            //txt to wav
            try
            {
                using (MemoryStream audioStream = new MemoryStream())
                {
                    using (System.Speech.Synthesis.SpeechSynthesizer synth = new System.Speech.Synthesis.SpeechSynthesizer())
                    {
                        synth.SetOutputToWaveStream(audioStream);
                        PromptBuilder pb = new PromptBuilder();
                        //pb.AppendBreak(PromptBreak.ExtraSmall); //'e' wont be recognized if this is large, or non-existent?
                        //synth.Speak(pb);
                        synth.Speak(MyWord);
                        //synth.Speak(pb);
                        synth.SetOutputToNull();
                        audioStream.Position = 0;

                        //now wav to txt (for reco phonemes)
                        recoPhonemes = String.Empty;
                        GrammarBuilder gb = new GrammarBuilder(MyWord);
                        System.Speech.Recognition.Grammar g = new System.Speech.Recognition.Grammar(gb); //TODO the hard letters to recognize are 'g' and 'e'
                        SpeechRecognitionEngine reco = new SpeechRecognitionEngine();
                        reco.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(reco_SpeechHypothesized);
                        reco.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(reco_SpeechRecognitionRejected);
                        reco.UnloadAllGrammars(); //only use the one word grammar
                        reco.LoadGrammar(g);
                        reco.SetInputToWaveStream(audioStream);
                        System.Speech.Recognition.RecognitionResult rr = reco.Recognize();
                        reco.SetInputToNull();
                        if (rr != null)
                        {
                            recoPhonemes = StringFromWordArray(rr.Words, WordType.Pronunciation);
                        }
                        //txtRecoPho.Text = recoPhonemes;
                        return recoPhonemes;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
            
        }

        private string StringFromWordArray(ReadOnlyCollection<RecognizedWordUnit> words, WordType type)
        {
            try
            {
                string text = "";
                foreach (RecognizedWordUnit word in words)
                {
                    string wordText = "";
                    if (type == WordType.Text || type == WordType.Normalized)
                    {
                        wordText = word.Text;
                    }
                    else if (type == WordType.Lexical)
                    {
                        wordText = word.LexicalForm;
                    }
                    else if (type == WordType.Pronunciation)
                    {
                        wordText = word.Pronunciation;
                        //MessageBox.Show(word.LexicalForm);
                    }
                    else
                    {
                        throw new InvalidEnumArgumentException(String.Format("[0}: is not a valid input", type));
                    }
                    //Use display attribute

                    if ((word.DisplayAttributes & DisplayAttributes.OneTrailingSpace) != 0)
                    {
                        wordText += " ";
                    }
                    if ((word.DisplayAttributes & DisplayAttributes.TwoTrailingSpaces) != 0)
                    {
                        wordText += "  ";
                    }
                    if ((word.DisplayAttributes & DisplayAttributes.ConsumeLeadingSpaces) != 0)
                    {
                        wordText = wordText.TrimStart();
                    }
                    if ((word.DisplayAttributes & DisplayAttributes.ZeroTrailingSpaces) != 0)
                    {
                        wordText = wordText.TrimEnd();
                    }

                    text += wordText;

                }
                return text;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
            
        }

        private void reco_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            try
            {
                recoPhonemes = StringFromWordArray(e.Result.Words, WordType.Pronunciation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }         
        }

        private void reco_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            try
            {
                recoPhonemes = StringFromWordArray(e.Result.Words, WordType.Pronunciation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }         
        }


        #endregion

    }

    public enum WordType
    {
        Text,
        Normalized = Text,
        Lexical,
        Pronunciation
    }


}
