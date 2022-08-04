using Khandon.Core.Interfaces.IRepository;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Resposne;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Command
{
    public class BookGroupCreateCommand : IRequest<BookGroupResult>
    {
        public BookGroupCreate BookGroupCreate { get; set; }
    }

    public class BookGroupCreateCommandHandler : IRequestHandler<BookGroupCreateCommand, BookGroupResult>
    {
        private readonly IBookGroupRepository _bookGroupRepository;

        public BookGroupCreateCommandHandler(IBookGroupRepository bookGroupRepository)
        {
            _bookGroupRepository = bookGroupRepository;
        }

        public async Task<BookGroupResult> Handle(BookGroupCreateCommand request, CancellationToken cancellationToken)
        {
            BookGroupResult bookGroupResult= _bookGroupRepository.CreateBookgroup(request.BookGroupCreate);

            return bookGroupResult;
        }
    }
}
