using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Response;
using Khandon.Shared.Dto.Result;
using Khandon.SharerdKernel.UI.Applications.IServices;
using Khandon.SharerdKernel.UI.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI.Applications.Services
{
    internal class BookHttpService: IBookHttpService
    {
        private readonly IHttpService httpService;

        public BookHttpService(IHttpService httpService)
        {      
            this.httpService = httpService;
        }

        public async Task<BookResult> CreateBookAsync(BookCreate bookCreate)
        {
            //http call
            var result = await httpService.Post<BookCreate, BookResult>($"{ApplicationConfig.ApiUrl}/V1/Book/CreateBook", bookCreate);
            //deserialize
            if (result.HttpResponseMessage.IsSuccessStatusCode)
            {
                return result.Response;
            }
            throw new Exception();
        }
        public async Task<BookListResponse> GetAllBooksAsync()
        {
            try
            {
                var result = await httpService.Get<BookListResponse>($"{ApplicationConfig.ApiUrl}/V1/Book/GetBookList");
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
