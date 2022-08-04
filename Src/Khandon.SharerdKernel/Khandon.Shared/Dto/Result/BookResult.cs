using Khandon.Shared.Dto.Base;
using Khandon.Shared.Dto.Resposne;

namespace Khandon.Shared.Dto.Result
{
    public class BookResult:BookBase
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public bool IsComplited { get; set; }
        public BookGroupResult BookGroup { get; set; }

        public DateTime CreateDate { get; set; }
        public List<ChapterResult> Chapters { get; set; }

    }
}
