using Microsoft.EntityFrameworkCore;
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

        public ClienteModel AlterarCliente(ClienteModel cliente)
        {

            using (var connection = _smartHintWebContext.Database.GetDbConnection())
            {
                connection.Open();
                using (var comando = connection.CreateCommand())
                {
                    comando.CommandText = "UPDATE cliente SET ";
                    List<string> updates = new List<string>();

                    if (cliente.NomeCliente != null)
                    {
                        updates.Add($"NomeCliente = '{cliente.NomeCliente}'");
                    }
                    if (cliente.Email != null)
                    {
                        updates.Add($"Email = '{cliente.Email}'");
                    }
                    if (cliente.Telefone != null)
                    {
                        var valor = new string(cliente.Telefone.Where(char.IsDigit).ToArray());
                        updates.Add($"Telefone = '{valor}'");
                    }
                    if (cliente.TipoPessoa != null)
                    {
                        updates.Add($"TipoPessoa = '{cliente.TipoPessoa}'");

                        if (cliente.TipoPessoa.Equals("F"))
                        {
                            if (cliente.Genero != null)
                            {
                                updates.Add($"Genero = '{cliente.Genero}'");
                            }
                            if (cliente.DataNascimento != null)
                            {
                                updates.Add($"DataNascimento = '{cliente.DataNascimento:yyyy-MM-dd}'");
                            }
                        }
                    }
                    if (cliente.CpfCnpj != null)
                    {
                        var valor = new string(cliente.CpfCnpj.Where(char.IsDigit).ToArray());
                        updates.Add($"CpfCnpj = '{valor}'");
                    }

                    updates.Add($"Isento = {cliente.Isento}");
                    if (cliente.Isento && cliente.InscricaoEstadual != null)
                    {
                        updates.Add($"InscricaoEstadual = '{cliente.InscricaoEstadual}'");
                    }

                    if (cliente.Senha != null)
                    {
                        updates.Add($"Senha = '{cliente.Senha}'");
                    }

                    updates.Add($"ClienteBloqueado = {cliente.ClienteBloqueado}");

                    comando.CommandText += string.Join(", ", updates);
                    comando.CommandText += $" WHERE Id = '{cliente.Id}' ";

                    comando.ExecuteNonQuery();


                }
                connection.Close();
            }

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

                            cliente.Id = reader.GetString(10);
                            cliente.NomeCliente = reader.IsDBNull(0) ? null : reader.GetString(0);
                            cliente.Email = reader.IsDBNull(1) ? null : reader.GetString(1);
                            cliente.Telefone = reader.IsDBNull(2) ? null : reader.GetString(2);
                            if (cliente.Telefone != null)
                            {
                                var valor = String.Join("", cliente.Telefone.ToCharArray().Where(Char.IsDigit));
                                var telefoneMascarado = '(' + valor.Substring(0, 2) + ") " + valor.Substring(2, 6) + "-" + valor.Substring(6);
                                cliente.Telefone = telefoneMascarado;
                            }
                            cliente.DataCadastro = reader.IsDBNull(3) ? null : reader.GetDateTime(3).ToString("dd/MM/yyyy");
                            cliente.ClienteBloqueado = reader.IsDBNull(12) ? null : reader.GetBoolean(12) ? "Sim" : "Não";
                            clienteModels.Add(cliente);
                        }
                    }
                    connection.Close();
                }
            }


            return clienteModels;
        }

        public ClienteModel ObterPorId(String Id)
        {

            ClienteModel cliente = new ClienteModel(); ;
            using (var connection = _smartHintWebContext.Database.GetDbConnection())
            {
                connection.Open();

                using (var comando = connection.CreateCommand())
                {
                    comando.CommandText = $"SELECT * FROM cliente cl WHERE cl.Id = '{Id}'";



                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cliente.Id = reader.IsDBNull(10) ? null : reader.GetString(10);
                            cliente.NomeCliente = reader.IsDBNull(0) ? null : reader.GetString(0);
                            cliente.Email = reader.IsDBNull(1) ? null : reader.GetString(1);
                            cliente.Telefone = reader.IsDBNull(2) ? null : reader.GetString(2);
                            cliente.DataCadastro = reader.GetDateTime(3);
                            cliente.TipoPessoa = reader.IsDBNull(4) ? null : reader.GetString(4);
                            cliente.InscricaoEstadual = reader.IsDBNull(6) ? null : reader.GetString(6);
                            cliente.CpfCnpj = reader.IsDBNull(5) ? null : reader.GetString(5);
                            cliente.Isento = reader.GetBoolean(7);
                            cliente.Genero = reader.IsDBNull(8) ? null : reader.GetString(8);
                            cliente.DataNascimento = reader.IsDBNull(9) ? null : reader.GetDateTime(9);
                            cliente.Senha = reader.IsDBNull(11) ? null : reader.GetString(11);
                            cliente.ClienteBloqueado = reader.GetBoolean(12);

                        }
                    }

                }
                connection.Close();
            }


            return cliente;
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
                    if (cliente.Email != null)
                    {
                        comando.CommandText += delimitador + $" cl.Email = '{cliente.Email}' ";
                        delimitador = " OR ";
                    }
                    if (cliente.CpfCnpj != null)
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

                    if (cliente.Email == null && cliente.CpfCnpj == null && !(cliente.Isento != null && !cliente.Isento && cliente.InscricaoEstadual != null))
                    {
                        return clientesObtidos;
                    }

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (clientesObtidos == null)
                            {
                                clientesObtidos = new List<ClienteModel>();
                            }
                            ClienteModel clienteObtido = new ClienteModel();
                            clienteObtido.Email = reader.IsDBNull(1) ? null : reader.GetString(1);
                            clienteObtido.InscricaoEstadual = reader.IsDBNull(6) ? null : reader.GetString(6);
                            clienteObtido.CpfCnpj = reader.IsDBNull(5) ? null : reader.GetString(5);
                            clientesObtidos.Add(clienteObtido);

                        }
                    }
                    connection.Close();
                }
            }


            return clientesObtidos;
        }
    }
}
