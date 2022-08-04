using Khandon.Core.Interfaces.IRepository;
using Khandon.Shared.Dto.Enums;
using Khandon.Shared.Dto.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.CQS.Query
{
    public class GetPeriodTimeSocityActivityQuery:IRequest<List<StatusActiveReadsDto>>
    {
        public ReadingActivityDateEnum time { get; set; }
        public int limit { get; set; }
    }

    public class GetPeriodTimeSocityActivityQueryHandler : IRequestHandler<GetPeriodTimeSocityActivityQuery, List<StatusActiveReadsDto>>
    {
        private readonly IStudyRepository studyRepository;

        public GetPeriodTimeSocityActivityQueryHandler(IStudyRepository studyRepository)
        {
            this.studyRepository = studyRepository;
        }

        public async Task<List<StatusActiveReadsDto>> Handle(GetPeriodTimeSocityActivityQuery request, CancellationToken cancellationToken)
        {
            return await studyRepository.GetPeriodTimeSocityActivityAsync(request.time, request.limit);
        }
    }
}
