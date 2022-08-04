using Khandon.Shared.Dto.Resposne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.Interfaces.IRepository
{
    public interface IBookGroupRepository
    {
        Khandon.Shared.Dto.Resposne.BookGroupResult CreateBookgroup(Khandon.Shared.Dto.Request.BookGroupCreate bookGroupCreate);
        Khandon.Shared.Dto.Resposne.BookGroupResult EditBookGroup(Khandon.Shared.Dto.Request.BookGroupEdit bookGroupEdit);
        BookGroupResult GetBookById(int Id);
        List<BookGroupResult> GetBookGroups();
    }
}
