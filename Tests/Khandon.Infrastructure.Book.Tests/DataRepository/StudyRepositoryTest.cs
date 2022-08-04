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
using System.Threading.Tasks;
using Xunit;

namespace Khandon.Infrastructure.Book.Tests.DataRepository
{
    public class StudyRepositoryTest
    {
        private readonly SqliteConnectionStringBuilder ConnectionStringBuilder;
        private readonly SqliteConnection Connection;
        private readonly DbContextOptions<BookContext> Options;
        private readonly string UserId;
        public StudyRepositoryTest()
        {
            ConnectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = ":memory:" };
            Connection = new SqliteConnection(ConnectionStringBuilder.ToString());
            Options = new DbContextOptionsBuilder<BookContext>().UseSqlite(Connection).Options;
            UserId = Guid.NewGuid().ToString();
        }

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

        private Domain.Enitties.Book.Book GetOneBook(int bookGroupId = 0)
        {
            using (var _context = new BookContext(Options))
            {
                var bookgroup = GetOneBookgroup();
                Domain.Enitties.Book.Book book = null;
                if (bookGroupId != 0)
                {
                    book = new Domain.Enitties.Book.Book()
                    {
                        UserId = UserId,
                        BookGroupId = bookGroupId,
                        IsComplited = true,
                        IsDeleted = false,
                        //IsTextual = true,
                        Title = "test",
                        Description = "test",
                        BookType = Domain.Enums.BookType.Page,
                        CreateDate = DateTime.Now,
                        Difficultye = 8
                    };
                }
                else
                {
                    book = new Domain.Enitties.Book.Book()
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
                }
                _context.Add(book);
                _context.SaveChanges();
                return book;
            }
        }

        private Chapter GetOneChapter(bool isDeleted, Guid bookId)
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                //Arange
                Chapter chapter = null;
                if (bookId.Equals(Guid.Empty))
                {

                    chapter = new Chapter()
                    {
                        BookId = GetOneBook().Id,
                        IsDeleted = isDeleted,
                        ShortDescription = "test",
                        Title = "title"
                    };
                }
                else
                {
                    chapter = new Chapter()
                    {
                        BookId = bookId,
                        IsDeleted = isDeleted,
                        ShortDescription = "test",
                        Title = "title"
                    };
                }
                _context.Chapters.Add(chapter);
                _context.SaveChanges();

