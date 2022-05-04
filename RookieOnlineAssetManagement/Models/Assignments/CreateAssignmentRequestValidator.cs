using FluentValidation;
using System;

namespace RookieOnlineAssetManagement.Models.Assignments
{
    public class CreateAssignmentRequestValidator : AbstractValidator<CreateAssignmentRequest>
    {
        public CreateAssignmentRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(x => x.AssetId).NotEmpty().WithMessage("AssetId is required");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.AssignedDate == DateTime.MinValue)
                {
                    context.AddFailure(nameof(request.AssignedDate), "Assigned Date is required");
                }
                else if (request.AssignedDate.Date < DateTime.Now.Date)
                {
                    context.AddFailure(nameof(request.AssignedDate), "Assigned Date must current or future date. Please select a different date");
                }
            });
        }
    }
}