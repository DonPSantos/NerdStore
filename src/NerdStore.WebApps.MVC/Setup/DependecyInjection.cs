using MediatR;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.Services.Interfaces;
using NerdStore.Catalogo.Data;
using NerdStore.Catalogo.Data.Repository;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Catalogo.Domain.Services;
using NerdStore.Core.Mediatr;
using NerdStore.Infra.DropBoxServices;
using NerdStore.Infra.DropBoxServices.Interfaces;

namespace NerdStore.WebApps.MVC.Setup
{
    public static class DependecyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {

            #region Catalogo

            services.AddScoped<IMediatrHandler, MediatrHandler>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IEstoqueServices, EstoqueServices>();
            services.AddScoped<IDropBoxService, DropBoxService>();
            services.AddScoped<CatalogoContext>();

            services.AddScoped<INotificationHandler<ProdutoBaixoEstoqueEvent>, ProdutoEventHandler>();
            #endregion
        }
    }
}
