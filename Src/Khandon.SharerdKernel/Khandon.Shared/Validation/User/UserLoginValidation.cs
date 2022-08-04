using FluentValidation;
using Khandon.Shared.Dto.Request.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Validation.User
{
    public class UserLoginValidation:AbstractValidator<UserLogin>
    {
        public UserLoginValidation()
        {
            RuleFor(a=> a.Password).NotEmpty().WithMessage("کلمه عبور را وارد کنید");
            RuleFor(a => a.UsernameOrEmail).NotEmpty().WithMessage("ایمیل یا نام کاربری را وارد کنید");

        }
    }
}
