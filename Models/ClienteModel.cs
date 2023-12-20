using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHintTeste.Models
{
    public class ClienteModel
    {
        public String Id { get; set; }
        public String NomeCliente { get; set; }
        [Required(ErrorMessage = "Digite o nome do cliente")]
        [MaxLength(150)]
        public String Email { get; set; }
        [Required(ErrorMessage = "Digite o e-mail do cliente")]
        [MaxLength(150)]
        [EmailAddress(ErrorMessage = "E-Mail informado não é valido!")]
        public String Telefone { get; set; }
        [Required(ErrorMessage = "Digite o telefone do cliente")]
        public DateTime DataCadastro { get; set; }
        public String TipoPessoa { get; set; }
        [Required(ErrorMessage = "Escolha o tipo do cliente")]
        public String CpfCnpj { get; set; }
        [Required(ErrorMessage = "Digite o CPF/CNPJ do cliente")]
        public String InscricaoEstadual {  get; set; }
        public bool Isento { get; set; }
        public String Genero { get; set; }
        public Nullable<DateTime> DataNascimento { get; set; }
        public String Senha { get; set; }
        [Required(ErrorMessage = "Digite a senha do cliente")]
        public bool ClienteBloqueado { get; set; }

    }
}
