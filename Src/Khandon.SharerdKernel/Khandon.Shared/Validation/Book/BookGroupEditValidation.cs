using FluentValidation;
using Khandon.Shared.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Validation
{
    public class BookGroupEditValidation:AbstractValidator<BookGroupEdit>
    {
        public BookGroupEditValidation()
        {
            RuleFor(a => a.Id).NotNull().WithMessage("شناسه نمیتواند خالی باشد");
            RuleFor(c => c.Title)
              .NotEmpty().WithMessage("عنوان نمیتواند خالی باشد")
              .MaximumLength(100).WithMessage("حداکثر 100 کارکتر وارد کنید");

            RuleFor(c => c.ShortDescription)
                .MaximumLength(500).WithMessage("حداکثر 500 کارکتر وارد کنید");
        }
    }
}
