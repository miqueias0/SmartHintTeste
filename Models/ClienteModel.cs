using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartHintTeste.Models
{
    public class ClienteModel
    {
        public String? Id { get; set; }
        public String? NomeCliente { get; set; }
        public String? Email { get; set; }
        public String? Telefone { get; set; }
        public DateTime? DataCadastro { get; set; }
        public String? TipoPessoa { get; set; }
        public String? CpfCnpj { get; set; }
        public String? InscricaoEstadual {  get; set; }
        public bool? Isento { get; set; } = false;
        public String? Genero { get; set; }
        public DateTime? DataNascimento { get; set; }
        public String? Senha { get; set; }
        public bool? ClienteBloqueado { get; set; }

    }
}
