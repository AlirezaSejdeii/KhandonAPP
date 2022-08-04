using Khandon.Core.Interfaces.IRepository;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Result;
using Khandon.Shared.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Command
{
    public class BookEditCommand : IRequest<BookResult>
    {
        public ClaimsPrincipal User { get; set; }
        public BookEdit BookEdit { get; set; }
        public Guid BookId { get; set; }
    }

    public class BookEditCommandHandler : IRequestHandler<BookEditCommand, BookResult>
    {
        private readonly IBookRepository _bookRepository;

        public BookEditCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookResult> Handle(BookEditCommand request, CancellationToken cancellationToken)
        {
            string userId = request.User.GetUserId();

            BookResult bookResult = await _bookRepository.EditBookAsync(request.BookEdit,request.BookId, userId);

            return bookResult;
        }
    }
}
