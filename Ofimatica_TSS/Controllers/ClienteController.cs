
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ofimatica_TSS.Data;
using Ofimatica_TSS.Models;
using Ofimatica_TSS.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Controllers
{
    public class ClienteController : Controller
    {
        private ClienteService clienteService;
        private PuntoTuristicoService puntoTuristicoService;
        public ClienteController(HotelContext context)
        {
            clienteService = new ClienteService(context);
            puntoTuristicoService = new PuntoTuristicoService(context);
        }

        public IActionResult Index()
        {
            return RedirectToAction("Checkin");
        }

        [HttpGet]
        public IActionResult Checkin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkin(ClienteViewModel cliente)
        {

            if (ModelState.IsValid)
            {
                cliente.Password = Cipher.Encrypt(cliente.Password, "password");
                bool correcto = clienteService.Insert(cliente, ModelState);
                if (correcto)
                {
                    ViewBag.mensaje = "Checkin realizado con exito";
                }
            }


            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Remove("SesionUser");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind] ClienteViewModel cliente)
        {

            var clientes = clienteService.GetAll();
            if (clientes.Any(u => u.DNI == cliente.DNI))
            {
                ClienteViewModel clienteFound = clientes.Where(o => o.DNI.Equals(cliente.DNI)).Select(cliente => new ClienteViewModel
                {
                    DNI = cliente.DNI,
                    Nombre = cliente.Nombre,
                    PrimerApellido = cliente.PrimerApellido,
                    SegundoApellido = cliente.SegundoApellido,
                    PuntoTuristico = cliente.PuntoTuristico,
                    Password = cliente.Password
                }).First();


                bool verif_ok = Cipher.Decrypt(clienteFound.Password, "password").Equals(cliente.Password);

                if (verif_ok)
                {
                    UserInfo userInfo = new UserInfo();
                    userInfo.user = cliente.DNI;
                    userInfo.signed = true;

                    HttpContext.Session.SetString("SesionUser", JsonConvert.SerializeObject(userInfo));
                    return RedirectToAction("SeleccionarPunto", "Cliente", new { });
                }
                else
                {
                    ModelState.AddModelError("Password", "Contraseña incorrecta");
                    return View(cliente);
                }
            }
            else
            {
                ModelState.AddModelError("DNI", "El DNI introducido no se ha dado de alta");
                return View(cliente);
            }


        }
        [HttpGet]
        public IActionResult SeleccionarPunto()
        {
 
            if (HttpContext.Session.GetString("SesionUser") != null)
            {
                UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SesionUser"));
                if (!string.IsNullOrEmpty(userInfo.user) && userInfo.signed)
                {

                    var puntos = puntoTuristicoService.GetDestinos();

                    return View(puntos);
                    //return View();
                }
            }
            return RedirectToAction("Login", "Cliente", new { });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SeleccionarPunto(int? id)
        {
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SesionUser"));
            HttpContext.Session.Remove("SesionUser");

            ClienteViewModel cliente = clienteService.GetByDNI(userInfo.user);
            cliente.PuntoTuristico = id;
            clienteService.Update(cliente, ModelState);

            ViewBag.mensaje = "Parada seleccionada con exito";
            return View("Mensaje");
        }


        [HttpGet]
        public IActionResult ListaClientes()
        {
           var clientes = clienteService.GetAll();
            return View(clientes);
        }
    }
}
