using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellPronounce.Web.ApiModels;
using WellPronounce.Web.Interfaces;

namespace WellPronounce.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextToSpeechController : ControllerBase
    {
        private readonly ITextToSpeechService _textToSpeechService;

        public TextToSpeechController(ITextToSpeechService textToSpeechService)
        {
            _textToSpeechService = textToSpeechService;
        }

        [HttpPost("StandardProcess")]
        public async Task<IActionResult> StandardProcess(TextRequestModel textRequestModel)
        {
            try
            {
                var response = await _textToSpeechService.StandardProcessSaveTextToSpeechData(textRequestModel);               
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        [HttpGet("Records")]
        public async Task<IActionResult> GetRecords()
        {
            try
            {
                var records = await _textToSpeechService.GetRecords();
                return Ok(records);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}
