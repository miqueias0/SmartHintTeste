using Microsoft.EntityFrameworkCore;
using MySqlConnector;
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

        public List<ClienteTableModelValue> BuscarTodos()
        {
            List<ClienteTableModelValue> clienteModels = new List<ClienteTableModelValue>();
            using (var connection = _smartHintWebContext.Database.GetDbConnection())
            {
                connection.Open();

                using (var comando = connection.CreateCommand())
                {
                    comando.CommandText = "SELECT * FROM cliente";

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClienteTableModelValue cliente = new ClienteTableModelValue();

                            cliente.NomeCliente = reader.IsDBNull(0) ? null : reader.GetString(0);
                            cliente.Email = reader.IsDBNull(1) ? null : reader.GetString(1);
                            cliente.Telefone = reader.IsDBNull(2) ? null : reader.GetString(2);
                            if(cliente.Telefone != null)
                            {
                                var valor = String.Join("", cliente.Telefone.ToCharArray().Where(Char.IsDigit));
                                var telefoneMascarado = '(' + valor.Substring(0, 2) + ") " + valor.Substring(2, 6) + "-" + valor.Substring(6);
                                cliente.Telefone = telefoneMascarado;
                            }
                            cliente.DataCadastro = reader.IsDBNull(3) ? null : reader.GetDateTime(3).ToString("dd/MM/yyyy");
                            cliente.ClienteBloqueado = reader.IsDBNull(12) ? null : reader.GetBoolean(12) ? "Sim": "Não";
                            clienteModels.Add(cliente);
                        }
                    }
                    connection.Close();
                }
            }


            return clienteModels;
        }

        public ClienteModel VerificarExistenciaDeCadastro(ClienteModel cliente)
        {
            ClienteModel clienteObtido = null;
            using (var connection = _smartHintWebContext.Database.GetDbConnection())
            {
                connection.Open();

                using (var comando = connection.CreateCommand())
                {
                    comando.CommandText = $"SELECT * FROM cliente cl WHERE cl.Email = {cliente.Email} OR cl.CpfCnpj = {cliente.CpfCnpj} " + (cliente.Isento != null && cliente.Isento != false ? $"OR {cliente.InscricaoEstadual}": "");

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clienteObtido = new ClienteModel();
                            clienteObtido.Email = reader.IsDBNull(1) ? null : reader.GetString(1);
                            clienteObtido.InscricaoEstadual = reader.IsDBNull(6) ? null : reader.GetString(6);
                            clienteObtido.CpfCnpj = reader.IsDBNull(5) ? null : reader.GetString(5);

                        }
                    }
                    connection.Close();
                }
            }


            return clienteObtido;
        }
    }
}
