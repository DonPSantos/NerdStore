using FluentValidation;

namespace NerdStore.Vendas.Domain.Entitys.Validations
{
    public class ValidarSeAplicavelValidation : AbstractValidator<Voucher>
    {
        public ValidarSeAplicavelValidation()
        {
            RuleFor(v => v.Validade)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Este voucher expirou.");

            RuleFor(v => v.Ativo)
                .Equal(true)
                .WithMessage("Voucher não é mais valido.");

            RuleFor(v => v.Utilizado)
                .Equal(false)
                .WithMessage("Voucher já utilizado.");

            RuleFor(v => v.Quantidade)
                .GreaterThan(0)
                .WithMessage("Voucher não está mais disponível.");
        }
    }
}
