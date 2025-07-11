using FluentValidation;

namespace FGC.Application.Catalog.Commands.CreatePromotion;

public class CreatePromotionCommandValidator : AbstractValidator<CreatePromotionCommand>
{
    public CreatePromotionCommandValidator()
    {
        RuleFor(x => x.JogoId).NotEmpty().WithMessage("Id do jogo é obrigatório.");
        RuleFor(x => x.Desconto)
            .ExclusiveBetween(0, 1).WithMessage("Desconto deve ser entre 0 e 1 (ex: 0.20 = 20%).");
        RuleFor(x => x.Termino)
            .GreaterThan(x => x.Inicio).WithMessage("Data de término deve ser após início.");
    }
}
