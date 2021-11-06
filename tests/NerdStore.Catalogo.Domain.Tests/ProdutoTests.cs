using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using NerdStore.Catalogo.Domain.Entitys;
using NerdStore.Core.DomainObjects;
using System;
using System.IO;
using Xunit;

namespace NerdStore.Catalogo.Domain.Tests
{
    public class ProdutoTests
    {
        [Fact]
        public void Produto_Validar_RetornarException()
        {
            // Arrange & Act & Assert
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", "Imagem");

            IFormFile fileNull = new FormFile(Stream.Null, 0, 0, null, null);

            var ex = Assert.Throws<DomainException>(() =>
                new Produto(string.Empty, "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, file, new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Nome do produto não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", string.Empty, false, 100, Guid.NewGuid(), DateTime.Now, file, new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Descricao do produto não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 0, Guid.NewGuid(), DateTime.Now, file, new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Valor do produto não pode se menor igual a 0", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 100, Guid.Empty, DateTime.Now, file, new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo CategoriaId do produto não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, fileNull, new Dimensoes(1, 1, 1))
            );

            Assert.Equal("O campo Imagem do produto não pode estar vazio", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
                new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, file, new Dimensoes(0, 1, 1))
            );

            Assert.Equal("O campo Altura não pode ser menor ou igual a 0", ex.Message);
        }
    }
}