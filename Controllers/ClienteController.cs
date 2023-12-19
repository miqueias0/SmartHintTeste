using Microsoft.AspNetCore.Mvc;
using SmartHintTeste.Models;
using SmartHintTeste.Repositorio;
using System.Text.RegularExpressions;

namespace SmartHintTeste.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ClienteController(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult Index(int pagina = 1, int itensPorPagina = 20)
        {
            List<ClienteTableModelValue> clientes = _clienteRepositorio.BuscarTodos();

            var clientesNaPagina = clientes.Skip((pagina - 1) * itensPorPagina).Take(itensPorPagina).ToList();
            var totalClientes = clientes.Count();

            ClienteTableModel cliente = new ClienteTableModel()
            {
                ClienteTableModelValue = clientesNaPagina,
                PaginaAtual = pagina,
                ItensPorPagina = itensPorPagina,
                TotalClientes = totalClientes
            };

            return View(cliente);
        }

        public IActionResult AdicionarCliente()
        {
            ClienteModel cliente = new ClienteModel();
            cliente.DataCadastro = DateTime.Now;
            cliente.DataNascimento = DateTime.Now;
            return View(cliente);
        }

        [HttpPost]
        public IActionResult AdicionarCliente(ClienteModel cliente)
        {
            cliente.Id = Guid.NewGuid().ToString();
            cliente.Telefone = String.Join("", cliente.Telefone.ToCharArray().Where(Char.IsDigit));
            cliente.CpfCnpj = String.Join("", cliente.CpfCnpj.ToCharArray().Where(Char.IsDigit));
            if (cliente.InscricaoEstadual != null)
            {
                cliente.InscricaoEstadual = String.Join("", cliente.InscricaoEstadual.ToCharArray().Where(Char.IsDigit));
            }

            _clienteRepositorio.AdicionarCliente(cliente);

            return RedirectToAction("Index");
            
        }

        public IActionResult EditarCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerificarExistenciaDeCadastro(ClienteModel cliente)
        {

            ClienteModel clienteModel = _clienteRepositorio.VerificarExistenciaDeCadastro(cliente);
            
            if (clienteModel == null)
            {
                return Json(new {sucesso = true});
            }

            if (clienteModel.Email.Equals(cliente.Email))
            {
                return Json(new { sucesso = true, mensagem = "Este e-mail já está cadastrado para outro Cliente" });
            }

            if (clienteModel.CpfCnpj.Equals(cliente.CpfCnpj))
            {
                return Json(new { sucesso = true, mensagem = "Este CPF/CNPJ já está cadastrado para outro Cliente" });
            }

            if (clienteModel.InscricaoEstadual != null && clienteModel.InscricaoEstadual.Equals(cliente.InscricaoEstadual))
            {
                return Json(new { sucesso = true, mensagem = "Esta Inscrição Estadual já está cadastrada para outro Cliente" });
            }

            return Json(new { sucesso = true });
        }
    }
}
