using Khandon.Core.Interfaces.IRepository;
using Khandon.Domain.Enitties.Book;
using Khandon.Domain.Enums;
using Khandon.Infrastructure.Book.DataContext;
using Khandon.Shared.Dto.Enums;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Result;
using Microsoft.EntityFrameworkCore;

namespace Khandon.Infrastructure.Book.DataRepository
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly BookContext _bookContext;

        public ChapterRepository(BookContext bookContext)
        {
            _bookContext = bookContext;
        }
        public async Task<ChapterResult> CreateChapterAsync(ChapterCreate chapterCreate, string UserId)
        {
            if (chapterCreate == null)
            {
                throw new ArgumentNullException(paramName: "chapterCreate");
            }
            if (string.IsNullOrEmpty(chapterCreate.Title))
            {
                throw new ArgumentNullException(paramName: nameof(chapterCreate.Title));
            }
            if (!_bookContext.Books.Any(a => a.Id == chapterCreate.BookId && a.UserId == UserId))
            {
                throw new KeyNotFoundException("BookId Not Be Found");
            }
            Chapter chapter = new Chapter()
            {
                BookId = chapterCreate.BookId,
                IsDeleted = false,
                ShortDescription = chapterCreate.ShortDescription,
                Title = chapterCreate.Title
            };
            try
            {
                await _bookContext.AddAsync(chapter);
                await _bookContext.SaveChangesAsync();
                return await GetChapterByIdAsync(chapter.Id);

            }
            catch (ArgumentNullException e)
            {
                throw e;
            }
        }

        public async Task<ChapterResult> EditChapterAsync(ChapterEdit chapterEdit, string UserId)
        {
            if (chapterEdit == null)
            {
                throw new ArgumentNullException(paramName: "chapterEdit");
            }
            if (string.IsNullOrEmpty(chapterEdit.Title))
            {
                throw new ArgumentNullException(paramName: nameof(chapterEdit.Title));
            }
            if (!_bookContext.Books.Any(a => a.Id == chapterEdit.BookId && a.UserId == UserId))
            {
                throw new KeyNotFoundException("Bookid Can Not Be Found");
            }
            if (_bookContext.Chapters.FirstOrDefault(a => a.Id == chapterEdit.Id) is Chapter chapter)
            {
                chapter.BookId = chapterEdit.BookId;
                chapter.ShortDescription = chapterEdit.ShortDescription;
                chapter.Title = chapterEdit.Title;
                try
                {
                    _bookContext.Update(chapter);
                    _bookContext.SaveChanges();
                    return await GetChapterByIdAsync(chapter.Id);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                throw new KeyNotFoundException("Chapter id was not found");
            }
        }

        public void SafeDeleteChapter(Guid Id, string UserId)
        {
            if (_bookContext.Chapters.Include(a => a.Book).FirstOrDefault(a => a.Id == Id && a.Book.UserId == UserId) is Chapter chapter)
            {
                chapter.IsDeleted = true;
                _bookContext.Chapters.Update(chapter);
                _bookContext.SaveChanges();
            }
            else
                throw new KeyNotFoundException("Chapter Id Not Valid");
        }

        public async Task<ChapterResult> GetChapterByIdAsync(Guid Id, string UserId)
        {
            if (await _bookContext.Chapters.Include(c => c.Studies).Include(a=> a.Book).Where(a => a.IsDeleted == false).AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id && a.Book.UserId == UserId && a.IsDeleted==false) is Chapter chapter)
            {
                return new ChapterResult()
                {
                    Id = chapter.Id,
                    Title = chapter.Title,
                    ShortDescription = chapter.ShortDescription,
                    IsDeleted = chapter.IsDeleted,
                    BookId = chapter.BookId,
                    Studies = chapter.Studies.Where(a => a.IsDeleted == false).Select(study => new StudyResult()
                    {
                        Id = study.Id,
                        BookType = (Shared.Dto.Enums.BookTypeEnum)study.Chapter.Book.BookType,
                        IsDeleted = study.IsDeleted,
                        ShortDescription = study.ShortDescription,
                        Title = study.Title,
                        ChapterId = study.ChapterId,
                        Length = study.Length,
                        CreateDate = study.CreateDate,
                    }).ToList()
                };
            }
            return null;
        }

        public async Task<List<ChapterResult>> GetChaptersByBookIdAsync(Guid BookId, string UserId, int limit = 10, int page = 1)
        {
            IQueryable<ChapterResult> chapterResults = _bookContext.Chapters.Include(a => a.Studies)
             .Where(a => a.Book.UserId == UserId && a.BookId == BookId && a.IsDeleted==false)
             .Select(chapter => new ChapterResult()
             {
                 Id = chapter.Id,
                 Title = chapter.Title,
                 ShortDescription = chapter.ShortDescription,
                 IsDeleted = chapter.IsDeleted,
                 BookId = chapter.BookId,
                 Studies = chapter.Studies.Where(a => a.IsDeleted == false).Select(study => new StudyResult()
                 {
                     Id=study.Id,
                     BookType = (BookTypeEnum) study.Chapter.Book.BookType,
                     IsDeleted = study.IsDeleted,
                     ShortDescription = study.ShortDescription,
                     Title = study.Title,
                     ChapterId = study.ChapterId,
                     Length = study.Length,
                     CreateDate = study.CreateDate,                     
                 }).Where(a => a.IsDeleted == false).ToList()
             }).AsQueryable();

            chapterResults = chapterResults.Skip((page - 1) * limit).Take(limit);

            return await chapterResults.AsNoTracking().ToListAsync();
        }
        private async Task<ChapterResult> GetChapterByIdAsync(Guid Id)
        {
            if (await _bookContext.Chapters.Include(c => c.Studies).Where(a => a.IsDeleted == false).AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id) is Chapter chapter)
            {
                return new ChapterResult()
                {
                    Id = chapter.Id,
                    Title = chapter.Title,
                    ShortDescription = chapter.ShortDescription,
                    IsDeleted = chapter.IsDeleted,
                    BookId = chapter.BookId,
                    Studies = chapter.Studies.Select(study => new StudyResult()
                    {
                        IsDeleted = study.IsDeleted,
                        ShortDescription = study.ShortDescription,
                        Title = study.Title,
                        ChapterId = study.ChapterId,
                        Length = study.Length,
                        CreateDate = study.CreateDate,
                    }).ToList()
                };
            }
            return null;
        }
        public async Task<List<ChapterResult>> GetChaptersAsync(string UserId, int limit = 10, int page = 1)
        {
            IQueryable<ChapterResult> chapterResults = _bookContext.Chapters.Include(a => a.Studies)
                .Where(a => a.Book.UserId == UserId)
                .Select(chapter => new ChapterResult()
                {
                    Id = chapter.Id,
                    Title = chapter.Title,
                    ShortDescription = chapter.ShortDescription,
                    IsDeleted = chapter.IsDeleted,
                    BookId = chapter.BookId,
                    Studies = chapter.Studies.Select(study => new StudyResult()
                    {
                        IsDeleted = study.IsDeleted,
                        ShortDescription = study.ShortDescription,
                        Title = study.Title,
                        ChapterId = study.ChapterId,
                        Length = study.Length,
                        CreateDate = study.CreateDate,
                    }).Where(a => a.IsDeleted == false).ToList()
                }).AsQueryable();

            chapterResults = chapterResults.Skip((page - 1) * limit).Take(limit);

            return await chapterResults.AsNoTracking().ToListAsync();

        }
    }
}
