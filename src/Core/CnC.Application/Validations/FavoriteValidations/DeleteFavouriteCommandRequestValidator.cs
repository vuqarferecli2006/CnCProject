using CnC.Application.Features.Favourite.Commands.Delete;
using FluentValidation;

namespace CnC.Application.Validations.FavoriteValidations;

public class DeleteFavouriteCommandRequestValidator : AbstractValidator<DeleteFavouriteCommandRequest>
{
    public DeleteFavouriteCommandRequestValidator()
    {
        RuleFor(F => F.ProductId)
            .NotEmpty()
            .WithMessage("ProductId cannot be null");
    }
}
