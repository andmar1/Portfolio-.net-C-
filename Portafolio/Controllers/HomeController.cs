using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using Portafolio.Servicios;
using System.Diagnostics;
using Portafolio.Servicios;

namespace Portafolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositorioProyectos repositorioProyectos;
        private readonly IServicioEmail servicioEmail;

        //Constructor de la clase "HomeController" inyeccion de dependencias
        public HomeController(IRepositorioProyectos repositorioProyectos,IServicioEmail servicioEmail)
        {
            this.repositorioProyectos = repositorioProyectos;
            this.servicioEmail = servicioEmail;
        }
        //Acciones, funciones cuando hacemos una peticion http, metodos que se ejecutan
        public IActionResult Index()
        {
            var proyectos = repositorioProyectos.obtenerProyectos().Take(3).ToList();

            var modelo = new HomeIndexViewModel()
            {
                Proyectos = proyectos,
            };

            return View(modelo);

        }

        //Accion para vista proyectos
        public IActionResult Proyectos()
        {
            var proyectos = repositorioProyectos.obtenerProyectos();

            return View(proyectos);
        }

        public IActionResult Contacto()
        {
            return View();
        }

        //Atributo hhtp post, para enviar informacion de la vista al controlador
        [HttpPost]
        public async Task <IActionResult>Contacto(ContactoViewModel contactoViewModel)
        {
            await servicioEmail.Enviar(contactoViewModel);
            //retornar redirección, evitar reenviar accion, Patron POST-redireccion-GET
            return RedirectToAction("Gracias");
        }
        public IActionResult Gracias()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}