using System.Text.Json.Serialization;

namespace PROJETO_BD_EMPRESA.Model
{
    public class Cliente
    {

        public int Id { get; set; }

        public string Nome { get; set; } = null!;

        public string Telefone { get; set; } = null!;
        [JsonIgnore] // Ignora a serialização deste campo
        public byte[]? DocIdentificacao { get; set; }

        [JsonIgnore] // Ignora a serialização deste campo
        public string? DocIdentificacaoBase64 => DocIdentificacao != null ? Convert.ToBase64String(DocIdentificacao) : null;

        public string UrlDocIdentificacao { get; set; } // Certifique-se de que esta propriedade esteja visível

    }
        
}
