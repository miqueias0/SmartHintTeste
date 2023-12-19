using Microsoft.AspNetCore.Mvc;
using SmartHintTeste.Models;
using SmartHintTeste.Repositorio;

namespace SmartHintTeste.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ClienteController(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdicionarCliente()
        {
            ClienteModel cliente = new ClienteModel();
            cliente.Id = Guid.NewGuid().ToString();
            cliente.DataCadastro = DateTime.Now;
            cliente.DataNascimento = DateTime.Now;
            return View(cliente);
        }

        [HttpPost]
        public IActionResult AdicionarCliente(ClienteModel cliente)
        {
            _clienteRepositorio.AdicionarCliente(cliente);

            return RedirectToAction("Index");
        }

        public IActionResult EditarCliente()
        {
            return View();
        }
    }
}
