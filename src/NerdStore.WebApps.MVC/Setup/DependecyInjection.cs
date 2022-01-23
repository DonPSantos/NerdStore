using MediatR;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.Services.Interfaces;
using NerdStore.Catalogo.Data;
using NerdStore.Catalogo.Data.Repository;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Catalogo.Domain.Services;
using NerdStore.Core.Mediatr;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Infra.DropBoxServices;
using NerdStore.Infra.DropBoxServices.Interfaces;
using NerdStore.Infra.EmailServices;
using NerdStore.Infra.EmailServices.Interfaces;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Application.Queries;
using NerdStore.Vendas.Application.Queries.Interfaces;
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
            #endregion

            #region Vendas
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoItemAdicionadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoAtualizadoEvent>, PedidoEventHandler>();
            services.AddScoped<IPedidoQueries, PedidoQueries>();
            #endregion

            #region Infra
            services.AddScoped<IDropBoxService, DropBoxService>();
            services.AddScoped<IEmailService, EmailService>();
            #endregion
        }
    }
}
