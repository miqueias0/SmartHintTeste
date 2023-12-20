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

        public List<ClienteModel> VerificarExistenciaDeCadastro(ClienteModel cliente)
        {
            List<ClienteModel> clientesObtidos = null;
            using (var connection = _smartHintWebContext.Database.GetDbConnection())
            {
                connection.Open();

                using (var comando = connection.CreateCommand())
                {
                    comando.CommandText = "SELECT * FROM cliente cl ";
                    String delimitador = " WHERE ";
                    if(cliente.Email != null)
                    {
                        comando.CommandText += delimitador + $" cl.Email = '{cliente.Email}' ";
                        delimitador = " OR ";
                    }
                    if(cliente.CpfCnpj != null)
                    {
                        var cpfCnpj = String.Join("", cliente.CpfCnpj.ToCharArray().Where(Char.IsDigit));
                        comando.CommandText += delimitador + $" cl.CpfCnpj = '{cpfCnpj}' ";
                        delimitador = " OR ";
                    }
                    if (cliente.Isento != null && !cliente.Isento && cliente.InscricaoEstadual != null)
                    {
                        var InscricaoEstadual = String.Join("", cliente.InscricaoEstadual.ToCharArray().Where(Char.IsDigit));
                        comando.CommandText += delimitador + $" cl.InscricaoEstadual = '{InscricaoEstadual}' ";
                        delimitador = " OR ";
                    }


                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if(clientesObtidos == null)
                            {
                                clientesObtidos = new List<ClienteModel> ();
                            }
                            ClienteModel clienteObtido = new ClienteModel();
                            clienteObtido.Email = reader.IsDBNull(1) ? null : reader.GetString(1);
                            clienteObtido.InscricaoEstadual = reader.IsDBNull(6) ? null : reader.GetString(6);
                            clienteObtido.CpfCnpj = reader.IsDBNull(5) ? null : reader.GetString(5);
                            clientesObtidos.Add (clienteObtido);

                        }
                    }
                    connection.Close();
                }
            }


            return clientesObtidos;
        }
    }
}
