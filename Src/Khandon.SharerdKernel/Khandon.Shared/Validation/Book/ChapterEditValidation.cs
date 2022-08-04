using FluentValidation;
using Khandon.Shared.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Validation
{
    public class ChapterEditValidation:AbstractValidator<ChapterEdit>
    {
        public ChapterEditValidation()
        {
            RuleFor(c => c.Id)
          .NotNull().WithMessage("شناسه نمیتواند خالی باشد")
          .NotEmpty().WithMessage("شناسه نمیتواند خالی باشد");

            RuleFor(c => c.Title)
              .MaximumLength(150).WithMessage("حداکثر 150 کارکتر وارد کنید")
              .NotEmpty().WithMessage("عنوان نمیتواند خالی باشد");

            RuleFor(c => c.ShortDescription)
               .MaximumLength(500).WithMessage("حداکثر 500 کارکتر وارد کنید");

            RuleFor(c => c.BookId)
             .NotNull().WithMessage("مورد مطالعه نمیتواند خالی باشد")
             .NotEmpty().WithMessage("مورد مطالعه نمیتواند خالی باشد");
        }
    }
}
