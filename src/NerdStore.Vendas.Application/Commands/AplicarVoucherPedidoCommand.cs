using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class AplicarVoucherPedidoCommand:Command
    {
        public Guid ClienteId { get; private set; }
        public string CodigoVouncher { get; private set; }

        public AplicarVoucherPedidoCommand(Guid clienteId,  string codigoVouncher)
        {
            ClienteId = clienteId;
            CodigoVouncher = codigoVouncher;

        }

        public override bool IsValido()
        {
            ValidationResult = new AplicarVoucherPedidoValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
