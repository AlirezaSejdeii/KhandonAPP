using Khandon.Core.Interfaces.IRepository;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Resposne;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Command
{
    public class BookGroupEditCommand : IRequest<BookGroupResult>
    {
        public BookGroupEdit BookGroupEdit { get; set; }
    }

    public class BookGroupEditCommandHandler : IRequestHandler<BookGroupEditCommand, BookGroupResult>
    {
        private readonly IBookGroupRepository _bookGroupRepository;

        public BookGroupEditCommandHandler(IBookGroupRepository bookGroupRepository)
        {
            _bookGroupRepository = bookGroupRepository;
        }

        public async Task<BookGroupResult> Handle(BookGroupEditCommand request, CancellationToken cancellationToken)
        {
            BookGroupResult bookGroupResult = _bookGroupRepository.EditBookGroup(request.BookGroupEdit);

            return bookGroupResult;
        }
    }
}
