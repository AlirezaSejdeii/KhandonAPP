using Khandon.Shared.Dto.Resposne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI.Applications.IServices
{
    public interface IBookGroupHttpService
    {
        Task<List<BookGroupResult>> GetAllBookGroupAsync();
    }
}
