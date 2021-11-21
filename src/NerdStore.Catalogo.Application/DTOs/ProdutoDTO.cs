using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NerdStore.Catalogo.Application.DTOs
{
    public class ProdutoDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid CategoriaId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public bool Ativo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal Altura { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal Largura { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal Profundidade { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataCadastro { get; set; }

        [NotMapped]
        public IFormFile ImagemUpload { get; set; }

        public string ImagemUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} precisa ter o valor minimo de {1} e máximo de {2}")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int QuantidadeEstoque { get; set; }

        public IEnumerable<CategoriaDTO> CategoriaDTOs { get; set; }
    }
}
