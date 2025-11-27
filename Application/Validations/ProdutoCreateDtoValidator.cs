using Application.DTOs;
using FluentValidation;

public class ProdutoCreateDtoValidator : AbstractValidator<ProdutoCreateDto>
{
    public ProdutoCreateDtoValidator()
    {
        RuleFor(p => p.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .Length(4, 200)
            .WithMessage("O tamanho do nome deve ser entre 2 e 200 caracteres")
            .Must(nome => !string.IsNullOrWhiteSpace(nome))
            .WithMessage("O nome não pode conter apenas um espaço em branco");

        RuleFor(p => p.Descricao)
            .MaximumLength(1000)
            .WithMessage("A descrição do produto não pode ter mais de 1000 caracteres")
            .When(p => !string.IsNullOrWhiteSpace(p.Descricao));

        RuleFor(p => p.Preco)
            .GreaterThan(0)
            .WithMessage("O preço deve ser maior que zero")
            .PrecisionScale(10, 2, ignoreTrailingZeros: true)
            .WithMessage("O preço deve ter no máximo duas casa decimais e conter dez digitos no total");

        RuleFor(p => p.Estoque)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O estoque não pode ser negativo");
    }
}