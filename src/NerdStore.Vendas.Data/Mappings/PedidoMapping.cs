using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Vendas.Domain.Entitys;

namespace NerdStore.Vendas.Data.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Codigo)
                .HasDefaultValueSql("nextval('\"MinhaSequencia\"')");

            // 1 : N => Pedido : PedidoItems
            builder.HasMany(c => c.PedidoItems)
                .WithOne(c => c.Pedido)
                .HasForeignKey(c => c.PedidoId);

            builder.HasOne(c => c.Voucher)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(c => c.VoucherId)
                .IsRequired(false);

            builder.ToTable("Pedidos");
        }
    }
}
