using Khandon.Core.Interfaces.IRepository;
using Khandon.Shared.Dto.Enums;
using Khandon.Shared.Dto.Request;
using Khandon.Shared.Dto.Resposne;
using Khandon.Shared.Dto.Result;
using MediatR;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Khandon.Core.CQS.Command
{
    public class BookGroupCreateCommandHandlerTest
    {

        private readonly Mock<IBookRepository> _bookRepositoryMock = new Mock<IBookRepository>();
        private BookCreateCommandHandler _command;
        private readonly string _UserId;
        public BookGroupCreateCommandHandlerTest()
        {
            _UserId = Guid.NewGuid().ToString();
        }
        [Fact]
        public async void BookGroupCreateCommand_createNewBookGroup_BookGroupResult()
        {
            //Arange
            BookGroupResult bookGroupResult = null;

            Mock<IBookGroupRepository> _bookGroupRepositoryMock = new Mock<IBookGroupRepository>();

            _bookGroupRepositoryMock.Setup(a => a.CreateBookgroup(It.IsAny<BookGroupCreate>()));

            var command = new BookGroupCreateCommandHandler(_bookGroupRepositoryMock.Object);


            bookGroupResult= await command.Handle( new BookGroupCreateCommand()
            {
                BookGroupCreate= It.IsAny<BookGroupCreate>()
            }, default);

            _bookGroupRepositoryMock.Verify(a => a.CreateBookgroup(It.IsAny<BookGroupCreate>()), Times.Once);

            //Assert
     
        }
    }
}
