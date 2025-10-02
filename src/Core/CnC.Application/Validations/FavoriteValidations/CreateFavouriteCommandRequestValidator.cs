using CnC.Application.Features.Favourite.Commands.Create;
using FluentValidation;

namespace CnC.Application.Validations.FavoriteValidations;

public class CreateFavouriteCommandRequestValidator : AbstractValidator<CreateFavouriteCommandRequest>
{
    public CreateFavouriteCommandRequestValidator()
    {
        RuleFor(F => F.ProductId)
            .NotEmpty()
            .WithMessage("ProductId cannot be null");
    }
}
