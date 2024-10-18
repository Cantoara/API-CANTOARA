namespace PROJETO_BD_EMPRESA.Model
{
    public class VendaDto
    {
        public decimal Valor { get; set; }

        public IFormFile NotaFiscal { get; set; }

        public int FkProduto { get; set; }

        public int FkCliente { get; set; }
    }
}
