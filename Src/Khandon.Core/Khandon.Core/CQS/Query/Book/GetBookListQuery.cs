using Khandon.Core.Interfaces.IRepository;
using Khandon.Shared.Dto.Response;
using Khandon.Shared.Dto.Result;
using Khandon.Shared.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Query
{
    public class GetBookListQuery:IRequest<BookListResponse>
    {
        public ClaimsPrincipal User { get; set; }
    }

    public  class GetBookListQueryHandler : IRequestHandler<GetBookListQuery, BookListResponse>
    {

        private readonly IBookRepository _bookRepository;

        public GetBookListQueryHandler(IBookRepository bookRepository)
        {
            this._bookRepository = bookRepository;
        }

        public async Task<BookListResponse> Handle(GetBookListQuery request, CancellationToken cancellationToken)
        {
            string userId = request.User.GetUserId();
            List<BookResult>  books= await _bookRepository.GetBooksAsync(userId);

            return new BookListResponse()
            {
                Books = books
            };
        }
    }
}
