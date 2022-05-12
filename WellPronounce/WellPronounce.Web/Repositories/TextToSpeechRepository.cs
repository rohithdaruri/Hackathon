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

        public async Task<StandardOutputModel> GetDetailByName(StandardTextRequestModel standardTextRequestModel)
        {
            var existing = await _dbContext.SpeechDetails.Where(x => x.PreferedName.Equals(standardTextRequestModel.PreferedName) && x.LegalFirstName.Equals(standardTextRequestModel.LegalFirstName) && x.LegalLastName.Equals(standardTextRequestModel.LegalLastName)).FirstOrDefaultAsync();

            if (existing != null)
            {
                var response = new StandardOutputModel
                {
                    Path = existing.BlobPath,
                    UniqueId = existing.UniqueId.ToString(),
                    Phonetics = existing.Phonetics
                };

                return response;
            }
            else
            {
                return null;
            }          
        }

        public async Task<StandardOutputModel> StandardProcessSaveTextToSpeechData(string blobPath , StandardTextRequestModel standardTextRequestModel, string phonetics)
        {
            var existingData = await _dbContext.SpeechDetails.Where(x => x.PreferedName.Equals(standardTextRequestModel.PreferedName) && x.LegalFirstName.Equals(standardTextRequestModel.LegalFirstName) && x.LegalLastName.Equals(standardTextRequestModel.LegalLastName)).FirstOrDefaultAsync();
            if (existingData == null)
            {
                var record = new SpeechDetail
                {
                    UniqueId = Guid.NewGuid(),
                    LegalFirstName = standardTextRequestModel.LegalFirstName,
                    LegalLastName = standardTextRequestModel.LegalLastName,
                    PreferedName = standardTextRequestModel.PreferedName,
                    Phonetics = phonetics,
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
                    UniqueId = record.UniqueId.ToString(),
                    Phonetics = record.Phonetics
                };

                return response;
            }
            else
            {
                var response = new StandardOutputModel
                {
                    Path = existingData.BlobPath,
                    UniqueId = existingData.UniqueId.ToString(),
                    Phonetics = existingData.Phonetics
                };

                return response;
            }
            
        }

    }
}
