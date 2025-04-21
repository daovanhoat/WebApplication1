using FluentValidation;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;

namespace WebApplication1.Validation
{
    public class UserValidation : AbstractValidator<UserModel>
    {
        public UserValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tên không được để trống");
            RuleFor(x => x.Gener)
                .NotEmpty().WithMessage("Giới tính không đươc để trống")
                .Must(g => g == "Nam" || g == "Nữ")
                .WithMessage("Giới tính chỉ Nam hoặc Nữ");
            RuleFor(x => x.Age)
                .NotEmpty().WithMessage("Tuổi không đươc để trống")
                .InclusiveBetween(18, 65).WithMessage("Tuổi phải từ 18 đến 65");
            RuleFor(x => x.Cong)
                .NotEmpty().WithMessage("Công không được để trống")
                .InclusiveBetween(25, 30).WithMessage("Công phải từ 25 đến 30 ngày");

        }
    }
}
