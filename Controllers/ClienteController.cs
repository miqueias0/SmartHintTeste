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
            try
            {
                if (ModelState.IsValid)
                {
                    cliente.Id = Guid.NewGuid().ToString();
                    cliente.Telefone = String.Join("", cliente.Telefone.ToCharArray().Where(Char.IsDigit));
                    cliente.CpfCnpj = String.Join("", cliente.CpfCnpj.ToCharArray().Where(Char.IsDigit));
                    if (cliente.InscricaoEstadual != null)
                    {
                        cliente.InscricaoEstadual = String.Join("", cliente.InscricaoEstadual.ToCharArray().Where(Char.IsDigit));
                    }

                    if (cliente.TipoPessoa == "J")
                    {
                        cliente.Genero = null;
                        cliente.DataNascimento = null;
                    }

                    if (cliente.Isento)
                    {
                        cliente.InscricaoEstadual = null;
                    }


                    _clienteRepositorio.AdicionarCliente(cliente);

                    return RedirectToAction("Index");
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                return View(cliente);
            }

        }

        [HttpPost]
        public IActionResult AlterarCliente(ClienteModel cliente)
        {
            try
            {
                _clienteRepositorio.AlterarCliente(cliente);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("EditarCliente", cliente);
            }
        }

        public IActionResult EditarCliente(String id)
        {
            try
            {
                return View(_clienteRepositorio.ObterPorId(id));
            }
            catch (Exception ex)
            {
                throw new Exception("Cliente não encontrado");
            }

        }

        [HttpPost]
        public IActionResult FiltrarClientes(ObjetoFiltro objetoFiltro)
        {

            List<ClienteTableModelValue> clientes = _clienteRepositorio.BuscarTodos();

            clientes = clientes.Where(x =>
            {
                return (objetoFiltro.email != null && objetoFiltro.email.ToLower().Equals(x.Email.ToLower()))
                && (objetoFiltro.telefone != null && objetoFiltro.telefone.ToLower().Equals(x.Telefone.ToLower()))
                && (objetoFiltro.nomeCliente != null && objetoFiltro.nomeCliente.ToLower().Equals(x.NomeCliente.ToLower()))
                && (objetoFiltro.dataCadastro != null && objetoFiltro.dataCadastro.Equals(x.DataCadastro))
                && (objetoFiltro.checkbox != null && objetoFiltro.checkbox.Equals(x.ClienteBloqueado));
            }).ToList();

            var clientesNaPagina = clientes.Skip((objetoFiltro.pagina - 1) * 20).Take(20).ToList();
            var totalClientes = clientes.Count();

            ClienteTableModel cliente = new ClienteTableModel()
            {
                ClienteTableModelValue = clientesNaPagina,
                PaginaAtual = objetoFiltro.pagina,
                ItensPorPagina = 20,
                TotalClientes = totalClientes
            };

            return View("Index", cliente);
        }



        [HttpPost]
        public ActionResult VerificarExistenciaDeCadastro(ClienteModel cliente)
        {
            try
            {
                List<ClienteModel> clienteModel = _clienteRepositorio.VerificarExistenciaDeCadastro(cliente);

                if (clienteModel != null)
                {
                    if (clienteModel.FirstOrDefault(x =>
                    {
                        return x.Email != null && cliente.Email != null
                               && x.Email.ToLower().Equals(cliente.Email.ToLower());
                    }) != null)
                    {
                        return Json(new { sucesso = false, mensagem = "Este e-mail já está cadastrado para outro Cliente" });
                    }

                    if (clienteModel.FirstOrDefault(x =>
                    {
                        var CpfCnpj = String.Empty;
                        if (cliente.CpfCnpj != null)
                        {
                            CpfCnpj = String.Join("", cliente.CpfCnpj.ToCharArray().Where(Char.IsDigit));
                        }
                        return x.CpfCnpj != null && cliente.CpfCnpj != null
                               && x.CpfCnpj.ToLower().Equals(CpfCnpj.ToLower());
                    }) != null)
                    {
                        return Json(new { sucesso = false, mensagem = "Este CPF/CNPJ já está cadastrado para outro Cliente" });
                    }

                    if (clienteModel.FirstOrDefault(x =>
                    {
                        var InscricaoEstadual = String.Empty;
                        if (cliente.InscricaoEstadual != null)
                        {
                            InscricaoEstadual = String.Join("", cliente.InscricaoEstadual.ToCharArray().Where(Char.IsDigit));
                        }
                        return x.Email != null && cliente.InscricaoEstadual != null
                               && x.InscricaoEstadual.ToLower().Equals(InscricaoEstadual.ToLower());
                    }) != null)
                    {
                        return Json(new { sucesso = false, mensagem = "Esta Inscrição Estadual já está cadastrada para outro Cliente" });
                    }
                }
                else if (ModelState.IsValid)
                {
                    return Json(new { sucesso = true, mensagem = "Tudo Certo" }); ;
                }
                return Json(new { sucesso = false, mensagem = "Cliente com dados inválidos" });
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao validar existencia do cliente");
            }
        }
    }
}
