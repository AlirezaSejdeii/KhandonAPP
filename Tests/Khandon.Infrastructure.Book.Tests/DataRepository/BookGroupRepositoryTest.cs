using FluentAssertions;
using Khandon.Infrastructure.Book.DataContext;
using Khandon.Infrastructure.Book.DataRepository;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Resposne;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Khandon.Infrastructure.Book.Tests.DataRepository
{
    public class BookGroupRepositoryTest
    {
        private readonly SqliteConnectionStringBuilder ConnectionStringBuilder;
        private readonly SqliteConnection Connection;
        private readonly DbContextOptions<BookContext> Options;

        public BookGroupRepositoryTest()
        {
            ConnectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = ":memory:" };
            Connection = new SqliteConnection(ConnectionStringBuilder.ToString());
            Options = new DbContextOptionsBuilder<BookContext>().UseSqlite(Connection).Options;
        }

        [Fact]
        public void BookGroupRepository_CreateBookGroup()
        {
            //Act
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                BookGroupCreate bookGroupCreate = new Shared.Dto.Request.BookGroupCreate()
                {
                    Title = "new Title",
                    ShortDescription = "this is test short description",
                };
                BookGroupResult result = bookGroupRepository.CreateBookgroup(bookGroupCreate);
                //Assert

                Assert.NotNull(result);
                Assert.NotEqual(result.Id, 0);

                Assert.Equal(bookGroupCreate.ShortDescription, result.ShortDescription);
                Assert.Equal(bookGroupCreate.Title, result.Title);
            }


        }

        [Fact]
        public void BookGroupRepository_CreateBookGroupWithoutTitle_shouldReturnExeption()
        {
            //Act
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                BookGroupCreate bookGroupCreate = new Shared.Dto.Request.BookGroupCreate()
                {
                    //Title = "new Title",
                    ShortDescription = "this is test short description",
                };

                //Assert

                Assert.Throws(typeof(Exception), (() => bookGroupRepository.CreateBookgroup(bookGroupCreate)));
            }
        }

        [Fact]
        public void BookGroupRepository_CreateBookGroupWithoutDescription_shouldReturnExeption()
        {
            //Act
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                BookGroupCreate bookGroupCreate = new Shared.Dto.Request.BookGroupCreate()
                {
                    Title = "new Title",
                    //ShortDescription = "this is test short description",
                };

                //Assert

                Assert.Throws(typeof(Exception), (() => bookGroupRepository.CreateBookgroup(bookGroupCreate)));
            }
        }

        [Fact]
        public void EditBookGroup_EditBookGroup_shouldReturnUpdatedBook()
        {
            int bookAddedId = 0;
            //Act
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                BookGroupCreate bookGroupCreate = new Shared.Dto.Request.BookGroupCreate()
                {
                    Title = "new Title",
                    ShortDescription = "this is test short description",
                };

                var result = bookGroupRepository.CreateBookgroup(bookGroupCreate);
                bookAddedId = result.Id;
            }
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                BookGroupEdit bookGrouptoEdit = new Shared.Dto.Request.BookGroupEdit()
                {
                    Id = bookAddedId,
                    Title = "title Edited",
                    ShortDescription = "short Edited",
                };

                var result = bookGroupRepository.EditBookGroup(bookGrouptoEdit);


                //Assert
                result.ShortDescription.Should().Be("short Edited");
                result.Should().NotBe(null);
                result.Title.Should().Be("title Edited");
                bookAddedId.Should().Be(result.Id);
            }
        }

        [Fact]
        public void EditBookGroup_EditBookGroupWith0Id_shouldReturnExeption()
        {
            int bookAddedId = 0;
            //Act
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                BookGroupCreate bookGroupCreate = new Shared.Dto.Request.BookGroupCreate()
                {
                    Title = "new Title",
                    ShortDescription = "this is test short description",
                };

                var result = bookGroupRepository.CreateBookgroup(bookGroupCreate);
                bookAddedId = result.Id;
            }
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                BookGroupEdit bookGrouptoEdit = new Shared.Dto.Request.BookGroupEdit()
                {
                    Id = 0,
                    Title = "title Edited",
                    ShortDescription = "short Edited",
                };

                //Assert
                Assert.Throws<Exception>(() => bookGroupRepository.EditBookGroup(bookGrouptoEdit));

            }
        }

        [Fact]
        public void GetBookById_getBookgroupById_GetBookgroup()
        {
            int bookgroupId = 0;
            //Act
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                BookGroupCreate bookGroupCreate = new Shared.Dto.Request.BookGroupCreate()
                {
                    Title = "new Title",
                    ShortDescription = "this is test short description",
                };
                var result = bookGroupRepository.CreateBookgroup(bookGroupCreate);

                bookgroupId = result.Id;
            }
            //assert
            using (var _context = new BookContext(Options))
            {
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                var result = bookGroupRepository.GetBookById(bookgroupId);
                result.Should().NotBeNull();
                result.Id.Should().NotBe(0);
                result.Id.Should().Be(bookgroupId);
            }
        }

        [Fact]
        public void GetBookById_GetBookgroupWithId0_GetNull()
        {
            //Act
            //assert
            using (var _context = new BookContext(Options))
            {
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                var result = bookGroupRepository.GetBookById(0);
                result.Should().BeNull();
            }
        }

        [Fact]
        public void GetBookGroups_GetListOfBookGroups_BookGroupList()
        {
            //Act
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                BookGroupCreate bookGroupCreate = new Shared.Dto.Request.BookGroupCreate()
                {
                    Title = "new Title",
                    ShortDescription = "this is test short description",
                };
                var result = bookGroupRepository.CreateBookgroup(bookGroupCreate);

            }
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                BookGroupCreate bookGroupCreate = new Shared.Dto.Request.BookGroupCreate()
                {
                    Title = "new Title",
                    ShortDescription = "this is test short description",
                };
                var result = bookGroupRepository.CreateBookgroup(bookGroupCreate);
            }
            //assert
            using (var _context = new BookContext(Options))
            {
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                var result = bookGroupRepository.GetBookGroups();
                result.Should().NotBeNull();
                result.Count.Should().BeGreaterThan(1);
            }
        }

        [Fact]
        public void GetBookGroups_RemoveAllBookGroupsAndReturnEmptyBookGroup_GetEmptyListOfBookGroup()
        {
            //Act
            using (var _context = new BookContext(Options))
            {
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                _context.RemoveRange(_context.BookGroups);
                _context.SaveChanges();
            }
            //assert
            using (var _context = new BookContext(Options))
            {
                BookGroupRepository bookGroupRepository = new BookGroupRepository(_context);

                var result = bookGroupRepository.GetBookGroups();
                result.Should().BeEmpty();

            }
        }
    }
}
