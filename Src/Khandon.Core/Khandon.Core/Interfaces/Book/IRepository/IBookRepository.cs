using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.Interfaces.IRepository
{
    public interface IBookRepository
    {
        Task<BookResult> CreateBookAsync(BookCreate bookCreate, string userId);
        Task<BookResult> EditBookAsync(BookEdit bookEdit, Guid bookId, string userId);
        Task<BookResult> GetBookByIdAsync(Guid Id, string UserId);
        Task<List<BookResult>> GetBooksAsync(string UserId,int groupId = 0);
        Task SafeDeleteBookAsync(Guid Id, string UserId);
    }
}
