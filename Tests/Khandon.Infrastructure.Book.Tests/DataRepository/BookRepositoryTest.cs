using FluentAssertions;
using Khandon.Domain.Enitties.Book;
using Khandon.Infrastructure.Book.DataContext;
using Khandon.Infrastructure.Book.DataRepository;
using Khandon.Shared.Dto.Enums;
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
    public class BookRepositoryTest
    {
        private readonly SqliteConnectionStringBuilder ConnectionStringBuilder;
        private readonly SqliteConnection Connection;
        private readonly DbContextOptions<BookContext> Options;
        private readonly string UserId;
        public BookRepositoryTest()
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

        [Fact]
        public async void CreateBook_Create_aBook_SuccesCreateBook()
        {
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookRepository bookRepo = new BookRepository(_context);

                var bookgroup = GetOneBookgroup();
                BookCreate bookCreate = new BookCreate()
                {
                    BookGroupId = bookgroup.Id,
                    Title = "test Title",
                    BookType = BookTypeEnum.Page,
                 //   Description = "new Description",
                    //IsTextual = false,
                    Difficultye = 9
                };
                //act
                var result = await bookRepo.CreateBookAsync(bookCreate, UserId);

                result.Should().NotBeNull();
                result.Id.Should().NotBeEmpty();
                result.Title.Should().Be("test Title");
          //      result.Description.Should().Be("new Description");
                result.Difficultye.Should().Be(9);
            }
        }

        [Fact]
        public async void CreateBook_CreateBookWIthNullTitle_ShouldThrowExeption()
        {
            using (var _context = new BookContext(Options))
            {
                //Arrnage         
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookRepository bookRepo = new BookRepository(_context);

                var bookgroup = GetOneBookgroup();
                BookCreate bookCreate = new BookCreate()
                {
                    BookGroupId = bookgroup.Id,
                    //Title = "test Title",
                    BookType = BookTypeEnum.Page,
                    Description = "new Description",
                    //IsTextual = false,
                    Difficultye = 9
                };
                //act
                await Assert.ThrowsAsync<ArgumentNullException>(
                         async () =>//Assert
                  await bookRepo.CreateBookAsync(bookCreate, UserId));
            }
        }

        [Fact]
        public async void CreateBook_CreateWithoutBookGroupid_ShouldThrowExeption()
        {
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookRepository bookRepo = new BookRepository(_context);

                var bookgroup = GetOneBookgroup();
                BookCreate bookCreate = new BookCreate()
                {
                    //BookGroupId = bookgroup.Id,
                    Title = "test Title",
                    BookType = BookTypeEnum.Page,
                    Description = "new Description",
                    //IsTextual = false,
                    Difficultye = 9
                };
                //act
                await Assert.ThrowsAsync<ArgumentNullException>(
                        async () =>//Assert
                  await bookRepo.CreateBookAsync(bookCreate, UserId));
            }
        }

        [Fact]
        public async void CreateBook_CreateBookWith_UpTo10ValueFor_Dificalty_ShouldThrowExeption()
        {
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookRepository bookRepo = new BookRepository(_context);

                var bookgroup = GetOneBookgroup();
                BookCreate bookCreate = new BookCreate()
                {
                    //BookGroupId = bookgroup.Id,
                    Title = "test Title",
                    BookType = BookTypeEnum.Page,
                    Description = "new Description",
                    //IsTextual = false,
                    Difficultye = 95 //dificalty Must be 1-10
                };
                //act
                await Assert.ThrowsAsync<ArgumentNullException>(
                       async () =>//Assert
                  await bookRepo.CreateBookAsync(bookCreate, UserId));
            }
        }

        [Fact]
        public async void CreateBook_CreateBookWithNullObject_shouldThrowExeption()
        {
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();
                BookRepository bookRepo = new BookRepository(_context);

                var bookgroup = GetOneBookgroup();
                BookCreate bookCreate = null;
                //act
                await Assert.ThrowsAsync<ArgumentNullException>(
                       async () =>//Assert
               await bookRepo.CreateBookAsync(bookCreate, UserId));
            }
        }


        [Fact]
        public async void EditBook_SuccessfulyEditBook_EditedBook()
        {
            Domain.Enitties.Book.Book book = null;
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);
                var bookgroup = GetOneBookgroup();
                book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
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
            }


            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                if (book is not null)
                {
                    BookEdit bookEdit = new BookEdit()
                    {
                        Id = book.Id,
                        BookGroupId = book.BookGroupId,
                        //IsTextual = true,
                        Title = "edited title",
                        Description = "edited Description for test",
                        BookType = BookTypeEnum.Minut,
                        Difficultye = 5,
                        IsComplited = true,
                    };

                    BookRepository bookRepo = new BookRepository(_context);

                    var result = await bookRepo.EditBookAsync(bookEdit, book.Id, UserId);

                    result.Should().NotBeNull();
                    result.Id.CompareTo(book.Id);
                    result.Title.Should().Be("edited title");
                    result.Description.Should().Be("edited Description for test");
                    result.CreateDate.Should().Be(book.CreateDate);


                }
                else
                {
                    //error occers
                    //database error
                    Assert.False(true);
                }
            }
        }

        [Fact]
        public async void EditBook_EditAnSafeDeletedBook_throwExeption()
        {
            Domain.Enitties.Book.Book book = null;
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);
                var bookgroup = GetOneBookgroup();
                book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    BookGroupId = bookgroup.Id,
                    IsComplited = true,
                    IsDeleted = true,
                    //IsTextual = true,
                    Title = "test",
                    Description = "test",
                    BookType = Domain.Enums.BookType.Page,
                    CreateDate = DateTime.Now,
                    Difficultye = 8
                };
                _context.Add(book);
                _context.SaveChanges();
            }


            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                if (book is not null)
                {
                    BookEdit bookEdit = new BookEdit()
                    {
                        Id = book.Id,
                        BookGroupId = book.BookGroupId,
                        //IsTextual = true,
                        Title = "edited title",
                        Description = "edited Description for test",
                        BookType = BookTypeEnum.Minut,
                        Difficultye = 5,
                        IsComplited = true,
                    };

                    BookRepository bookRepo = new BookRepository(_context);

                    var result = await Assert.ThrowsAsync<KeyNotFoundException>(() => bookRepo.EditBookAsync(bookEdit, book.Id, UserId));

                    result.Message.Should().Be("Book Is Invalid, Maybe Deleted");

                }
                else
                {
                    //error occers
                    //database error
                    Assert.False(true);
                }
            }
        }

        [Fact]
        public async void EditBook_EditBookWithWrangID_ThrowExeption()
        {
            Domain.Enitties.Book.Book book = null;
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);
                var bookgroup = GetOneBookgroup();
                book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid().ToString(),
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
            }


            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                if (book is not null)
                {
                    BookEdit bookEdit = new BookEdit()
                    {
                        Id = Guid.NewGuid(),
                        BookGroupId = book.BookGroupId,
                        //IsTextual = true,
                        Title = "edited title",
                        Description = "edited Description for test",
                        BookType = BookTypeEnum.Minut,
                        Difficultye = 5,
                        IsComplited = true,
                    };

                    BookRepository bookRepo = new BookRepository(_context);

                    var exp = await Assert.ThrowsAsync<KeyNotFoundException>(() => bookRepo.EditBookAsync(bookEdit, book.Id, UserId));
                    exp.Message.Should().Be("Book Id Invalid");


                }
                else
                {
                    //error occers
                    //database error
                    Assert.False(true);
                }
            }
        }

        [Fact]
        public async void EditBook_WithWrangBookGroupId_throwExeption()
        {
            Domain.Enitties.Book.Book book = null;
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);
                var bookgroup = GetOneBookgroup();
                book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
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
            }


            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                if (book is not null)
                {
                    BookEdit bookEdit = new BookEdit()
                    {
                        Id = book.Id,
                        BookGroupId = 9999,//wrang
                        //IsTextual = true,
                        Title = "edited title",
                        Description = "edited Description for test",
                        BookType = BookTypeEnum.Minut,
                        Difficultye = 5,
                        IsComplited = true,
                    };

                    BookRepository bookRepo = new BookRepository(_context);

                    await Assert.ThrowsAsync<InvalidOperationException>(() => bookRepo.EditBookAsync(bookEdit, book.Id, UserId));


                }
                else
                {
                    //error occers
                    //database error
                    Assert.False(true);
                }
            }
        }
        [Fact]
        public async void EditBook_WithWrangUserId_throwExeption()
        {
            Domain.Enitties.Book.Book book = null;
            var bookgroup = GetOneBookgroup();
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
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
            }


            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                if (book is not null)
                {
                    BookEdit bookEdit = new BookEdit()
                    {
                        Id = book.Id,
                        BookGroupId = bookgroup.Id,
                        //IsTextual = true,
                        Title = "edited title",
                        Description = "edited Description for test",
                        BookType = BookTypeEnum.Minut,
                        Difficultye = 5,
                        IsComplited = true,
                    };

                    BookRepository bookRepo = new BookRepository(_context);

                    //wrang
                    await Assert.ThrowsAsync<KeyNotFoundException>(() => bookRepo.EditBookAsync(bookEdit, book.Id, UserId + $"{Guid.NewGuid()}"));


                }
                else
                {
                    //error occers
                    //database error
                    Assert.False(true);
                }
            }
        }

        [Fact]
        public async void GetBookById_getOneBookWithWrongId_RetunNull()
        {
            //Arange
            Domain.Enitties.Book.Book book = null;
            //Act
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);


                var getedBook = await bookRepo.GetBookByIdAsync(Guid.NewGuid(),UserId);

                //Assert
                Assert.Null(getedBook);
            }
        }
        [Fact]
        public async void GetBookById_getOneBookWithId_RetunBook()
        {
            var bookgroup1 = GetOneBookgroup(true);
            Domain.Enitties.Book.Book book =null;
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                book = new Domain.Enitties.Book.Book()
                {
                    UserId = UserId,
                    BookGroupId = bookgroup1.Id,
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
            }

        
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                var result1 = await bookRepo.GetBookByIdAsync(book.Id, UserId);

                result1.Should().NotBeNull();
                result1.Title.Should().Be(book.Title);
                result1.Difficultye.Should().Be(book.Difficultye);
                result1.BookGroupId.Should().Be(book.BookGroupId);
                result1.Description.Should().Be(book.Description);

            }

        }

        [Fact]
        public async void GetBooks_getListOfBooks_returnListOfBook()
        {
            var bookgroup1 = GetOneBookgroup(true);
            var bookgroup2 = GetOneBookgroup(true);

            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                var book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    BookGroupId = bookgroup1.Id,
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
            }

            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);
                var book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    BookGroupId = bookgroup1.Id,
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
            }
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                var book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    BookGroupId = bookgroup2.Id,
                    IsComplited = true,
                    IsDeleted = false,
                    //IsTextual = true,
                    Title = "test",
                    Description = "test",
                    BookType = Domain.Enums.BookType.Page,
                    CreateDate = DateTime.Now,
                    Difficultye = 8,
                };
                _context.Add(book);
                _context.SaveChanges();
            }

            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                var result1 = await bookRepo.GetBooksAsync(UserId, bookgroup1.Id);
                var result2 = await bookRepo.GetBooksAsync(UserId, bookgroup2.Id);
                var result3 = await bookRepo.GetBooksAsync(UserId);

                Assert.Equal(result1.Count, 2);
                Assert.Equal(result2.Count, 1);
                Assert.Equal(result3.Count, 3);
            }

        }

        [Fact]
        public async void GetBooks_WithDefrentId_returnListOfBookForSpesficUser()
        {
            var bookgroup1 = GetOneBookgroup(true);
            string userid1 = "1234";
            string userid2 = "2345";
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                var book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
                    UserId = userid1,
                    BookGroupId = bookgroup1.Id,
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
            }

            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);
                var book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
                    UserId = userid1,
                    BookGroupId = bookgroup1.Id,
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
            }
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                var book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
                    UserId = userid2,
                    BookGroupId = bookgroup1.Id,
                    IsComplited = true,
                    IsDeleted = false,
                    //IsTextual = true,
                    Title = "test",
                    Description = "test",
                    BookType = Domain.Enums.BookType.Page,
                    CreateDate = DateTime.Now,
                    Difficultye = 8,
                };
                _context.Add(book);
                _context.SaveChanges();
            }

            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                var result1 = await bookRepo.GetBooksAsync(userid1, bookgroup1.Id);
                var result2 = await bookRepo.GetBooksAsync(userid2, bookgroup1.Id);

                Assert.Equal(result1.Count, 2);
                Assert.Equal(result2.Count, 1);
            }

        }

        [Fact]
        public void SafeDeleteBook_deleteBooks_NoReturn()
        {
            var bookgroup = GetOneBookgroup();
            Domain.Enitties.Book.Book book;
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

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
                _context.Add(book);
                _context.SaveChanges();
            }

            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                try
                {
                    bookRepo.SafeDeleteBookAsync(book.Id, UserId);
                    Assert.True(true);
                }
                catch (Exception)
                {
                    Assert.True(false);
                }
            }
        }

        [Fact]
        public async void SafeDeleteBook_deleteBooksWIthWrangId_throwExeption()
        {
            var bookgroup = GetOneBookgroup();
            Domain.Enitties.Book.Book book;
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
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
            }

            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                var result = await Assert.ThrowsAsync<KeyNotFoundException>(() => bookRepo.SafeDeleteBookAsync(Guid.NewGuid(), UserId));

                result.Message.Should().Be("Book id is invalid");
            }
        }

        [Fact]
        public async void SafeDeleteBook_deleteBooksWIthWrangUserId_throwExeption()
        {
            var bookgroup = GetOneBookgroup();
            Domain.Enitties.Book.Book book;
            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                book = new Domain.Enitties.Book.Book()
                {
                    Id = Guid.NewGuid(),
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
            }

            using (var _context = new BookContext(Options))
            {
                //Arrnage
                _context.Database.OpenConnection();
                _context.Database.EnsureCreated();

                BookRepository bookRepo = new BookRepository(_context);

                var result = await Assert.ThrowsAsync<KeyNotFoundException>(() => bookRepo.SafeDeleteBookAsync(book.Id, UserId + "te"));

                Assert.Equal(result.Message, "UserId is invalid");
            }
        }
    }
}
