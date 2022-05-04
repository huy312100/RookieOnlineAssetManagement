using System;
using FluentValidation;

namespace RookieOnlineAssetManagement.Models.Assets
{
    public class UpdateAssetRequestValidator : AbstractValidator<UpdateAssetRequest>
    {
        public UpdateAssetRequestValidator()
        {
            RuleFor(x => x.AssetName)
                .NotEmpty().WithMessage("Asset name is required")
                .Matches("^[a-zA-Z0-9 ]*$").WithMessage("Asset name cannot contain special character");
            RuleFor(x => x.Specification).NotEmpty().WithMessage("Specification is required");
            RuleFor(x => x.InstalledDate).NotEmpty().WithMessage("Installed date is required");
            RuleFor(x => x.AssetState).NotNull().WithMessage("Asset state is required");
            RuleFor(x => x).Custom((request, context) =>
            {
                if((int)request.AssetState == 3)
                {
                    context.AddFailure(nameof(request.AssetState), "Asset state cannot update for because it was assigned");
                }
            });
        }
    }
}
