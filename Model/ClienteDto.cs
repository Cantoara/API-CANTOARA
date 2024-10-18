using System.Text.Json.Serialization;

namespace PROJETO_BD_EMPRESA.Model
{
    public class ClienteDto
    {
        public string Nome { get; set; } = null!;

        public string Telefone { get; set; } = null!;
       
        public IFormFile DocIdentificacao { get; set; }
    }
}
