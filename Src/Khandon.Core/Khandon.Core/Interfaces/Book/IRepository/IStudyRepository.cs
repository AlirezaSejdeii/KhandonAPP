using Khandon.Shared.Dto.Enums;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Response;
using Khandon.Shared.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.Interfaces.IRepository
{
    public interface IStudyRepository
    {
        Task<StudyResult> CreateStudyAsync(StudyCreate studyCreate, string userId);
        Task<StudyResult> EditStudyAsync(StudyEdit studyEdit, string userId);
        Task<List<StatusActiveReadsDto>> GetPeriodTimeSocityActivityAsync(ReadingActivityDateEnum time, int limit);
        Task<SocietyStatusDto> GetSocietyStatusAsync(string userId);
        Task<List<StudyResult>> GetStudyListAsync(string userId, StudyGet studyGet);
        Task<StudyResult> GetStudyResultById(Guid Id, string UserId);
        Task<bool> SafeDeleteStudyAsync(string userId, Guid studyId);
    }
}
