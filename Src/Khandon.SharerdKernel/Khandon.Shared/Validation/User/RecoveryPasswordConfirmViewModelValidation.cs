using FluentValidation;
using Khandon.Shared.Dto.Request.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Validation.User
{
    public class RecoveryPasswordConfirmViewModelValidation:AbstractValidator<RecoveryPasswordConfirmViewModel>
    {
        public RecoveryPasswordConfirmViewModelValidation()
        {
            RuleFor(a => a.NewPassword).NotEmpty().WithMessage("کلمه عبور جدید را وارد کنید");
            RuleFor(a => a.EmailOrUsername).NotEmpty().WithMessage("ایمیل یا نام کاربری را وارد کنید");
            RuleFor(a => a.Token).NotEmpty().WithMessage("توکن معتبر نیست");
           
        }
    }
}
