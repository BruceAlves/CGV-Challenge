using Dominio;
using Repository.Implementacao;
using Repository.Interface;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Web.ViewModels;


namespace Web.Controllers
{
   // [Authorize]
    public class AdvogadoController : Controller
    {
        private readonly IAdvogadoRepositorio _advogadoRepository;

        public AdvogadoController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _advogadoRepository = new AdvogadoRepositorio(connectionString);
        }

        public ActionResult Index()
        {
            var advogados = _advogadoRepository.ListarAdvogadoComEndereco();

            var advogadosViewModel = advogados.Select(advogado => new AdvogadoViewModel
            {
                Id = advogado.ID,
                Nome = advogado.Nome,
                Senioridade = advogado.Senioridade,
                Estado = advogado.endereco.Estado,
                Logradouro = advogado.endereco.Logradouro,
                Bairro = advogado.endereco.Bairro,
                Cep = advogado.endereco.Cep,
                Numero = advogado.endereco.Numero,
                Complemento = advogado.endereco.Complemento
            }).ToList(); 

            return View(advogadosViewModel);
        }

        public ActionResult Create()
        {
            ViewBag.Senioridades = Enum.GetValues(typeof(SenioridadeEnum)).Cast<SenioridadeEnum>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdvogadoViewModel advogadoViewModel)
        {
            if (advogadoViewModel.Id.Equals(null)) return RedirectToAction(nameof(Create));

            Advogado advogado = new Advogado
            {
                Nome = advogadoViewModel.Nome,
                Senioridade = (SenioridadeEnum)advogadoViewModel.Senioridade,
                endereco = new Endereco()
            };

            if (advogadoViewModel.Endereco != null)
            {
                advogado.endereco.Estado = advogadoViewModel.Endereco.Estado;
                advogado.endereco.Cep = advogadoViewModel.Endereco.Cep;
                advogado.endereco.Bairro = advogadoViewModel.Endereco.Bairro;
                advogado.endereco.Logradouro = advogadoViewModel.Endereco.Logradouro;
                advogado.endereco.Numero = advogadoViewModel.Endereco.Numero;
                advogado.endereco.Complemento = advogadoViewModel.Endereco.Complemento;
            }

            _advogadoRepository.Incluir(advogado);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Senioridades = Enum.GetValues(typeof(SenioridadeEnum)).Cast<SenioridadeEnum>();

            Advogado advogado = _advogadoRepository.BuscarPorId(id);

            if (advogado == null)
            {
                return RedirectToAction("Index");
            }

            AdvogadoViewModel advogadoViewModel = new AdvogadoViewModel
            {
                Nome = advogado.Nome,
                Senioridade = advogado.Senioridade,
                Estado = advogado.Estado,
                Logradouro = advogado.Logradouro,
                Cep = advogado.Cep,
                Numero = advogado.Numero,
                Complemento = advogado.Complemento,
                Bairro = advogado.Bairro,
            };

            return View(advogadoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdvogadoViewModel advogadoViewModel)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            Advogado advogado = new Advogado
            {
                ID = advogadoViewModel.Id,
                Nome = advogadoViewModel.Nome,
                Senioridade = (SenioridadeEnum)advogadoViewModel.Senioridade,
                Estado = advogadoViewModel.Estado,
                Cep = advogadoViewModel.Cep,
                Bairro = advogadoViewModel.Bairro,
                Logradouro = advogadoViewModel.Logradouro,
                Numero = advogadoViewModel.Numero,
                Complemento = advogadoViewModel.Complemento,
            };

            _advogadoRepository.Atualizar(advogado);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var advogado = _advogadoRepository.BuscarPorId(id);
            if (advogado == null) return RedirectToAction("Index");

            AdvogadoViewModel advogadoViewModel = new AdvogadoViewModel
            {
                Nome = advogado.Nome,
                Senioridade = advogado.Senioridade,
                Estado = advogado.Estado,
                Logradouro = advogado.Logradouro,
                Cep = advogado.Cep,
                Numero = advogado.Numero,
                Complemento = advogado.Complemento,
                Bairro = advogado.Bairro,
            };

            return View(advogadoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ComfirmarExclusão(int id)
        {
            var advogadoviwmodel = _advogadoRepository.BuscarPorId(id);
            if (advogadoviwmodel == null) return RedirectToAction("Index");

            _advogadoRepository.Excluir(id);

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var advogado = _advogadoRepository.BuscarPorId(id);
            if (advogado == null) return RedirectToAction("Index");

            AdvogadoViewModel advogadoViewModel = new AdvogadoViewModel
            {
                Nome = advogado.Nome,
                Senioridade = advogado.Senioridade,
                Estado = advogado.Estado,
                Logradouro = advogado.Logradouro,
                Cep = advogado.Cep,
                Numero = advogado.Numero,
                Complemento = advogado.Complemento,
                Bairro = advogado.Bairro,
            };

            return View(advogadoViewModel);
        }
    }
}
