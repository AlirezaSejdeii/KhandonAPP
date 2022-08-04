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
    public class BookCreateCommand : IRequest<BookResult>
    {
        public ClaimsPrincipal User { get; set; }
        public BookCreate BookCreate { get; set; }
    }

    public class BookCreateCommandHandler : IRequestHandler<BookCreateCommand, BookResult>
    {
        private readonly IBookRepository _bookRepository;

        public BookCreateCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookResult> Handle(BookCreateCommand request, CancellationToken cancellationToken)
        {
            string userId = request.User.GetUserId();

            //BookResult bookResult= await _bookRepository.CreateBookAsync(request.BookCreate, userId);

            var result = await _bookRepository.CreateBookAsync(request.BookCreate, userId);


            return result;
        }
    }
}
