using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellPronounce.Web.ApiModels;
using WellPronounce.Web.DBContext;
using WellPronounce.Web.Entities;
using WellPronounce.Web.Interfaces;

namespace WellPronounce.Web.Repositories
{
    public class TextToSpeechRepository : ITextToSpeechRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public TextToSpeechRepository(ApplicationDBContext applicationDBContext)
        {
            _dbContext = applicationDBContext;
        }

        public async Task<List<SpeechDetail>> GetRecords()
        {
            return await _dbContext.SpeechDetails.OrderByDescending(x=>x.Created).ToListAsync();
        }

        public async Task<StandardOutputModel> StandardProcessSaveTextToSpeechData(string blobPath , StandardTextRequestModel standardTextRequestModel)
        {
            var existingData = _dbContext.SpeechDetails.Where(x => x.InputText.Equals(standardTextRequestModel.Text)).FirstOrDefault();
            if (existingData == null)
            {
                var record = new SpeechDetail
                {
                    UniqueId = Guid.NewGuid(),
                    InputText = standardTextRequestModel.Text,
                    Language = standardTextRequestModel.Language,
                    ProcessType = standardTextRequestModel.ProcessType,
                    BlobPath = blobPath,
                    Created = DateTime.UtcNow
                };

                _dbContext.SpeechDetails.Add(record);
                await _dbContext.SaveChangesAsync();

                var response = new StandardOutputModel
                {
                    Path = record.BlobPath,
                    UniqueId = record.UniqueId.ToString()
                };

                return response;
            }
            else
            {
                var response = new StandardOutputModel
                {
                    Path = existingData.BlobPath,
                    UniqueId = existingData.UniqueId.ToString()
                };

                return response;
            }
            
        }

    }
}
