using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace RookieOnlineAssetManagement.Models.Users
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MinimumLength(1).WithMessage("First Name very short")
                .MaximumLength(100).WithMessage("First Name very long")
                //.Matches(@"^[^0123456789`~!@#$%^&*()-_+={}\[\]|\\:;“’<,>.?๐฿]*$")
                .Matches(@"^[a-zA-Z]")
                .WithMessage("First Name can not contain special character or number");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MinimumLength(1).WithMessage("Last Name very short")
                .MaximumLength(100).WithMessage("Last Name very long")
                //.Matches(@"^[^0123456789`~!@#$%^&*()-_+={}\[\]|\\:;“’<,>.?๐฿]*$")
                .Matches(@"^[a-zA-Z]")
                .WithMessage("Last Name can not contain special character or number");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.BirthDay == DateTime.MinValue)
                {
                    context.AddFailure(nameof(request.BirthDay), "Date Of Birth is required");
                }
                else if (request.BirthDay > DateTime.Now.AddYears(-18))
                {
                    context.AddFailure(nameof(request.BirthDay), "User is under 18. Please select a different date");
                }
                if (request.JoinedDate == DateTime.MinValue)
                {
                    context.AddFailure(nameof(request.JoinedDate), "Joined date is required");
                }
                else if (DateTime.Compare(request.JoinedDate, request.BirthDay) <= 0)
                {
                    context.AddFailure(nameof(request.JoinedDate), "Joined date is not later than Date of Birth. Please select a different date");
                }
                else if (request.JoinedDate.DayOfWeek == DayOfWeek.Saturday || request.JoinedDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    context.AddFailure(nameof(request.JoinedDate), "Joined date is Saturday or Sunday. Please select a different date");
                }
            });
        }
    }
}