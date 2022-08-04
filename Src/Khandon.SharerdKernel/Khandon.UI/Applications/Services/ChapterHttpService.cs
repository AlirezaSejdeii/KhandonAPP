using Khandon.Shared.Dto.Request;
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
    public class ChapterHttpService : IChapterHttpService
    {
        private readonly IHttpService httpService;

        public ChapterHttpService(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<ChapterResult> CreateChapterAsync(ChapterCreate chapterCreate)
        {
            try
            {
                //http call
                var result = await httpService.Post<ChapterCreate, ChapterResult>($"{ApplicationConfig.ApiUrl}/V1/Chapter/CreateChapter", chapterCreate);
                //deserialize
                if (result.HttpResponseMessage.IsSuccessStatusCode)
                {
                    return result.Response;
                }
                Console.WriteLine(result.HttpResponseMessage.Content.ReadAsStringAsync());
                throw new Exception("Response Is Not Successful");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
