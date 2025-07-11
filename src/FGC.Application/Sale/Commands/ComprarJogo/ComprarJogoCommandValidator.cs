using FluentValidation;

namespace FGC.Application.Sale.Commands.ComprarJogo;

public class ComprarJogoCommandValidator : AbstractValidator<ComprarJogoCommand>
{
    public ComprarJogoCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Id do usuário é obrigatório.");
        RuleFor(x => x.JogoId).NotEmpty().WithMessage("Id do jogo é obrigatório.");
    }
}
