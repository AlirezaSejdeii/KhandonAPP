using Khandon.Core.Interfaces.IRepository;
using Khandon.Shared.Dto.Resposne;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Query
{
   public class GetBookGroupsQuery:IRequest<List<BookGroupResult>>
    {
    }
    public class GetBookGroupsQueryHandler:IRequestHandler<GetBookGroupsQuery,List<BookGroupResult>>
    {
        private readonly IBookGroupRepository bookGroupRepository;

        public GetBookGroupsQueryHandler(IBookGroupRepository bookGroupRepository)
        {
            this.bookGroupRepository = bookGroupRepository;
        }

        public async Task<List<BookGroupResult>> Handle(GetBookGroupsQuery request, CancellationToken cancellationToken)
        {
            var result = bookGroupRepository.GetBookGroups();
            return result;
        }
    }
}
