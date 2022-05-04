using FluentValidation;
using System;

namespace RookieOnlineAssetManagement.Models.Assignments
{
    public class GetAssignmentPagingRequestValidator : AbstractValidator<GetAssignmentPagingRequest>
    {
        public GetAssignmentPagingRequestValidator()
        {
            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.AssignedDateTo < request.AssignedDateFrom)
                {
                    context.AddFailure(nameof(request.AssignedDateTo), "Assigned Date To must equal or later than Assigned Date From. Please select a different date");
                }
            });
        }
    }
}