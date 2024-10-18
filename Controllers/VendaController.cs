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
    public class VendaController : ControllerBase
    {
        private readonly VendaRepositorio _vendaRepo;

        public VendaController(VendaRepositorio vendaRepo)
        {
            _vendaRepo = vendaRepo;
        }

        [HttpGet("{id}/NotaFiscal")]
        public IActionResult GetNotaFiscal(int id)
        {
            try
            {
                var venda = _vendaRepo.GetById(id);

                if (venda == null || venda.NotaFiscal == null)
                {
                    return NotFound(new { Mensagem = "Nota fiscal não encontrada." });
                }

                return File(venda.NotaFiscal, "image/jpeg"); // Ou "image/png" dependendo do formato
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao buscar a nota fiscal.", Detalhes = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<Venda>> GetAll()
        {
            try
            {
                var vendas = _vendaRepo.GetAll();

                if (vendas == null || !vendas.Any())
                {
                    return NotFound(new { Mensagem = "Nenhuma venda encontrada." });
                }

                var listaComUrl = vendas.Select(venda => new Venda
                {
                    Id = venda.Id,
                    Valor = venda.Valor,
                    FkProduto = venda.FkProduto,
                    FkCliente = venda.FkCliente,
                    UrlNotaFiscal = $"{Request.Scheme}://{Request.Host}/api/Venda/{venda.Id}/NotaFiscal" // Define a URL completa para a nota fiscal
                }).ToList();

                return Ok(listaComUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao buscar as vendas.", Detalhes = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Venda> GetById(int id)
        {
            try
            {
                var venda = _vendaRepo.GetById(id);

                if (venda == null)
                {
                    return NotFound(new { Mensagem = "Venda não encontrada." });
                }

                var vendaComUrl = new Venda
                {
                    Id = venda.Id,
                    Valor = venda.Valor,
                    FkProduto = venda.FkProduto,
                    FkCliente = venda.FkCliente,
                    UrlNotaFiscal = $"{Request.Scheme}://{Request.Host}/api/Venda/{venda.Id}/NotaFiscal" // Define a URL completa para a nota fiscal
                };

                return Ok(vendaComUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao buscar a venda.", Detalhes = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<object> Post([FromForm] VendaDto novaVenda)
        {
            try
            {
                var venda = new Venda
                {
                    Valor = novaVenda.Valor,
                    FkProduto = novaVenda.FkProduto,
                    FkCliente = novaVenda.FkCliente,
                };

                _vendaRepo.Add(venda, novaVenda.NotaFiscal);

                var resultado = new
                {
                    Mensagem = "Venda cadastrada com sucesso!",
                    Valor = novaVenda.Valor,
                    FkProduto = novaVenda.FkProduto,
                    FkCliente = novaVenda.FkCliente,
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao cadastrar a venda.", Detalhes = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<object> Put(int id, [FromForm] VendaDto vendaAtualizada)
        {
            try
            {
                var vendaExistente = _vendaRepo.GetById(id);

                if (vendaExistente == null)
                {
                    return NotFound(new { Mensagem = "Venda não encontrada." });
                }

                vendaExistente.Valor = vendaAtualizada.Valor;
                vendaExistente.FkProduto = vendaAtualizada.FkProduto;

                _vendaRepo.Update(vendaExistente, vendaAtualizada.NotaFiscal);

                var urlNotaFiscal = $"{Request.Scheme}://{Request.Host}/api/Venda/{vendaExistente.Id}/NotaFiscal";

                var resultado = new
                {
                    Mensagem = "Venda atualizada com sucesso!",
                    Valor = vendaExistente.Valor,
                    FkProduto = vendaExistente.FkProduto,
                    UrlNotaFiscal = urlNotaFiscal
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao atualizar a venda.", Detalhes = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var vendaExistente = _vendaRepo.GetById(id);

                if (vendaExistente == null)
                {
                    return NotFound(new { Mensagem = "Venda não encontrada." });
                }

                _vendaRepo.Delete(id);

                var resultado = new
                {
                    Mensagem = "Venda excluída com sucesso!",
                    Valor = vendaExistente.Valor,
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao excluir a venda.", Detalhes = ex.Message });
            }
        }
    }

}
