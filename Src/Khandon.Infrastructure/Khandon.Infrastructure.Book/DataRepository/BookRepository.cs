using Khandon.Core.Interfaces.IRepository;
using Khandon.Domain.Enitties.Book;
using Khandon.Domain.Enums;
using Khandon.Infrastructure.Book.DataContext;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Khandon.Shared.Utilities;
using Microsoft.EntityFrameworkCore;
using Khandon.Shared.Dto.Enums;

namespace Khandon.Infrastructure.Book.DataRepository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContext _bookContext;

        public BookRepository(BookContext bookContext)
        {
            _bookContext = bookContext;
        }

        private BookResult ConvertBookToBookResult(Domain.Enitties.Book.Book book)
        {
            return new BookResult()
            {
                Id = book.Id,
                IsDeleted = book.IsDeleted,
                BookGroup = new Shared.Dto.Resposne.BookGroupResult()
                {
                    Id = book.BookGroup.Id,
                    Title = book.BookGroup.Title,
                    ShortDescription = book.BookGroup.ShortDescription
                },
                BookGroupId = book.BookGroupId,
                BookType = (BookTypeEnum)book.BookType,
                Chapters = new List<ChapterResult>(),
                CreateDate = book.CreateDate,
                Description = book.Description,
                Title = book.Title,
                Difficultye = book.Difficultye,
                //IsTextual = book.IsTextual,
                IsComplited = book.IsComplited
            };
        }
        //add bool
        public async Task<BookResult> CreateBookAsync(BookCreate bookCreate, string userId)
        {
            if (bookCreate != null)
            {
                Domain.Enitties.Book.Book book = new Domain.Enitties.Book.Book()
                {
                    BookGroupId = bookCreate.BookGroupId,
                    BookType = (BookType)bookCreate.BookType,
                    CreateDate = MyDateTime.Now(),
                    Description = bookCreate.Description,
                    Title = bookCreate.Title,
                    Difficultye =(byte) bookCreate.Difficultye,
                    //IsTextual = bookCreate.IsTextual,
                    IsDeleted = false,
                    IsComplited = false,
                    UserId = userId
                };

                try
                {
                    await _bookContext.Books.AddAsync(book);
                    await _bookContext.SaveChangesAsync();


                    if (book.Id == null || string.IsNullOrEmpty(book.Id.ToString()))
                    {
                        throw new ArgumentNullException("Book can't be genrate");
                    }
                    //book.BookGroup = _bookContext.BookGroups.First(a => a.Id == book.BookGroupId);
                    return await GetBookByIdAsync(book.Id, userId);
                }
                catch (Exception e)
                {

                    throw new ArgumentNullException(e.Message);
                }

            }
            else
                throw new ArgumentNullException("BookCreate can not be null");
        }
        //edit book
        public async Task<BookResult> EditBookAsync(BookEdit bookEdit, Guid bookId, string userId)
        {
            if (bookEdit.Id != bookId)
            {
                throw new KeyNotFoundException("Book Id Invalid");
            }
            Domain.Enitties.Book.Book book = new Domain.Enitties.Book.Book();

            book = await _bookContext.Books.FirstOrDefaultAsync(A => A.Id == bookId && A.IsDeleted != true);

            if (book == null)
            {
                throw new KeyNotFoundException("Book Is Invalid, Maybe Deleted");
            }

            if (book.UserId != userId)
            {
                throw new KeyNotFoundException("User with this key not match with currnt book");
            }
            if (book != null)
            {
                book.Title = bookEdit.Title;
                book.Description = bookEdit.Description;
                book.Difficultye = (byte)bookEdit.Difficultye;
                book.BookGroupId = bookEdit.BookGroupId;
                book.BookType = (BookType)bookEdit.BookType;
                book.IsComplited = bookEdit.IsComplited;
                //book.IsTextual = bookEdit.IsTextual;
                try
                {
                    _bookContext.Books.Update(book);
                    _bookContext.SaveChanges();

                }
                catch (Exception e)
                {
                    throw new InvalidOperationException(e.Message);
                }
            }
            //book.BookGroup = _bookContext.BookGroups.First(a => a.Id == book.BookGroupId);
            return await GetBookByIdAsync(book.Id, userId);
        }

        //get book by filter
        public async Task<List<BookResult>> GetBooksAsync(string UserId, int groupId = 0)
        {
            IQueryable<BookResult> bookResult = _bookContext.Books.Select(book => new BookResult()
            {
                Id = book.Id,
                IsDeleted = book.IsDeleted,
                BookGroup = new Shared.Dto.Resposne.BookGroupResult()
                {
                    Id = book.BookGroup.Id,
                    Title = book.BookGroup.Title,
                    ShortDescription = book.BookGroup.ShortDescription
                },
                BookGroupId = book.BookGroupId,
                BookType = (BookTypeEnum)book.BookType,
                Chapters = book.Chapters.Where(a => a.IsDeleted == false).Select(chapter => new ChapterResult()
                {
                    Id = chapter.Id,
                    BookId = chapter.BookId,
                    ShortDescription = chapter.ShortDescription,
                    Title = chapter.Title,
                    IsDeleted = chapter.IsDeleted,
                    Studies = chapter.Studies.Where(a => a.IsDeleted == false).Select(study => new StudyResult()
                    {
                        Id= study.Id,
                        BookType=(BookTypeEnum)book.BookType,
                        IsDeleted = study.IsDeleted,
                        ChapterId = study.ChapterId,
                        Length = study.Length,
                        ShortDescription = study.ShortDescription,
                        CreateDate = study.CreateDate,
                        Title = study.Title,
                        Flag=study.Flag,
                    }).ToList()
                }).ToList(),
                CreateDate = book.CreateDate,
                Description = book.Description,
                Title = book.Title,
                Difficultye = book.Difficultye,
                //IsTextual = book.IsTextual,
                IsComplited = book.IsComplited,
                UserId = book.UserId
            }).AsQueryable();

            if (groupId != 0)
            {
                bookResult = bookResult.Where(a => a.BookGroupId == groupId);
            }
            return await bookResult.Where(a => a.IsDeleted == false && a.UserId == UserId).AsNoTracking().ToListAsync();
        }

        public async Task<BookResult> GetBookByIdAsync(Guid Id, string UserId)
        {
            if (_bookContext.Books.Any(a => a.Id == Id))
            {

                BookResult bookResult = await _bookContext.Books.Where(a=> a.UserId==UserId).Select(book => new BookResult()
                {
                    Id = book.Id,
                    IsDeleted = book.IsDeleted,
                    BookGroup = new Shared.Dto.Resposne.BookGroupResult()
                    {
                        Id = book.BookGroup.Id,
                        Title = book.BookGroup.Title,
                        ShortDescription = book.BookGroup.ShortDescription
                    },
                    BookGroupId = book.BookGroupId,
                    BookType = (BookTypeEnum)book.BookType,
                    Chapters = book.Chapters.Where(a => a.IsDeleted == false).Select(chapter => new ChapterResult()
                    {
                        Id = chapter.Id,
                        BookId = chapter.BookId,
                        ShortDescription = chapter.ShortDescription,
                        Title = chapter.Title,
                        IsDeleted = chapter.IsDeleted,
                        Studies = chapter.Studies.Where(a => a.IsDeleted == false).Select(study => new StudyResult()
                        {
                            Id=study.Id,
                            IsDeleted = study.IsDeleted,
                            ChapterId = study.ChapterId,
                            Length = study.Length,
                            ShortDescription = study.ShortDescription,
                            CreateDate = study.CreateDate,
                            Title = study.Title,
                            Flag=study.Flag,
                        }).Where(a => a.IsDeleted == false).ToList()
                    }).Where(a=> a.IsDeleted==false).ToList(),
                    CreateDate = book.CreateDate,
                    Description = book.Description,
                    Title = book.Title,
                    Difficultye = book.Difficultye,
                    //IsTextual = book.IsTextual,
                    IsComplited = book.IsComplited,
                    UserId=book.UserId
                }).AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                return bookResult;
            }
            else
            {
                return null;
            }
        }

        public async Task SafeDeleteBookAsync(Guid Id, string UserId)
        {
            if (_bookContext.Books.FirstOrDefault(a => a.Id == Id && a.IsDeleted == false) is Domain.Enitties.Book.Book book)
            {
                if (book.UserId != UserId)
                    throw new KeyNotFoundException("UserId is invalid");

                book = await _bookContext.Books.FirstAsync(a => a.Id == Id);
                book.IsDeleted = true;
                await _bookContext.SaveChangesAsync();
            }
            else
                throw new KeyNotFoundException("Book id is invalid");
        }


    }
}
