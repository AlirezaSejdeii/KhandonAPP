using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Core.Interfaces.IRepository
{
    public interface IChapterRepository
    {
        Task<ChapterResult> CreateChapterAsync(ChapterCreate chapterCreate, string UserId);
        Task<ChapterResult> EditChapterAsync(ChapterEdit chapterEdit, string UserId);
        Task<ChapterResult> GetChapterByIdAsync(Guid Id, string UserId);
        Task<List<ChapterResult>> GetChaptersAsync(string UserId, int limit = 10, int page = 1);
        Task<List<ChapterResult>> GetChaptersByBookIdAsync(Guid BookId, string UserId, int limit = 10, int page = 1);
        void SafeDeleteChapter(Guid Id,string UserId);
    }
}
