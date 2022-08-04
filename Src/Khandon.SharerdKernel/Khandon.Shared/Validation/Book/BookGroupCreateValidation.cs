using FluentValidation;
using Khandon.Shared.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Validation
{
    public class BookGroupCreateValidation : AbstractValidator<BookGroupCreate>
    {
        public BookGroupCreateValidation()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("عنوان نمیتواند خالی باشد")
                .MaximumLength(100).WithMessage("حداکثر 100 کارکتر وارد کنید");

            RuleFor(c => c.ShortDescription)
                .MaximumLength(500).WithMessage("حداکثر 500 کارکتر وارد کنید");

        }
    }
}
