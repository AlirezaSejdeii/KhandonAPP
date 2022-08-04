using Khandon.Core.Interfaces.IRepository;
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
    public class BookDeleteCommand:IRequest<bool>
    {
        public ClaimsPrincipal User { get; set; }
        public Guid BookId { get; set; }
    }

    public class BookDeleteCommandHandler : IRequestHandler<BookDeleteCommand, bool>
    {
        private readonly IBookRepository bookRepository;

        public BookDeleteCommandHandler(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public async Task<bool> Handle(BookDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
               await bookRepository.SafeDeleteBookAsync(request.BookId, request.User.GetUserId());
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
