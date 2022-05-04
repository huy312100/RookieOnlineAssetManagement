using System;
using FluentValidation;

namespace RookieOnlineAssetManagement.Models.Users
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.PasswordOld).NotEmpty().WithMessage("Old password is required")
                .Matches(@"\A\S+\z").WithMessage("Old password does not contain spaces");

            RuleFor(x => x.PasswordNew).NotEmpty().WithMessage("New password is required")
                .MinimumLength(8).WithMessage("New Password larger than 8 character")
                .MaximumLength(50).WithMessage("New Password must less than 50 character")
                .Matches(@"\A\S+\z").WithMessage("New password does not contain spaces");

            RuleFor(x => x).Custom((request, context) =>
            {
                if(request.PasswordOld == request.PasswordNew)
                {
                    context.AddFailure("The new password matches the old password");
                }
            });


        }
    }
}
