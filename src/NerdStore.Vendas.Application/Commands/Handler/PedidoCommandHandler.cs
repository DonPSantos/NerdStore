using MediatR;
using NerdStore.Core.Mediatr;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain.Entitys;
using NerdStore.Vendas.Domain.Interfaces;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
                                        ,IRequestHandler<AtualizarItemPedidoCommand, bool>
                                        ,IRequestHandler<RemoverItemPedidoCommand, bool>
                                        ,IRequestHandler<AplicarVoucherPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediatorHandler mediatorHandler)
        {
            _pedidoRepository = pedidoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteIdAsync(message.ClienteId);
            var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);

            if (pedido is null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
                pedido.AdicionarItem(pedidoItem);

                _pedidoRepository.Adicionar(pedido);
                pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(message.ClienteId, message.ProdutoId));
            }
            else
            {
                var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
                pedido.AdicionarItem(pedidoItem);

                if (pedidoItemExistente)
                {
                    //Não precisa atualizar o pedido em si porque ele está sendo rastreado aqui
                    _pedidoRepository.AtualizarProduto(pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
                }
                else
                {
                    //Não precisa atualizar o pedido em si porque ele está sendo rastreado aqui
                    _pedidoRepository.AdicionarProduto(pedidoItem);
                }
                pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            }
            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId,message.ValorUnitario, message.Quantidade, message.Nome));

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AtualizarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;
            
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteIdAsync(message.ClienteId);

            if (pedido is null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorPedidoAsync(pedido.Id, message.ProdutoId);

            if (!pedido.PedidoItemExistente(pedidoItem))
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido não encontrado!"));
                return false;
            }

            pedido.AtualizarUnidades(pedidoItem, message.Quantidade);

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new PedidoItemAtualizadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId, message.Quantidade));

            //Não precisa atualizar o pedido em si porque ele está sendo rastreado aqui
            _pedidoRepository.AtualizarProduto(pedidoItem);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AplicarVoucherPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteIdAsync(message.ClienteId);

            if(pedido is null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado."));
                return false;
            }

            var voucher = await _pedidoRepository.ObterVoucherPorCodigo(message.CodigoVouncher);

            if(voucher is null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Voucher não encontrado."));
                return false;
            }

            var voucherAplicacaoValidacao = pedido.AplicarVoucher(voucher);

            if (!voucherAplicacaoValidacao.IsValid)
            {
                foreach (var erro in voucherAplicacaoValidacao.Errors)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(erro.ErrorCode, erro.ErrorMessage));
                }
                return false;
            }
            pedido.AdicionarEvento(new VoucherAplicadoPedidoEvent(pedido.ClienteId, pedido.Id,pedido.VoucherId.GetValueOrDefault()));

            _pedidoRepository.Atualizar(pedido);
            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(RemoverItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteIdAsync(message.ClienteId);

            if (pedido is null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorPedidoAsync(pedido.Id, message.ProdutoId);

            if (!pedido.PedidoItemExistente(pedidoItem))
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido não encontrado!"));
                return false;
            }

            pedido.RemoverItem(pedidoItem);
            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new PedidoItemRemovidoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId));

            //Não precisa atualizar o pedido em si porque ele está sendo rastreado aqui
            _pedidoRepository.RemoverProduto(pedidoItem);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        private bool ValidarComando(Command message)
        {
            if (message.IsValido()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));

            }
            return false;
        }
    }
}
