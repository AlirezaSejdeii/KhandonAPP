using FluentValidation;
using Khandon.Shared.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Validation
{
    public class StudyGetValidation:AbstractValidator<StudyGet>
    {
        public StudyGetValidation()
        {
            RuleFor(a => a.Page).GreaterThanOrEqualTo(1).WithMessage("صفحه 1 و بالاتر را انتخاب کنید");
        }
    }
}
