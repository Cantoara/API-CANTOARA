namespace PROJETO_BD_EMPRESA.Model
{
    public class ProdutoDto
    {
        public string Nome { get; set; } = null!;

        public decimal Preco { get; set; }

        public int Quantidade { get; set; }

        public IFormFile NotaFiscal { get; set; }
    }
}
