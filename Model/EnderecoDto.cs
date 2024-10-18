namespace PROJETO_BD_EMPRESA.Model
{
    public class EnderecoDto
    {
        public string Logradouro { get; set; } = null!;

        public string Cidade { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public string Cep { get; set; } = null!;

        public string PontoReferencia { get; set; } = null!;

        public int Nº { get; set; }

        public int FkCliente { get; set; }
    }
}
