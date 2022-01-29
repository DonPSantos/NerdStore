using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services.Interfaces;
using NerdStore.Core.Mediatr;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Queries.Interfaces;

namespace NerdStore.WebApps.MVC.Controllers
{
    public class CarrinhoController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatorHandler _mediatrHandler;
        private readonly IPedidoQueries _pedidoQueries;

        public CarrinhoController(IProdutoAppService produtoAppService
                                , IMediatorHandler mediatrHandler
                                , IPedidoQueries pedidoQueries
                                , INotificationHandler<DomainNotification> notifications) : base(notifications, mediatrHandler)
        {
            _produtoAppService = produtoAppService;
            _mediatrHandler = mediatrHandler;
            _pedidoQueries = pedidoQueries;
        }

        [Route("meu-carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("meu-carrinho")]
        public async Task<IActionResult> AdicionarItem(Guid id,int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto == null) return BadRequest();

            if (produto.QuantidadeEstoque < quantidade)
            {
                TempData["Erro"] = "Produto com estoque insuficiente";
                return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
            }

            var command = new AdicionarItemPedidoCommand(ClienteId,produto.Id, produto.Nome, quantidade, produto.Valor);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
            {
                return RedirectToAction("Index");

            }

            TempData["Erro"] = "Produto indisponível";
            TempData["Erros"] = ObterMenssagensErro();
            return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
        }

        [HttpPost]
        [Route("remover-item")]
        public async Task<IActionResult> RemoverItem(Guid id)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto is null) return BadRequest();

            var command = new RemoverItemPedidoCommand(ClienteId, id);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
            {
                return RedirectToAction("Index");
            }
            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("atualizar-item")]
        public async Task<IActionResult> AtualizarItem(Guid id, Guid pedidoId, int quantidade)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            if (produto is null) return BadRequest();

            var command = new AtualizarItemPedidoCommand(ClienteId, pedidoId, id, quantidade);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var command = new AplicarVoucherPedidoCommand(ClienteId,voucherCodigo);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }


        //REMOVER TODAS OS PARAMETROS PEDIDO ID
        //Pode remover porque a logica força apenas um pedido/carrinho por cliente
    }
}
