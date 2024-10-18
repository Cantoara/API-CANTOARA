using System.Text.Json.Serialization;

namespace PROJETO_BD_EMPRESA.Model
{
    public class Venda
    {
        public int Id { get; set; }

        public decimal Valor { get; set; }

        public int FkProduto { get; set; }

        public int FkCliente { get; set; }
        [JsonIgnore]
        public byte[]? NotaFiscal { get; set; }

        [JsonIgnore] // Ignora a serialização deste campo
        public string? NotaFiscal64 => NotaFiscal != null ? Convert.ToBase64String(NotaFiscal) : null;

        public string UrlNotaFiscal { get; set; } // Certifique-se de que esta propriedade esteja visível
    }
}
