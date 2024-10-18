using System.Text.Json.Serialization;

namespace PROJETO_BD_EMPRESA.Model
{
    public class Produto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = null!;

        public decimal Preco { get; set; }

        public int Quantidade { get; set; }
        [JsonIgnore] // Ignora a serialização deste campo
        public byte[]? NotaFiscal { get; set; }

        [JsonIgnore] // Ignora a serialização deste campo
        public string? NotaFiscalBase64 => NotaFiscal != null ? Convert.ToBase64String(NotaFiscal) : null;

        public string UrlNotaFiscal { get; set; } // Certifique-se de que esta propriedade esteja visível
    }
}
