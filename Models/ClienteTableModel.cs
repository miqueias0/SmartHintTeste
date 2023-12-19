namespace SmartHintTeste.Models
{
    public class ClienteTableModel
    {

        public List<ClienteTableModelValue> ClienteTableModelValue { get; set; }
        public int PaginaAtual { get; set; }
        public int ItensPorPagina { get; set; }
        public int TotalClientes { get; set; }
        
    }


    public class ClienteTableModelValue
    {
        public String? Id { get; set; }
        public String? NomeCliente { get; set; }
        public String? Email { get; set; }
        public String? Telefone { get; set; }
        public String? DataCadastro { get; set; }
        public String? ClienteBloqueado { get; set; }
        public bool Selected { get; set; } = false;
    }
}
