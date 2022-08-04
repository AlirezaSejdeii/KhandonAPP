using FluentValidation;
using Khandon.Shared.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Validation
{
    public class BookCreateValidation : AbstractValidator<BookCreate>
    {
        public BookCreateValidation()
        {
            RuleFor(c => c.Title)
                .MaximumLength(150).WithMessage("حداکثر 150 کارکتر وارد کنید")
                .NotEmpty().WithMessage("عنوان نمیتواند خالی باشد");



            RuleFor(c => c.Description)
              .MaximumLength(500).WithMessage("حداکثر 500 کارکتر وارد کنید");


            RuleFor(c => c.Difficultye)
                .GreaterThan(0).WithMessage("ورودی باید بین 1 تا 10 باشد")
                .LessThan(10).WithMessage("ورودی باید بین 1 تا 10 باشد");


            //RuleFor(c => c.BookType).NotEmpty().WithMessage("نوع مطالعه نمیتواند خالی باشد");


            RuleFor(c => c.BookGroupId)
                .NotEmpty().WithMessage("گروه مطالعاتی نمیتواند خالی باشد")
                .NotNull().WithMessage("گروه مطالعاتی نمیتواند خالی باشد");
        }
    }
}
