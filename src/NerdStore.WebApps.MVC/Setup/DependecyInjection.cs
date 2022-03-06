using MediatR;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.Services.Interfaces;
using NerdStore.Catalogo.Data;
using NerdStore.Catalogo.Data.Repository;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Catalogo.Domain.Services;
using NerdStore.Core.Mediatr;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Infra.DropBoxServices;
using NerdStore.Infra.DropBoxServices.Interfaces;
using NerdStore.Infra.EmailServices;
using NerdStore.Infra.EmailServices.Interfaces;
using NerdStore.Pagamentos.AntiCorruption;
using NerdStore.Pagamentos.AntiCorruption.Interfaces;
using NerdStore.Pagamentos.Business;
using NerdStore.Pagamentos.Business.Events;
using NerdStore.Pagamentos.Business.Interfaces;
using NerdStore.Pagamentos.Data;
using NerdStore.Pagamentos.Data.Repositories;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Application.Queries;
using NerdStore.Vendas.Application.Queries.Interfaces;
using NerdStore.Vendas.Data;
using NerdStore.Vendas.Data.Repository;
using NerdStore.Vendas.Domain.Interfaces;

namespace NerdStore.WebApps.MVC.Setup
{
    public static class DependecyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region Mediator

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            #endregion

            #region Notifications

            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            #endregion

            #region Catalogo

            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IEstoqueServices, EstoqueServices>();

            services.AddScoped<CatalogoContext>();

            services.AddScoped<INotificationHandler<ProdutoBaixoEstoqueEvent>, ProdutoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoIniciadoEvent>, ProdutoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoProcessamentoCanceladoEvent>, ProdutoEventHandler>();

            #endregion

            #region Vendas
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoQueries, PedidoQueries>();
            services.AddScoped<VendasContext>();

            services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AplicarVoucherPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<IniciarPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<FinalizarPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<CancelarProcessamentoPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<CancelarProcessamentoPedidoEstornarEstoqueCommand, bool>, PedidoCommandHandler>();

            services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoEstoqueRejeitadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoPagamentoRealizadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoPagamentoRecusadoEvent>, PedidoEventHandler>();
            #endregion

            #region Pagamentos

            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
            services.AddScoped<IPayPalGateway, PayPalGateway>();
            services.AddScoped<IConfigurationManager, Pagamentos.AntiCorruption.ConfigurationManager>();
            services.AddScoped<PagamentoContext>();

            services.AddScoped<INotificationHandler<PedidoEstoqueConfirmadoEvent>, PagamentoEventHandler>();

            #endregion

            #region Infra
            services.AddScoped<IDropBoxService, DropBoxService>();
            services.AddScoped<IEmailService, EmailService>();
            #endregion
        }
    }
}
