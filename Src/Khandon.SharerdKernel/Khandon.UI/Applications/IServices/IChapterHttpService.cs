using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI.Applications.IServices
{
    public interface IChapterHttpService
    {
        Task<ChapterResult> CreateChapterAsync(ChapterCreate chapterCreate);
    }
}
