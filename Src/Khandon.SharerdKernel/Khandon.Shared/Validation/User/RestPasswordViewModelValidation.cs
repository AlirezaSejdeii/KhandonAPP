using FluentValidation;
using Khandon.Shared.Dto.Request.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Validation.User
{
    public class RestPasswordViewModelValidation:AbstractValidator<RestPasswordViewModel>
    {
        public RestPasswordViewModelValidation()
        {
            RuleFor(a => a.NewPassword).NotEmpty().WithMessage("کلمه عبور جدید را وارد کنید");
            RuleFor(a => a.OldPassword).NotEmpty().WithMessage("کلمه عبور قدیمی را وارد کنید");

            RuleFor(a => a.NewPassword)
                .NotEqual(a => a.OldPassword)
                .WithMessage("کلمه عبور جدید نمیتواند تکراری باشد");
        }
    }
}
