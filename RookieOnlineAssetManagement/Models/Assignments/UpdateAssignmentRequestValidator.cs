using System;
using FluentValidation;

namespace RookieOnlineAssetManagement.Models.Assignments
{
    public class UpdateAssignmentRequestValidator :AbstractValidator<UpdateAssignmentRequest>
    {
        public UpdateAssignmentRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(x => x.NewAssetId).NotEmpty().WithMessage("New AssetId is required");
            RuleFor(x => x.OldAssetId).NotEmpty().WithMessage("Old AssetId is required");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.AssignedDate == DateTime.MinValue)
                {
                    context.AddFailure(nameof(request.AssignedDate), "Assigned Date is required");
                }
            });
        }
    }
}
