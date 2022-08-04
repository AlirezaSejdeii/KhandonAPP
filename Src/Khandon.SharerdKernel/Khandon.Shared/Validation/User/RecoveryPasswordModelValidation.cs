using FluentValidation;
using Khandon.Shared.Dto.Request.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Validation.User
{
    public class RecoveryPasswordModelValidation : AbstractValidator<RecoveryPasswordModel>
    {
        public RecoveryPasswordModelValidation()
        {
            RuleFor(a => a.UsernameOrEmail).NotEmpty().WithMessage("نام کاربری یا ایمیل خود را وارد کنید");
        }
    }
}
