using NerdStore.Core.DomainObjects;
using NerdStore.Core.Mediatr;

namespace NerdStore.Vendas.Data
{
    public static class MediatorExtension
    {
        public static async Task PublicarEventos(this IMediatorHandler mediator, VendasContext vendasContext)
        {
            var domainEntities = vendasContext
                                            .ChangeTracker
                                            .Entries<Entity>()
                                            .Where(x => x.Entity.Notificacoes is not null && x.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                                        .SelectMany(x => x.Entity.Notificacoes)
                                        .ToList();

            domainEntities.ToList().ForEach(entity => entity.Entity.LimparEventos());

            var tasks = domainEvents.Select(async (domainEvent) =>
            {
                await mediator.PublicarEvento(domainEvent);
            });

            await Task.WhenAll(tasks);
        }
    }
}
