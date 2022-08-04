using Khandon.Shared.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Dto.Response
{
    /// <summary>
    /// this for showing in index page (books)  and books detail page
    /// </summary>
    public class BookListResponse
    {
        public List<BookResult> Books { get; set; }
    }
}
