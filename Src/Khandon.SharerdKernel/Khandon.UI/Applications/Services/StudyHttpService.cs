using Khandon.Shared.Dto.Enums;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Response;
using Khandon.Shared.Dto.Result;
using Khandon.SharerdKernel.UI.Applications.IServices;
using Khandon.SharerdKernel.UI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI.Applications.Services
{
    public class StudyHttpService : IStudyHttpService
    {
        private readonly IHttpService httpService;

        public StudyHttpService(IHttpService httpService)
        {
            this.httpService = httpService;
        }
        public async Task<StudyResult> CreateStudyAsync(StudyCreate studyCreate)
        {
            //http call
            var result = await httpService.Post<StudyCreate, StudyResult>($"{ApplicationConfig.ApiUrl}/V1/Study/CreateStudy", studyCreate);
            //deserialize
            if (result.HttpResponseMessage.IsSuccessStatusCode)
            {
                return result.Response;
            }
            throw new Exception();
        }
        public async Task<SocietyStatusDto> GetSocietyStatusAsync()
        {
            try
            {
                var result = await httpService.Get<SocietyStatusDto>($"{ApplicationConfig.ApiUrl}/V1/Study/GetSocietyStatus");
                return result.Response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }
        public async Task<List<StatusActiveReadsDto>> GetPeriodTimeSocityActivityAsync(
            int limit=10, ReadingActivityDateEnum time=ReadingActivityDateEnum.WEEK)
        {
            try
            {
                var result = await httpService.Get<List<StatusActiveReadsDto>>($"{ApplicationConfig.ApiUrl}/V1/Study/GetPeriodTimeSocityActivity?time={time}&limit={limit}");
                return result.Response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }
    }
}
