using SmartHintTeste.Models;

namespace SmartHintTeste.Repositorio
{
    public interface IClienteRepositorio
    {
        ClienteModel AdicionarCliente(ClienteModel cliente);

        List<ClienteTableModelValue> BuscarTodos();

        List<ClienteModel> VerificarExistenciaDeCadastro(ClienteModel cliente);

        ClienteModel ObterPorId(String Id);

        ClienteModel AlterarCliente(ClienteModel cliente);
    }
}
