using SmartHintTeste.Data;
using SmartHintTeste.Models;

namespace SmartHintTeste.Repositorio
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly SmartHintWebContext _smartHintWebContext;
        public ClienteRepositorio(SmartHintWebContext smartHintWebContext)
        {
            _smartHintWebContext = smartHintWebContext;
        }
        public ClienteModel AdicionarCliente(ClienteModel cliente)
        {
            _smartHintWebContext.Cliente.Add(cliente);
            _smartHintWebContext.SaveChanges();

            return cliente;
        }
    }
}
