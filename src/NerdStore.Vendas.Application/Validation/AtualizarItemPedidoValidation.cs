using FluentValidation;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.Vendas.Application.Validation
{
    public class AtualizarItemPedidoValidation : AbstractValidator<AtualizarItemPedidoCommand>
    {
        public AtualizarItemPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente invalido.");

            RuleFor(c => c.PedidoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do pedido invalido.");

            RuleFor(c => c.ProdutoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do produto invalido.");

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage("A quantidade minima de um item é 1");


            RuleFor(c => c.Quantidade)
                .LessThan(15)
                .WithMessage("A quantidade máxima de um item é 15");
        }
    }
}
