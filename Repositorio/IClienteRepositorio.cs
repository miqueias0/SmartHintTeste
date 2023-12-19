using SmartHintTeste.Models;

namespace SmartHintTeste.Repositorio
{
    public interface IClienteRepositorio
    {
        ClienteModel AdicionarCliente(ClienteModel cliente);

        List<ClienteTableModelValue> BuscarTodos();

        ClienteModel VerificarExistenciaDeCadastro(ClienteModel cliente);
    }
}
