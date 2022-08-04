using Khandon.Shared.Dto.Enums;
using Khandon.Shared.Dto.Resposne;

namespace Khandon.Shared.Dto.Base
{
    public class BookBase
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        //public bool IsTextual { get; set; }

        public int Difficultye { get; set; } = 1;
        public BookTypeEnum BookType { get; set; }

        //Navigations
        public int BookGroupId { get; set; }
    }

   
}
