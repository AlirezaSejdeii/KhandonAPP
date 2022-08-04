using FluentValidation;
using Khandon.Shared.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Validation
{
    public class StudyEditValidation : AbstractValidator<StudyEdit>
    {
        public StudyEditValidation()
        {
            RuleFor(c => c.Title)
              .MaximumLength(150).WithMessage("حداکثر 150 کارکتر وارد کنید")
              .NotEmpty().WithMessage("عنوان نمیتواند خالی باشد");

            RuleFor(c => c.ShortDescription)
               .MaximumLength(500).WithMessage("حداکثر 500 کارکتر وارد کنید");

            RuleFor(a => a.Length)
                .NotNull().WithMessage("طول مطالعه نمیتواند خالی باشد")
                .NotEmpty().WithMessage("طول مطالعه نمیتواند خالی باشد")
                .NotEqual(0).WithMessage("طول مطالعه نمیتواند خالی باشد");

            RuleFor(a => a.Flag)
                .NotNull().WithMessage("نشانگر نمیتواند خالی باشد")
                .NotEmpty().WithMessage("نشانگر نمیتواند خالی باشد")
                .NotEqual(0).WithMessage("نشانگر نمیتواند خالی باشد");

            RuleFor(a => a.Id)
             .NotEmpty().WithMessage("شناسه نمیتواند خالی باشد")
             .NotNull().WithMessage("شناسه نمیتواند خالی باشد");
        }
    }
}
