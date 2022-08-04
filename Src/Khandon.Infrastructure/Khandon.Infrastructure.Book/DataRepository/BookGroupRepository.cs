using Khandon.Core.Interfaces.IRepository;
using Khandon.Domain.Enitties.Book;
using Khandon.Infrastructure.Book.DataContext;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Resposne;

namespace Khandon.Infrastructure.Book.DataRepository
{
    public class BookGroupRepository : IBookGroupRepository
    {
        private readonly BookContext _bookContext;

        public BookGroupRepository(BookContext bookContext)
        {
            _bookContext = bookContext;
        }

        public BookGroupResult CreateBookgroup(BookGroupCreate bookGroupCreate)
        {
            try
            {
                BookGroup bookgroupToCreate = new BookGroup()
                {
                    ShortDescription = bookGroupCreate.ShortDescription,
                    Title = bookGroupCreate.Title
                };
                _bookContext.BookGroups.Add(bookgroupToCreate);
                _bookContext.SaveChanges();

                return new BookGroupResult()
                {
                    Id = bookgroupToCreate.Id,
                    ShortDescription = bookGroupCreate.ShortDescription,
                    Title = bookGroupCreate.Title
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public BookGroupResult EditBookGroup(BookGroupEdit bookGroupEdit)
        {
            if (bookGroupEdit.Id == 0 || bookGroupEdit.Id == null)
            {
                throw new Exception("the value if Id is 0");
            }
            try
            {
                BookGroup bookgroupToCreate = new BookGroup()
                {
                    Id = bookGroupEdit.Id,
                    ShortDescription = bookGroupEdit.ShortDescription,
                    Title = bookGroupEdit.Title
                };
                _bookContext.BookGroups.Update(bookgroupToCreate);
                _bookContext.SaveChanges();

                return new BookGroupResult()
                {
                    Id = bookgroupToCreate.Id,
                    ShortDescription = bookGroupEdit.ShortDescription,
                    Title = bookGroupEdit.Title
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public BookGroupResult GetBookById(int Id)
        {
            if (Id==0)
            {
                return null;
            }
            if (_bookContext.BookGroups.Any(a => a.Id == Id))
            {
                BookGroup bookGroup = _bookContext.BookGroups.First(a => a.Id == Id);
                return new BookGroupResult()
                {
                    Id = bookGroup.Id,
                    ShortDescription = bookGroup.ShortDescription,
                    Title = bookGroup.Title
                };
            }
            return null;
        }
        public List<BookGroupResult> GetBookGroups()
        {
            List<BookGroupResult> bookGroups = _bookContext.BookGroups.Select(a => new BookGroupResult()
            {
                Id = a.Id,
                ShortDescription = a.ShortDescription,
                Title = a.Title
            }).ToList();
            return bookGroups;
        }

    }
}
