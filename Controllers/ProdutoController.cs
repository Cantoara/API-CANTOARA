using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROJETO_BD_EMPRESA.Model;
using PROJETO_BD_EMPRESA.Repositorio;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PROJETO_BD_EMPRESA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoRepositorio _produtoRepo;

        public ProdutoController(ProdutoRepositorio produtoRepo)
        {
            _produtoRepo = produtoRepo;
        }

        [HttpGet("{id}/NotaFiscal")]
        public IActionResult GetNotaFiscal(int id)
        {
            try
            {
                var produto = _produtoRepo.GetById(id);

                if (produto == null || produto.NotaFiscal == null)
                {
                    return NotFound(new { Mensagem = "Nota fiscal não encontrada." });
                }

                return File(produto.NotaFiscal, "image/jpeg"); // Ou "image/png" dependendo do formato
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao buscar a nota fiscal.", Detalhes = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<Produto>> GetAll()
        {
            try
            {
                var produtos = _produtoRepo.GetAll();

                if (produtos == null || !produtos.Any())
                {
                    return NotFound(new { Mensagem = "Nenhum produto encontrado." });
                }

                var listaComUrl = produtos.Select(produto => new Produto
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    Quantidade = produto.Quantidade,
                    UrlNotaFiscal = $"{Request.Scheme}://{Request.Host}/api/Produto/{produto.Id}/NotaFiscal" // Define a URL completa para a nota fiscal
                }).ToList();

                return Ok(listaComUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao buscar os produtos.", Detalhes = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetById(int id)
        {
            try
            {
                var produto = _produtoRepo.GetById(id);

                if (produto == null)
                {
                    return NotFound(new { Mensagem = "Produto não encontrado." });
                }

                var produtoComUrl = new Produto
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    Quantidade = produto.Quantidade,
                    UrlNotaFiscal = $"{Request.Scheme}://{Request.Host}/api/Produto/{produto.Id}/NotaFiscal" // Define a URL completa para a nota fiscal
                };

                return Ok(produtoComUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao buscar o produto.", Detalhes = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<object> Post([FromForm] ProdutoDto novoProduto)
        {
            try
            {
                var produto = new Produto
                {
                    Nome = novoProduto.Nome,
                    Preco = novoProduto.Preco
                };

                _produtoRepo.Add(produto, novoProduto.NotaFiscal);

                var resultado = new
                {
                    Mensagem = "Produto cadastrado com sucesso!",
                    Nome = produto.Nome,
                    Preco = produto.Preco
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao cadastrar o produto.", Detalhes = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<object> Put(int id, [FromForm] ProdutoDto produtoAtualizado)
        {
            try
            {
                var produtoExistente = _produtoRepo.GetById(id);

                if (produtoExistente == null)
                {
                    return NotFound(new { Mensagem = "Produto não encontrado." });
                }

                produtoExistente.Nome = produtoAtualizado.Nome;
                produtoExistente.Preco = produtoAtualizado.Preco;

                _produtoRepo.Update(produtoExistente, produtoAtualizado.NotaFiscal);

                var urlNotaFiscal = $"{Request.Scheme}://{Request.Host}/api/produto/{produtoExistente.Id}/NotaFiscal";

                var resultado = new
                {
                    Mensagem = "Produto atualizado com sucesso!",
                    Nome = produtoExistente.Nome,
                    Preco = produtoExistente.Preco,
                    UrlNotaFiscal = urlNotaFiscal
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao atualizar o produto.", Detalhes = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var produtoExistente = _produtoRepo.GetById(id);

                if (produtoExistente == null)
                {
                    return NotFound(new { Mensagem = "Produto não encontrado." });
                }

                _produtoRepo.Delete(id);

                var resultado = new
                {
                    Mensagem = "Produto excluído com sucesso!",
                    Nome = produtoExistente.Nome
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao excluir o produto.", Detalhes = ex.Message });
            }
        }
    }

}