                return chapter;
            }
        }

        private Study GetOneStudy()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                var chapter = GetOneChapter(false, Guid.Empty);
                Study study = new Study()
                {
                    ChapterId = chapter.Id,
                    IsDeleted = false,
                    Title = "test",
                    Length = 12,
                    ShortDescription = "description",
                    CreateDate = DateTime.Now
                };
                _context.Studies.Add(study);
                _context.SaveChanges();

                return study;
            }
        }


        /*
        - create study - success
        - create study with wrang chapter id - KeyNotFoundExeption
        - create study with null title - ArgumentNullExeption
        - create study with null description - success
        - create study with mines(-) value for LENGTH - ArgumentExeption
        */

        //- create study - success
        [Fact]
        public async Task CreateStudyAsync_CreateSuccesFulyStudy_StudyResult()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                //Arange

                var chapter = GetOneChapter(false, Guid.Empty);

                StudyRepository studyRepository = new StudyRepository(_context);

                StudyCreate studyCreate = new StudyCreate()
                {
                    Length = 2,
                    Title = "title",
                    ShortDescription = "test",
                    ChapterId = chapter.Id
                };
                //Act
                var result = await studyRepository.CreateStudyAsync(studyCreate, UserId);

                //Asert
                result.Should().NotBeNull();
            }
        }
        //- create study with wrang chapter id - KeyNotFoundExeption
        [Fact]
        public async Task CreateStudyAsync_CreateStudyWithWrangChapterId_ThrowKeyNotFoundExeption()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                //Arange

                //var chapter = GetOneChapter(false);

                StudyRepository studyRepository = new StudyRepository(_context);

                StudyCreate studyCreate = new StudyCreate()
                {
                    Length = 2,
                    Title = "title",
                    ShortDescription = "test",
                    ChapterId = Guid.NewGuid()
                };
                //Act
                var result = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                //Assert
                    studyRepository.CreateStudyAsync(studyCreate, UserId));

                result.Message.Should().Be("Chapter Id Invalid");
            }
        }
        //- create study with null title - ArgumentNullExeption
        [Fact]
        public async Task CreateStudyAsync_CreateStudyWithNullTitle_throwArgumentNullExeption()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                //Arange

                var chapter = GetOneChapter(false, Guid.Empty);

                StudyRepository studyRepository = new StudyRepository(_context);

                StudyCreate studyCreate = new StudyCreate()
                {
                    Length = 2,
                    //  Title = "title",
                    ShortDescription = "test",
                    ChapterId = chapter.Id
                };
                //Act
                var result = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                      //Assert
                      studyRepository.CreateStudyAsync(studyCreate, UserId));
                result.ParamName.Should().Be("Title");

            }
        }
        //- create study with null description - success
        [Fact]
        public async Task CreateStudyAsync_CreateSuccesFulyStudyWithNullDescription_StudyResult()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                //Arange

                var chapter = GetOneChapter(false, Guid.Empty);

                StudyRepository studyRepository = new StudyRepository(_context);

                StudyCreate studyCreate = new StudyCreate()
                {
                    Length = 2,
                    Title = "title",
                    //   ShortDescription = "test",
                    ChapterId = chapter.Id
                };
                //Act
                var result = await studyRepository.CreateStudyAsync(studyCreate, UserId);

                //Asert
                result.Should().NotBeNull();
            }
        }
        //- create study with mines(-) value for LENGTH - ArgumentExeption
        [Fact]
        public async Task CreateStudyAsync_CreateStudyWithMinesLength_ThrowArgumentExeption()
        {
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                //Arange

                var chapter = GetOneChapter(false, Guid.Empty);

                StudyRepository studyRepository = new StudyRepository(_context);

                StudyCreate studyCreate = new StudyCreate()
                {
                    Length = -45,
                    Title = "title",
                    ShortDescription = "test",
                    ChapterId = chapter.Id
                };
                //Act
                var result = await Assert.ThrowsAsync<ArgumentException>(() =>
                //Assert
                    studyRepository.CreateStudyAsync(studyCreate, UserId));

                result.Message.Should().Be("Length Is Invalid");
            }
        }

        /*
        - edit study - success
        - edit study with another chapter id (user can edit chapter just for current chapter id) - ArgumentExeption
        - edit study with null title - ArgumentNullExeption
        - edit study with null description - success
        - edit study with mines(-) value for LENGTH - ArgumentExeption
         */

        //- edit study - success
        [Fact]
        public async Task EditStudyAsync_EditSuccessfulyStudy_StudyResult()
        {
            using (var _context = new BookContext(Options))
            {
                //Arange
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                var study = GetOneStudy();

                var studyRepo = new StudyRepository(_context);

                var studyEdit = new StudyEdit()
                {
                    Length = 467,
                    Title = "it's my onwne title",
                    ShortDescription = "it's new edited",
                    Id = study.Id
                };

                //Act
                var result = await studyRepo.EditStudyAsync(studyEdit, UserId);
                //Assert
                result.Id.Should().Be(study.Id);
                result.ChapterId.Should().Be(study.ChapterId);
                result.Length.Should().Be(467);
                result.Length.Should().NotBe(study.Length);
                result.Title.Should().Be("it's my onwne title");
                result.ShortDescription.Should().Be("it's new edited");

            }
        }
        //- edit study with null title - ArgumentNullExeption
        [Fact]
        public async Task EditStudyAsync_EditStudyWithNullTitle_throwArgumentNullExeption()
        {
            using (var _context = new BookContext(Options))
            {
                //Arange
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                var study = GetOneStudy();

                var studyRepo = new StudyRepository(_context);

                var studyEdit = new StudyEdit()
                {
                    Length = 467,
                    ShortDescription = "it's new edited",
                    // Title = "it's my onwne title",
                    Id = study.Id
                };

                //Act
                //Act
                var result = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                //Assert
                studyRepo.EditStudyAsync(studyEdit, UserId));
                result.ParamName.Should().Be("Title");

            }
        }

        //- edit study with null description - success
        [Fact]
        public async Task EditStudyAsync_EditSuccessfulyStudyWithNullDescription_StudyResult()
        {
            using (var _context = new BookContext(Options))
            {
                //Arange
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                var study = GetOneStudy();

                var studyRepo = new StudyRepository(_context);

                var studyEdit = new StudyEdit()
                {
                    Length = 467,
                    // ShortDescription = "it's new edited",
                    Title = "it's my onwne title",
                    Id = study.Id
                };

                //Act
                var result = await studyRepo.EditStudyAsync(studyEdit, UserId);
                //Assert
                result.Id.Should().Be(study.Id);
                result.ChapterId.Should().Be(study.ChapterId);
                result.Length.Should().Be(467);
                result.Length.Should().NotBe(study.Length);
                result.Title.Should().Be("it's my onwne title");
            }
        }

        //- edit study with mines(-) value for LENGTH - ArgumentExeption
        [Fact]
        public async Task EditStudyAsync_EditStudyWithMinesLength_throwArgumentExeption()
        {
            using (var _context = new BookContext(Options))
            {
                //Arange
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                var study = GetOneStudy();

                var studyRepo = new StudyRepository(_context);

                var studyEdit = new StudyEdit()
                {
                    Length = -13,
                    ShortDescription = "it's new edited",
                    Title = "it's my onwne title",
                    Id = study.Id
                };

                //Act
                //Act
                var result = await Assert.ThrowsAsync<ArgumentException>(() =>
                //Assert
                studyRepo.EditStudyAsync(studyEdit, UserId));
                result.Message.Should().Be("Length Is Invalid");

            }
        }

        /*
        - safe delete study - success
        - safe delete study with wrang study Id
         */

        //- safe delete study - success
        [Fact]
        public async Task SafeDeleteStudyAsync_SuccessfulySafeDeleteStudy_ReturnTrue()
        {

            using (var _context = new BookContext(Options))
            {
                //Arange
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                var study = GetOneStudy();

                var studyRepo = new StudyRepository(_context);

                Assert.True(await studyRepo.SafeDeleteStudyAsync(UserId, study.Id));

            }
        }

        [Fact]
        public async Task SafeDeleteStudyAsync_DeleteStudyWithWrangId_ReturnFalse()
        {
            using (var _context = new BookContext(Options))
            {
                //Arange
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                var study = GetOneStudy();

                var studyRepo = new StudyRepository(_context);

                Assert.False(await studyRepo.SafeDeleteStudyAsync(UserId, Guid.NewGuid()));

            }
        }

        /*
        - get studies with group id
        - get studies without group id
        - get studies with pagenation and limtation per page
        - get studies with pagenation and over(+100) limtation per page
         */

        //- get studies with group id - all studies is 12 - my spesfic group id has 5 study
        [Fact]
        public async Task GetStudyListAsync_GetStudiesWithSpesficGroupId_ListStudiesContaing5Study()
        {

            using (var _context = new BookContext(Options))
            {
                var bookgroupId1 = GetOneBookgroup(true);
                var bookgroupId2 = GetOneBookgroup(true);
                //Arange

                //with bookgroupId1
                for (int i = 0; i < 7; i++)
                {
                    using (var _context1 = new BookContext(Options))
                    {
                        _context1.Database.OpenConnection();
                        _context1.Database.EnsureCreated();
                        Study study = new Study()
                        {
                            ChapterId = GetOneChapter(false, GetOneBook(bookgroupId1.Id).Id).Id,
                            IsDeleted = false,
                            Title = "test",
                            Length = 12,
                            ShortDescription = "description",
                            CreateDate = DateTime.Now
                        };
                        _context1.Studies.Add(study);
                        _context1.SaveChanges();
                    }
                }


                //with bookgroupId2
                for (int i = 0; i < 5; i++)
                {
                    using (var _context2 = new BookContext(Options))
                    {
                        _context2.Database.OpenConnection();
                        _context2.Database.EnsureCreated();
                        Study study = new Study()
                        {
                            ChapterId = GetOneChapter(false, GetOneBook(bookgroupId2.Id).Id).Id,
                            IsDeleted = false,
                            Title = "test",
                            Length = 12,
                            ShortDescription = "description",
                            CreateDate = DateTime.Now
                        };
                        _context2.Studies.Add(study);
                        _context2.SaveChanges();
                    }
                }
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                var studyRepo = new StudyRepository(_context);

                //Act
                var result1 = await studyRepo.GetStudyListAsync(UserId, new StudyGet()
                {
                    BookGroupId = bookgroupId1.Id,
                });
                var result2 = await studyRepo.GetStudyListAsync(UserId, new StudyGet()
                {
                    BookGroupId = bookgroupId2.Id,
                });


                //Assert
                result1.Count.Should().Be(7);
                result1.Should().NotContain(a => result2.Contains(a));

                result2.Count.Should().Be(5);
                result2.Should().NotContain(a => result1.Contains(a));
            }

        }
        //- get studies without group id - all studies is 12
        [Fact]
        public async Task GetStudyListAsync_GetStudies_ListStudiesContains12Study()
        {
            //Assert

            for (int i = 0; i < 12; i++)
            {
                GetOneStudy();
            }
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                var studyRepo = new StudyRepository(_context);

                //Act
                var result = await studyRepo.GetStudyListAsync(UserId, new StudyGet()
                {
                    Page = 1,
                    Limit = 20
                });
                result.Count.Should().Be(12);
            }
        }

        //- get studies with pagenation and limtation per page - all studies count is 44 - paginate 9 perpage and get page 2
        //should get page (10,11,12,13,14,15,16,17,18)
        [Fact]
        public async Task GetStudyListAsync_GetStudies9PerPageAndGetPage2_return9Studies()
        {
            List<Study> studyResults = new List<Study>();
            for (int i = 0; i < 44; i++)
            {
                studyResults.Add(GetOneStudy());
            }
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                var studyRepo = new StudyRepository(_context);

                //Act
                var result = await studyRepo.GetStudyListAsync(UserId, new StudyGet()
                {
                    Page = 2,
                    Limit = 9
                });
                result.Count.Should().Be(9);
                foreach (var item in result)
                {
                    Assert.True(studyResults.FindIndex(a => a.Id == item.Id) >= 9 &&

                        studyResults.FindIndex(a => a.Id == item.Id) <= 18);
                }
            }
        }
        //- get studies with pagenation and over(+100) limtation per page - we save 110 study and want to get all of them in one requst and should return maximum 100 per page for limit
        [Fact]
        public async Task GetStudyListAsync_allStudiesIs110AndWeWantGetAllOfThem_ShouldReturnOnly100()
        {
            for (int i = 0; i <= 110; i++)
            {
                GetOneStudy();
            }
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                var studyRepo = new StudyRepository(_context);

                //Act
                var result = await studyRepo.GetStudyListAsync(UserId, new StudyGet()
                {
                    Page = 1,
                    Limit = 110
                });
                result.Count.Should().Be(100);
            }
        }
    }
}
