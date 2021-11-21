using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.DTOs;
using NerdStore.Catalogo.Application.Services.Interfaces;

namespace NerdStore.WebApps.MVC.Controllers.Admin
{
    [Route("admin/")]
    public class AdminProdutosController : Controller
    {
        private readonly IProdutoAppService _produtoAppService;

        public AdminProdutosController(IProdutoAppService produtoAppService)
        {
            _produtoAppService = produtoAppService;
        }

        [HttpGet]
        [Route("produtos")]
        public async Task<IActionResult> Index()
        {
            return View(await _produtoAppService.ObterTodos());
        }


        [HttpGet]
        [Route("novo-produto")]
        public async Task<IActionResult> NovoProduto()
        {
            return View(await PopularCategorias(new ProdutoDTO()));
        }


        [HttpPost]
        [Route("novo-produto")]
        public async Task<IActionResult> NovoProduto(ProdutoDTO produtoDTO)
        {
            ModelState.Remove("CategoriaDTOs");
            ModelState.Remove("ImagemUrl");
            if (!ModelState.IsValid) return View(await PopularCategorias(produtoDTO));

            await _produtoAppService.AdicionarProduto(produtoDTO);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("atualizar-produto")]
        public async Task<IActionResult> AtualizarProduto(Guid id)
        {
            return View(await PopularCategorias(await _produtoAppService.ObterPorId(id)));
        }


        [HttpPost]
        [Route("atualizar-produto")]
        public async Task<IActionResult> AtualizarProduto(Guid id, ProdutoDTO produtoDTO)
        {
            ModelState.Remove("CategoriaDTOs");
            ModelState.Remove("ImagemUpload");
            ModelState.Remove("QuantidadeEstoque");

            var produto = await _produtoAppService.ObterPorId(produtoDTO.Id);
            produtoDTO.QuantidadeEstoque = produto.QuantidadeEstoque;

            if (!ModelState.IsValid) return View(await PopularCategorias(produtoDTO));

            await _produtoAppService.AlterarProduto(produtoDTO);

            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("produtos-atualizar-estoque")]
        public async Task<IActionResult> AtualizarEstoque(Guid id)
        {
            return View("Estoque", await _produtoAppService.ObterPorId(id));
        }


        [HttpPost]
        [Route("produtos-atualizar-estoque")]
        public async Task<IActionResult> AtualizarEstoque(Guid id, int quantidade)
        {
            if (quantidade > 0)
            {
                await _produtoAppService.ReporEstoque(id, quantidade);
            }
            else
            {
                await _produtoAppService.DebitarEstoque(id, quantidade);
            }
            return RedirectToAction("Index");
        }

        private async Task<ProdutoDTO> PopularCategorias(ProdutoDTO produtoDTO)
        {
            produtoDTO.CategoriaDTOs = await _produtoAppService.ObterCategorias();
            return produtoDTO;
        }
    }
}
