using FluentValidation;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.Vendas.Application.Validation
{
    public class IniciarPedidoValidation : AbstractValidator<IniciarPedidoCommand>
    {
        public IniciarPedidoValidation()
        {
            RuleFor(p => p.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido.");

            RuleFor(p => p.PedidoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do pedido inválido.");

            RuleFor(p => p.NomeCartao)
                .NotEmpty()
                .WithMessage("Nome do cartão não informado.");

            RuleFor(p => p.NumeroCartao)
                .CreditCard()
                .WithMessage("Número do carão de crédito inválido.");

            RuleFor(p => p.ExpiracaoCartao)
                .NotEmpty()
                .WithMessage("Data de expiração não informada.");

            RuleFor(p => p.CvvCartao)
                .NotEmpty()
                .Length(3)
                .WithMessage("O CVV não foi preenchido corretamente.");
        }
    }
}
