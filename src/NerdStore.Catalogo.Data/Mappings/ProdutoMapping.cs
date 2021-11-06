using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Catalogo.Domain.Entitys;

namespace NerdStore.Catalogo.Data.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.Guid);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Descricao)
                .IsRequired()
                .HasColumnType("varchar(250)");


            builder.Property(p => p.Imagem)
                .HasColumnType("varbinary(MAX)");


            builder.OwnsOne(p => p.Dimensoes, cm =>
            {

                cm.Property(p => p.Altura)
                .HasColumnName("Altura")
                .HasColumnType("int");

                cm.Property(p => p.Largura)
                .HasColumnName("Largura")
                .HasColumnType("int");


                cm.Property(p => p.Profundidade)
                .HasColumnName("Profundidade")
                .HasColumnType("int");
            });
        }
    }
}
