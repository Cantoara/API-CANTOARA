using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROJETO_BD_EMPRESA.Model;
using PROJETO_BD_EMPRESA.ORM;
using PROJETO_BD_EMPRESA.Repositorio;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PROJETO_BD_EMPRESA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteRepositorio _clienteRepo;

        public ClienteController(ClienteRepositorio clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        [HttpGet("{id}/DocIdentificacao")]
        public IActionResult GetDocIdentificacao(int id)
        {
            try
            {
                var cliente = _clienteRepo.GetById(id);

                if (cliente == null || cliente.DocIdentificacao == null)
                {
                    return NotFound(new { Mensagem = "Foto não encontrada." });
                }

                return File(cliente.DocIdentificacao, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao buscar a foto.", Detalhes = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<List<Cliente>> GetAll()
        {
            try
            {
                var clientes = _clienteRepo.GetAll();

                if (clientes == null || !clientes.Any())
                {
                    return NotFound(new { Mensagem = "Nenhum cliente encontrado." });
                }

                var listaComUrl = clientes.Select(cliente => new Cliente
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Telefone = cliente.Telefone,
                    DocIdentificacao = cliente.DocIdentificacao,
                    UrlDocIdentificacao = $"{Request.Scheme}://{Request.Host}/api/Cliente/{cliente.Id}/DocIdentificacao"
                }).ToList();

                return Ok(listaComUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao buscar a lista de clientes.", Detalhes = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Cliente> GetById(int id)
        {
            try
            {
                var cliente = _clienteRepo.GetById(id);

                if (cliente == null)
                {
                    return NotFound(new { Mensagem = "Cliente não encontrado." });
                }

                var clienteComUrl = new Cliente
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Telefone = cliente.Telefone,
                    UrlDocIdentificacao = $"{Request.Scheme}://{Request.Host}/api/Cliente/{cliente.Id}/DocIdentificacao"
                };

                return Ok(clienteComUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao buscar o cliente.", Detalhes = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<object> Post([FromForm] ClienteDto novoCliente)
        {
            try
            {
                var cliente = new Cliente
                {
                    Nome = novoCliente.Nome,
                    Telefone = novoCliente.Telefone
                };

                _clienteRepo.Add(cliente, novoCliente.DocIdentificacao);

                var resultado = new
                {
                    Mensagem = "Cliente cadastrado com sucesso!",
                    Nome = cliente.Nome,
                    Telefone = cliente.Telefone
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao cadastrar o cliente.", Detalhes = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<object> Put(int id, [FromForm] ClienteDto clienteAtualizado)
        {
            try
            {
                var clienteExistente = _clienteRepo.GetById(id);

                if (clienteExistente == null)
                {
                    return NotFound(new { Mensagem = "Cliente não encontrado." });
                }

                clienteExistente.Nome = clienteAtualizado.Nome;
                clienteExistente.Telefone = clienteAtualizado.Telefone;

                _clienteRepo.Update(clienteExistente, clienteAtualizado.DocIdentificacao);

                var urlFoto = $"{Request.Scheme}://{Request.Host}/api/Cliente/{clienteExistente.Id}/DocIdentificacao";

                var resultado = new
                {
                    Mensagem = "Cliente atualizado com sucesso!",
                    Nome = clienteExistente.Nome,
                    Telefone = clienteExistente.Telefone,
                    UrlFoto = urlFoto
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao atualizar o cliente.", Detalhes = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var clienteExistente = _clienteRepo.GetById(id);

                if (clienteExistente == null)
                {
                    return NotFound(new { Mensagem = "Cliente não encontrado." });
                }

                _clienteRepo.Delete(id);

                var resultado = new
                {
                    Mensagem = "Cliente excluído com sucesso!",
                    Nome = clienteExistente.Nome,
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao excluir o cliente.", Detalhes = ex.Message });
            }
        }
    }


}

