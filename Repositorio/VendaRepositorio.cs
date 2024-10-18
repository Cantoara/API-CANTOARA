using PROJETO_BD_EMPRESA.Model;
using PROJETO_BD_EMPRESA.ORM;

namespace PROJETO_BD_EMPRESA.Repositorio
{
    public class VendaRepositorio
    {
        private ProjetoBdEmpresaContext _context;
       
        public VendaRepositorio(ProjetoBdEmpresaContext context)
        {
            _context = context;
        }
       
        public void Add(Venda venda, IFormFile NotaFiscal)
        {
            // Verifica se uma foto foi enviada
            byte[] NotaFiscalBytes = null;
            if (NotaFiscal != null && NotaFiscal.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    NotaFiscal.CopyTo(memoryStream);
                    NotaFiscalBytes = memoryStream.ToArray();
                }
            }

            // Cria uma nova entidade do tipo TbVenda a partir do objeto Venda recebido
            var TbVenda = new TbVendum()
            {
                Valor = venda.Valor,
                FkProduto = venda.FkProduto,
                FkCliente = venda.FkCliente,
                NotaFiscal = NotaFiscalBytes // Armazena a foto na entidade
            };

            // Adiciona a entidade ao contexto
            _context.TbVenda.Add(TbVenda);

            // Salva as mudanças no banco de dados
            _context.SaveChanges();
        }
       
        public void Delete(int id)
        {
            // Busca a entidade existente no banco de dados pelo Id
            var tbVenda = _context.TbVenda.FirstOrDefault(f => f.Id == id);

            // Verifica se a entidade foi encontrada
            if (tbVenda != null)
            {
                // Remove a entidade do contexto
                _context.TbVenda.Remove(tbVenda);

                // Salva as mudanças no banco de dados
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Venda não encontrado.");
            }
        }

        public List<Venda> GetAll()
        {
            List<Venda> listFun = new List<Venda>();

            var listTb = _context.TbVenda.ToList();

            foreach (var item in listTb)
            {
                var venda = new Venda
                {
                    Id = item.Id,
                    Valor = item.Valor,
                    NotaFiscal = item.NotaFiscal,
                    FkProduto = item.FkProduto,
                    FkCliente= item.FkCliente
                };

                listFun.Add(venda);
            }

            return listFun;
        }

        public Venda GetById(int id)
        {
            // Busca o venda pelo ID no banco de dados
            var item = _context.TbVenda.FirstOrDefault(f => f.Id == id);

            // Verifica se o venda foi encontrado
            if (item == null)
            {
                return null; // Retorna null se não encontrar
            }

            // Mapeia o objeto encontrado para a classe Cliente
            var venda = new Venda
            {
                Id = item.Id,
                Valor = item.Valor,
                NotaFiscal = item.NotaFiscal,
                FkProduto = item.FkProduto,
                FkCliente = item.FkCliente
            };

            return venda; // Retorna o venda encontrado
        }

        public void Update(Venda venda, IFormFile foto)
        {
            // Busca a entidade existente no banco de dados pelo Id
            var tbVenda = _context.TbVenda.FirstOrDefault(f => f.Id == venda.Id);

            // Verifica se a entidade foi encontrada
            if (tbVenda != null)
            {
                // Atualiza os campos da entidade com os valores do objeto venda recebido
                tbVenda.Valor = venda.Valor;
                tbVenda.NotaFiscal = venda.NotaFiscal;
                tbVenda.FkProduto = venda.FkProduto;
                tbVenda.FkCliente = venda.FkCliente;

                // Verifica se uma nova foto foi enviada
                if (foto != null && foto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        foto.CopyTo(memoryStream);
                        tbVenda.NotaFiscal = memoryStream.ToArray(); // Atualiza a foto na entidade
                    }
                }

                // Atualiza as informações no contexto
                _context.TbVenda.Update(tbVenda);

                // Salva as mudanças no banco de dados
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Venda não encontrado.");
            }
        }
    }
}
