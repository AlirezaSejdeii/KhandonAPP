using Khandon.Shared.Dto.Enums;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Response;
using Khandon.Shared.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI.Applications.IServices
{
    public interface IStudyHttpService
    {
        Task<StudyResult> CreateStudyAsync(StudyCreate studyCreate);
        Task<List<StatusActiveReadsDto>> GetPeriodTimeSocityActivityAsync(int limit = 10, ReadingActivityDateEnum time = ReadingActivityDateEnum.WEEK);
        Task<SocietyStatusDto> GetSocietyStatusAsync();
    }
}
