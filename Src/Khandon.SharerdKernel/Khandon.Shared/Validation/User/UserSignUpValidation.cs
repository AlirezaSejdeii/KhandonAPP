using FluentValidation;
using Khandon.Shared.Dto.Request.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Validation.User
{
    public class UserSignUpValidation:AbstractValidator<UserSignUp>
    {
        public UserSignUpValidation()
        {
            RuleFor(a => a.Password)
                .NotEmpty()
                .WithMessage("کلمه عبور را وارد کنید");

            RuleFor(a => a.Email)
                .EmailAddress()
                .WithMessage("ایمیل را به درستی وارد کنید")
                .NotEmpty()
                .WithMessage("ایمیل را وارد کنید");

            RuleFor(a=> a.Username)
                .NotEmpty()
                .WithMessage("نام کاربری را وارد کنید ");
        }
    }
}
