using FluentValidation;
using System;

namespace RookieOnlineAssetManagement.Models.Users
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.BirthDay).NotEmpty().WithMessage("BirthDay is required").LessThan(DateTime.Now.AddYears(-18)).WithMessage("User is under 18. Please select a different date");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.JoinedDate == DateTime.MinValue)
                {
                    context.AddFailure(nameof(request.JoinedDate), "Joined date is required");
                }
                if (DateTime.Compare(request.JoinedDate, request.BirthDay) < 0)
                {
                    context.AddFailure(nameof(request.JoinedDate), "Joined date is not later than Date of Birth. Please select a different date");
                }
                if (request.JoinedDate.DayOfWeek == DayOfWeek.Saturday || request.JoinedDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    context.AddFailure(nameof(request.JoinedDate), "Joined date is Saturday or Sunday. Please select a different date");
                }
            });
        }
    }
}