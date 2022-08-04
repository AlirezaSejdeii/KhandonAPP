using FluentAssertions;
using Khandon.Domain.Enitties.Book;
using Khandon.Infrastructure.Book.DataContext;
using Khandon.Infrastructure.Book.DataRepository;
using Khandon.Shared.Dto.Request;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Khandon.Infrastructure.Book.Tests.DataRepository
{
    public class ChapterRepositoryTest
    {
        private readonly SqliteConnectionStringBuilder ConnectionStringBuilder;
        private readonly SqliteConnection Connection;
        private readonly DbContextOptions<BookContext> Options;
        private readonly string UserId;
        public ChapterRepositoryTest()
        {
            ConnectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = ":memory:" };
            Connection = new SqliteConnection(ConnectionStringBuilder.ToString());
            Options = new DbContextOptionsBuilder<BookContext>().UseSqlite(Connection).Options;
            UserId = Guid.NewGuid().ToString();
        }

        /*
         Create chapter

        -succes crate chapter - sucssess
        -create with null object - ArgumentException
        -create chapter with wrang book id - KeyNotFoundExeption
        -create chapter with null title - ArgumentNullException
        -create chapter with null description - succsess
        -edit chapter - success
        -edit chapter with wrang book id - KeyNotFoundExeption
        -edit chapter with null title - ArgumentNullException
        -edit chapter with null description - succsess
        -delete chapter - succsess
        -delete chapter with wrang id - KeyNotFoundExeption
        -get one chapter - succsess
        -get one deleted chapter - KeyNotFoundExeption
        -get all user chapters
        -get chapters by book id
         */

        private BookGroup GetOneBookgroup(bool newly = false)
        {
            using (var _context = new BookContext(Options))
            {
                //Arange
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                if (_context.BookGroups.Any() && !newly)
                {
                    return _context.BookGroups.First();
                }
                BookGroup bookGroup = new BookGroup()
                {
                    Title = "new Title",
                    ShortDescription = "this is test short description",
                };
                _context.BookGroups.Add(bookGroup);
                _context.SaveChanges();
                return bookGroup;
            }
        }

        private Chapter GetOneChapter(bool isDeleted)
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                Chapter chapter = new Chapter()
                {
                    BookId = GetOneBook().Id,
                    IsDeleted = isDeleted,
                    ShortDescription = "test",
                    Title = "title"
                };
                _context.Chapters.Add(chapter);
                _context.SaveChanges();

                return chapter;
            }
        }
        private Domain.Enitties.Book.Book GetOneBook()
        {
            using (var _context = new BookContext(Options))
            {
                var bookgroup = GetOneBookgroup();
                var book = new Domain.Enitties.Book.Book()
                {
                    UserId = UserId,
                    BookGroupId = bookgroup.Id,
                    IsComplited = true,
                    IsDeleted = false,
                    //IsTextual = true,
                    Title = "test",
                    Description = "test",
                    BookType = Domain.Enums.BookType.Page,
                    CreateDate = DateTime.Now,
                    Difficultye = 8
                };
                _context.Add(book);
                _context.SaveChanges();
                return book;
            }
        }


        /*
         -succes crate chapter - sucssess
        -create with null object - ArgumentException
        -create chapter with wrang book id - KeyNotFoundExeption
        -create chapter with null title - ArgumentNullException
        -create chapter with null description - succsess
        */
        //-succes crate chapter - sucssess
        [Fact]
        public async Task CreateChapterAsync_sussessCreateChapter_ChapterResultAsync()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                var book = GetOneBook();
                ChapterRepository chapterRepository = new ChapterRepository(_context);

                //Act
                ChapterCreate chapterCreate = new ChapterCreate()
                {
                    BookId = book.Id,
                    ShortDescription = "test description",
                    Title = "test title"
                };
                var result = await chapterRepository.CreateChapterAsync(chapterCreate, UserId);

                //Assert
                result.Should().NotBeNull();
                result.ShortDescription.Should().Be(chapterCreate.ShortDescription);

            }
        }

        //-create with null object - ArgumentException
        [Fact]
        public async Task CreateChapterAsync_createChapterWithNull_ThrowNullExeption()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                ChapterRepository chapterRepository = new ChapterRepository(_context);
                //Act
                ChapterCreate chapterCreate = null;

                var result = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                //Assert
                await chapterRepository.CreateChapterAsync(chapterCreate, UserId));
                //result.Message.Should().Be("Chapter Objecct Is Null");
                result.ParamName.Should().Be("chapterCreate");
            }
        }

        //-create chapter with wrang book id - KeyNotFoundExeption
        [Fact]
        public async Task CreateChapterAsync_CreateChapterWithWrangBookID_ThrowNullExeption()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                ChapterRepository chapterRepository = new ChapterRepository(_context);

                //Act
                ChapterCreate chapterCreate = new ChapterCreate()
                {
                    BookId = Guid.NewGuid(),
                    ShortDescription = "test description",
                    Title = "test title",
                };

                var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                //Assert
                await chapterRepository.CreateChapterAsync(chapterCreate, UserId));

                result.Message.Should().Be("BookId Not Be Found");

            }
        }

        //-create chapter with null title - ArgumentNullException
        [Fact]
        public async Task CreateChapterAsync_CreateChapterWithNullTitle_ThrowNullExeption()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                ChapterRepository chapterRepository = new ChapterRepository(_context);
                var book = GetOneBook();
                //Act
                ChapterCreate chapterCreate = new ChapterCreate()
                {
                    BookId = book.Id,
                    ShortDescription = "test description",
                    // Title = "test title",
                };

                var result = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                //Assert
                await chapterRepository.CreateChapterAsync(chapterCreate, UserId));
                result.ParamName.Should().Be("Title");

            }
        }

        //-create chapter with null description - succsess
        [Fact]
        public async Task CreateChapterAsync_CreateChapterWithNullDescription_ChapterResultAsync()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                var book = GetOneBook();
                ChapterRepository chapterRepository = new ChapterRepository(_context);

                //Act
                ChapterCreate chapterCreate = new ChapterCreate()
                {
                    BookId = book.Id,
                    // ShortDescription = "",
                    Title = "test title",
                };
                var result = await chapterRepository.CreateChapterAsync(chapterCreate, UserId);

                //Assert
                result.Should().NotBeNull();
            }
        }

        /*
        -edit chapter - success
        -edit chapter with wrang book id - KeyNotFoundExeption
        -edit chapter with null title - ArgumentNullException
        -edit chapter with null description - succsess
        -edit chapter with wrang chapter id - KeyNotFoundExeption
        */
        //-edit chapter - success
        [Fact]
        public async Task EditChapterAsync_EditChapter_GetChapterResult()
        {

            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                var chapter = GetOneChapter(false);

                ChapterEdit chapterEdit = new ChapterEdit()
                {
                    Id = chapter.Id,
                    Title = "this is my edited title",
                    ShortDescription = "this is my favor",
                    BookId = chapter.BookId,

                };

                //Act
                ChapterRepository chapterRepository = new ChapterRepository(_context);
                var result = await chapterRepository.EditChapterAsync(chapterEdit, UserId);

                result.ShortDescription.Should().Be(chapterEdit.ShortDescription);
                result.ShortDescription.Should().NotBe(chapter.ShortDescription);

                result.Title.Should().Be(chapterEdit.Title);
                result.Title.Should().NotBe(chapter.Title);

                result.Id.Should().Be(chapterEdit.Id);
                result.Id.Should().Be(chapter.Id);

                result.BookId.Should().Be(chapterEdit.BookId);

            }

        }

        //-edit chapter with wrang book id - KeyNotFoundExeption
        [Fact]
        public async Task EditChapterAsync_EditChapterWithWrangBookId_ThrowKeyNotFoundExeption()
        {

            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                var chapter = GetOneChapter(false);

                ChapterEdit chapterEdit = new ChapterEdit()
                {
                    Id = chapter.Id,
                    Title = "this is my edited title",
                    ShortDescription = "this is my favor",
                    BookId = Guid.NewGuid(),

                };

                //Act
                ChapterRepository chapterRepository = new ChapterRepository(_context);

                var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await chapterRepository.EditChapterAsync(chapterEdit, UserId));

                result.Message.Should().Be("Bookid Can Not Be Found");
            }

        }


        //-edit chapter with null title - ArgumentNullException
        [Fact]
        public async Task EditChapterAsync_EditChapterWithNullTitle_ThrowArgumentNullException()
        {

            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                var chapter = GetOneChapter(false);

                ChapterEdit chapterEdit = new ChapterEdit()
                {
                    Id = chapter.Id,
                    // Title = "this is my edited title",
                    ShortDescription = "this is my favor",
                    BookId = chapter.BookId,

                };

                //Act
                ChapterRepository chapterRepository = new ChapterRepository(_context);

                var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await chapterRepository.EditChapterAsync(chapterEdit, UserId));

                result.ParamName.Should().Be("Title");
            }

        }

        //-edit chapter with null description - succsess
        [Fact]
        public async Task EditChapterAsync_EditChapterWithNullDescriptioun_GetSuccessChapterResult()
        {

            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                var chapter = GetOneChapter(false);

                ChapterEdit chapterEdit = new ChapterEdit()
                {
                    Id = chapter.Id,
                    Title = "this is my edited title",
                    //ShortDescription = "this is my favor",
                    BookId = chapter.BookId,

                };

                //Act
                ChapterRepository chapterRepository = new ChapterRepository(_context);
                var result = await chapterRepository.EditChapterAsync(chapterEdit, UserId);

                result.ShortDescription.Should().Be(chapterEdit.ShortDescription);
                result.ShortDescription.Should().NotBe(chapter.ShortDescription);

                result.Title.Should().Be(chapterEdit.Title);
                result.Title.Should().NotBe(chapter.Title);

                result.Id.Should().Be(chapterEdit.Id);
                result.Id.Should().Be(chapter.Id);

                result.BookId.Should().Be(chapterEdit.BookId);

            }

        }

        //-edit chapter with wrang chapter id - KeyNotFoundExeption
        [Fact]
        public async Task EditChapterAsync_EditChapterWithWrangChapterId_ThrowKeyNotFoundExeption()
        {

            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                var chapter = GetOneChapter(false);

                ChapterEdit chapterEdit = new ChapterEdit()
                {
                    Id = Guid.NewGuid(),
                    Title = "this is my edited title",
                    ShortDescription = "this is my favor",
                    BookId = chapter.BookId,

                };

                //Act
                ChapterRepository chapterRepository = new ChapterRepository(_context);
                var result = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                //Assert
                 await chapterRepository.EditChapterAsync(chapterEdit, UserId));

                result.Message.Should().Be("Chapter id was not found");

            }

        }

        /*
         -delete chapter - succsess
         -delete chapter with wrang id - KeyNotFoundExeption
        */

        //-delete chapter - succsess
        [Fact]
        public async Task SafeDeleteChapter_DeleteChapter_SuccessDelete()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                var chapter = GetOneChapter(false);

                ChapterRepository chapterRepository = new ChapterRepository(_context);

                //Act
                chapterRepository.SafeDeleteChapter(chapter.Id,UserId);

                //Assert
                Assert.Equal((await _context.Chapters.FirstOrDefaultAsync(a => a.Id == chapter.Id)).IsDeleted, true);

            }
        }

        //-delete chapter with wrang id - KeyNotFoundExeption
        [Fact]
        public void SafeDeleteChapter_DeleteChapterWithWrangChapterId_SuccessDelete()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                //  var chapter = GetOneChapter(false);

                ChapterRepository chapterRepository = new ChapterRepository(_context);
                //Act                
                //Assert
                var result = Assert.Throws<KeyNotFoundException>(() => chapterRepository.SafeDeleteChapter(Guid.NewGuid(), UserId));
                result.Message.Should().Be("Chapter Id Not Valid");
            }
        }

        /*
        -get one chapter - succsess
        -get one deleted chapter - KeyNotFoundExeption
        -get one chapter where when user id is not match - KeyNotFoundExeption
        -get all user chapters
        */


        //-get one chapter - succsess
        [Fact]
        public async void GetChapterByIdAsync_getOneChapterById_GetSuccessFulyChapter()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                var chapter = GetOneChapter(false);

                ChapterRepository chapterRepository = new ChapterRepository(_context);
                //Act                
                var mychapter = await chapterRepository.GetChapterByIdAsync(chapter.Id, UserId);
                //Assert
                mychapter.Should().NotBeNull();

            }
        }

        //-get one deleted chapter - KeyNotFoundExeption
        [Fact]
        public async void GetChapterByIdAsync_GetWithWrangId_ReturnNull()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                var chapter = GetOneChapter(false);

                ChapterRepository chapterRepository = new ChapterRepository(_context);
                //Act                
                var mychapter = await chapterRepository.GetChapterByIdAsync(Guid.NewGuid(), UserId);
                //Assert
                mychapter.Should().BeNull();

            }
        }

        //-get one chapter where when user id is not match
        [Fact]
        public async void GetChapterByIdAsync_GetWithWrangUserId_ReturnNull()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                var chapter = GetOneChapter(false);

                ChapterRepository chapterRepository = new ChapterRepository(_context);
                //Act                
                var mychapter = await chapterRepository.GetChapterByIdAsync(chapter.Id, UserId + "ab");
                //Assert
                mychapter.Should().BeNull();

            }
        }


        //-get user chapters add 60 chapter and get 50 from repository
        [Fact]
        public async void GetChaptersAsync_GetSuccessFuly50CountChapterList_Get50ChapterList()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                int len = 60;
                for (int i = 0; i < len; i++)
                {
                    GetOneChapter(false);
                }

                ChapterRepository chapterRepository = new ChapterRepository(_context);
                //Act                
                var mychapter = await chapterRepository.GetChaptersAsync(UserId, 50);
                //Assert
                mychapter.Should().NotBeNull();
                mychapter.Count.Should().Be(50);

            }
        }
        //-get user chapters. add 60 chapter and make page with 50 in with page and send request to get page 2 and get only 10 chapter left
        [Fact]
        public async void GetChaptersAsync_Make60ChapterAndLimitPerPageSet50AndGetPage2_Get10ChapterList()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                int len = 60;
                for (int i = 0; i < len; i++)
                {
                    GetOneChapter(false);
                }

                ChapterRepository chapterRepository = new ChapterRepository(_context);
                //Act                
                var mychapter = await chapterRepository.GetChaptersAsync(UserId, 50, 2);
                //Assert
                mychapter.Should().NotBeNull();
                mychapter.Count.Should().Be(10);

            }
        }


        //-get user chapters. add 60 chapter and try to get 10 chapter with wran user id
        [Fact]
        public async void GetChaptersAsync_Make60ChapteraAndGetChapterFromWrangUser_ReturnNull()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                int len = 60;
                for (int i = 0; i < len; i++)
                {
                    GetOneChapter(false);
                }

                ChapterRepository chapterRepository = new ChapterRepository(_context);
                //Act                
                var mychapter = await chapterRepository.GetChaptersAsync(UserId + "a", 50, 2);
                //Assert
                mychapter.Should().BeEmpty();

            }
        }


        //-get chapters by book id
        [Fact]
        public async void GetChaptersByBookIdAsync_GetSuccessFulyChaptersForSpesificBook_GetBooksChapter()
        {
            Domain.Enitties.Book.Book book1, book2;

            Domain.Enitties.Book.Chapter chapter1_1, chapter1_2, chapter1_3;

            Domain.Enitties.Book.Chapter chapter2_1, chapter2_2, chapter2_3;

            //Arange
            book1 = GetOneBook();
            book2 = GetOneBook();
            //For Book 1
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                chapter1_1 = new Chapter()
                {
                    BookId = book1.Id,
                    IsDeleted = false,
                    ShortDescription = "test",
                    Title = "title"
                };
                _context.Chapters.Add(chapter1_1);
                _context.SaveChanges();
            }
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                chapter1_2 = new Chapter()
                {
                    BookId = book1.Id,
                    IsDeleted = false,
                    ShortDescription = "test",
                    Title = "title"
                };
                _context.Chapters.Add(chapter1_2);
                _context.SaveChanges();
            }
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                chapter1_3 = new Chapter()
                {
                    BookId = book1.Id,
                    IsDeleted = false,
                    ShortDescription = "test",
                    Title = "title"
                };
                _context.Chapters.Add(chapter1_3);
                _context.SaveChanges();
            }
            //For Book 2
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                chapter2_1 = new Chapter()
                {
                    BookId = book2.Id,
                    IsDeleted = false,
                    ShortDescription = "test",
                    Title = "title"
                };
                _context.Chapters.Add(chapter2_1);
                _context.SaveChanges();
            }
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                chapter2_2 = new Chapter()
                {
                    BookId = book2.Id,
                    IsDeleted = false,
                    ShortDescription = "test",
                    Title = "title"
                };
                _context.Chapters.Add(chapter2_2);
                _context.SaveChanges();
            }
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                chapter2_3 = new Chapter()
                {
                    BookId = book2.Id,
                    IsDeleted = false,
                    ShortDescription = "test",
                    Title = "title"
                };
                _context.Chapters.Add(chapter2_3);
                _context.SaveChanges();
            }


            //Act
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                ChapterRepository chapterRepository = new ChapterRepository(_context);

                var book1Chapters = await chapterRepository.GetChaptersByBookIdAsync(book1.Id, UserId);
                var book2Chapters = await chapterRepository.GetChaptersByBookIdAsync(book2.Id, UserId);

                //Assert

                List<Guid> book1ChapterIds = new List<Guid>
                {
                    chapter1_1.Id,
                    chapter1_2.Id,
                    chapter1_3.Id
                };

                List<Guid> book2ChapterIds = new List<Guid>
                {
                    chapter2_1.Id,
                    chapter2_2.Id,
                    chapter2_3.Id
                };

                // 'book1Chapters' should contains book 1 chapters id and don't have ids from book 2 chapters

                book1Chapters.Should().Contain(a=> book1ChapterIds.Contains(a.Id));
                book1Chapters.Should().NotContain(a => book2ChapterIds.Contains(a.Id));


                // 'book2Chapters' should contains book 2 chapters id and don't have ids from book 1 chapters

                book2Chapters.Should().Contain(a => book2ChapterIds.Contains(a.Id));
                book2Chapters.Should().NotContain(a => book1ChapterIds.Contains(a.Id));
            }
        }
    }
}
