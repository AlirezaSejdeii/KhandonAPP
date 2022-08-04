using Khandon.Shared.Dto.Resposne;
using Khandon.SharerdKernel.UI.Applications.IServices;
using Khandon.SharerdKernel.UI.Helper;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI.State
{
    public class BookGroupStateService
    {
        private readonly IBookGroupHttpService bookGroupHttpService;
        private readonly ISnackbar snackbar;

        List<BookGroupResult> BookGroups = new();

        public BookGroupStateService(ISnackbar snackbar, IBookGroupHttpService bookGroupHttpService)
        {
            this.snackbar = snackbar;
            this.bookGroupHttpService = bookGroupHttpService;
        }


        public async Task<List<BookGroupResult>> GetBookGroupsAsync()
        {
            if (!BookGroups.Any())
            {
                try
                {
                    Console.WriteLine("fetch 'BOOK GROUPS' from network");
                    var allBookGroups = await bookGroupHttpService.GetAllBookGroupAsync();
                    if (allBookGroups.Any())
                    {
                        BookGroups = allBookGroups;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //Books = new List<BookResult>();
                    snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Warning);
                }
            }
            return BookGroups;
        }
    }
}
