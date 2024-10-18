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
    public class EnderecoController : ControllerBase
    {
        private readonly EnderecoRepositorio _enderecoRepo;

        public EnderecoController(EnderecoRepositorio enderecoRepositorio)
        {
            _enderecoRepo = enderecoRepositorio;
        }

        // GET: api/Endereco
        [HttpGet]
        public ActionResult<List<Endereco>> GetAll()
        {
            try
            {
                var enderecos = _enderecoRepo.GetAll();

                if (enderecos == null || !enderecos.Any())
                {
                    return NotFound(new { Mensagem = "Nenhum endereço encontrado." });
                }

                var listaComUrl = enderecos.Select(endereco => new Endereco
                {
                    Id = endereco.Id,
                    Logradouro = endereco.Logradouro,
                    Cidade = endereco.Cidade,
                    Estado = endereco.Estado,
                    Cep = endereco.Cep,
                    PontoReferencia = endereco.PontoReferencia,
                    Nº = endereco.Nº,
                    FkCliente = endereco.FkCliente
                }).ToList();

                return Ok(listaComUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao buscar os endereços.", Detalhes = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Endereco> GetById(int id)
        {
            try
            {
                var endereco = _enderecoRepo.GetById(id);

                if (endereco == null)
                {
                    return NotFound(new { Mensagem = "Endereço não encontrado." });
                }

                var enderecoComUrl = new Endereco
                {
                    Id = endereco.Id,
                    Logradouro = endereco.Logradouro,
                    Cidade = endereco.Cidade,
                    Estado = endereco.Estado,
                    Cep = endereco.Cep,
                    PontoReferencia = endereco.PontoReferencia,
                    Nº = endereco.Nº,
                    FkCliente = endereco.FkCliente
                };

                return Ok(enderecoComUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao buscar o endereço.", Detalhes = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<object> Post([FromForm] EnderecoDto novoEndereco)
        {
            try
            {
                var endereco = new Endereco
                {
                    Logradouro = novoEndereco.Logradouro,
                    Cidade = novoEndereco.Cidade,
                    Estado = novoEndereco.Estado,
                    Cep = novoEndereco.Cep,
                    PontoReferencia = novoEndereco.PontoReferencia,
                    Nº = novoEndereco.Nº,
                    FkCliente = novoEndereco.FkCliente
                };

                _enderecoRepo.Add(endereco);

                var resultado = new
                {
                    Mensagem = "Endereço cadastrado com sucesso!",
                    Logradouro = endereco.Logradouro,
                    Cidade = endereco.Cidade
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao cadastrar o endereço.", Detalhes = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<object> Put(int id, [FromForm] EnderecoDto enderecoAtualizado)
        {
            try
            {
                var enderecoExistente = _enderecoRepo.GetById(id);

                if (enderecoExistente == null)
                {
                    return NotFound(new { Mensagem = "Endereço não encontrado." });
                }

                enderecoExistente.Logradouro = enderecoAtualizado.Logradouro;
                enderecoExistente.Cidade = enderecoAtualizado.Cidade;

                _enderecoRepo.Update(enderecoExistente);

                var resultado = new
                {
                    Mensagem = "Endereço atualizado com sucesso!",
                    Logradouro = enderecoExistente.Logradouro,
                    Cidade = enderecoExistente.Cidade
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao atualizar o endereço.", Detalhes = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var enderecoExistente = _enderecoRepo.GetById(id);

                if (enderecoExistente == null)
                {
                    return NotFound(new { Mensagem = "Endereço não encontrado." });
                }

                _enderecoRepo.Delete(id);

                var resultado = new
                {
                    Mensagem = "Endereço excluído com sucesso!",
                    Logradouro = enderecoExistente.Logradouro
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Ocorreu um erro ao excluir o endereço.", Detalhes = ex.Message });
            }
        }
    }

}
