using Khandon.Shared.Dto.Resposne;
using Khandon.SharerdKernel.UI.Applications.IServices;
using Khandon.SharerdKernel.UI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI.Applications.Services
{
    internal class BookGroupHttpService: IBookGroupHttpService
    {
        private readonly IHttpService httpService;

        public BookGroupHttpService(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<List<BookGroupResult>> GetAllBookGroupAsync ()
        {
            try
            {
                var result = await httpService.Get<List<BookGroupResult>>($"{ApplicationConfig.ApiUrl}/V1/BookGroup/GetAllBookGroup");
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
