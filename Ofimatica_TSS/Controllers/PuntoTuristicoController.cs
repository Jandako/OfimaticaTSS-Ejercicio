using Microsoft.AspNetCore.Mvc;
using Ofimatica_TSS.Data;
using Ofimatica_TSS.Models;
using Ofimatica_TSS.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Controllers
{
    public class PuntoTuristicoController : Controller
    {
        private ClienteService clienteService;
        private PuntoTuristicoService puntoTuristicoService;
        private ParadaPuntoTuristicoViewModelService paradasService;
        public PuntoTuristicoController(HotelContext context)
        {
            clienteService = new ClienteService(context);
            puntoTuristicoService = new PuntoTuristicoService(context);
            paradasService = new ParadaPuntoTuristicoViewModelService(context);

        }

        public IActionResult Index()
        {
            var clientes = clienteService.GetCLientesConParada();
            List<PuntoTuristicoViewModel> puntosTuristicos = puntoTuristicoService.GetAll().ToList();

            
            for (int i = 0; i < puntosTuristicos.Count(); i++)
            {
                puntosTuristicos[i].PuntosTuristicosConectados = paradasService.GetParadasByIdPunto(puntosTuristicos[i].IdPuntoTuristico).ToList();
            }

            List<ParadaRuta> listaParadas = new List<ParadaRuta>();
            foreach (ClienteViewModel cliente in clientes)
            {
                ParadaRuta paradaCurrent = listaParadas.Find(o => o.numeroParada == cliente.PuntoTuristico);
                if (paradaCurrent != null)
                {
                    paradaCurrent.numeroPersonas++;
                }
                else
                {
                    paradaCurrent = new ParadaRuta();
                    paradaCurrent.numeroParada = (int)cliente.PuntoTuristico;
                    paradaCurrent.numeroPersonas++;
                    listaParadas.Add(paradaCurrent);
                }
            }

            if (listaParadas.Count == 0)
            {
                ViewBag.mensaje = "No hay paradas";
                return View();
            }

            ParadaRuta a = new ParadaRuta();
            a.numeroParada = 0;

            List<List<List<ParadaRuta>>> listaResultadosMayor = new List<List<List<ParadaRuta>>>();
            List<List<ParadaRuta>> listaResultados = new List<List<ParadaRuta>>();
            List<int> nodosVisitados = new List<int>();

            foreach (ParadaRuta destino in listaParadas)
            {
                calcularRutaAB(a, destino, puntosTuristicos, ref listaResultados, new List<ParadaRuta>(), /*ref nodosVisitados,*/ listaParadas, 1);
            }

            int kmTotales = int.MaxValue;
            List<ParadaRuta> resultadoFinal = null;

            foreach (List<ParadaRuta> resultadoLinea in listaResultados)
            {
                if (resultadoLinea.Last().kmRecorridos < kmTotales)
                {
                    kmTotales = resultadoLinea.Last().kmRecorridos;
                    resultadoFinal = resultadoLinea;
                }
                else if (resultadoLinea.Last().kmRecorridos == kmTotales && resultadoLinea.Count < resultadoFinal.Count)
                {
                    resultadoFinal = resultadoLinea;
                }
            }

            for (int i = 0; i < resultadoFinal.Count; i++)
            {
                ParadaRuta paradaCurrent = resultadoFinal[i];
                for (int x = 0; x < listaParadas.Count; x++)
                {
                    ParadaRuta paradaPogramada = listaParadas[x];
                    if (paradaCurrent.numeroParada == paradaPogramada.numeroParada)
                    {
                        paradaCurrent.numeroPersonas = paradaPogramada.numeroPersonas;
                        paradaPogramada.numeroPersonas = 0;
                        break;
                    }
                }
            }

            ViewBag.resultado = resultadoFinal;
            return View();
        }

        public bool rutaCompleta(List<ParadaRuta> listaParadas, List<ParadaRuta> rutaParadasList)
        {
            foreach (ParadaRuta parada in listaParadas)
            {
                ParadaRuta paradaRuta = rutaParadasList.Find(o => o.numeroParada == parada.numeroParada);
                if (paradaRuta == null)
                {
                    return false;
                }
            }
            return true;
        }

        public void calcularRutaAB(ParadaRuta a, ParadaRuta b, List<PuntoTuristicoViewModel> mapa, ref List<List<ParadaRuta>> listaResultados, List<ParadaRuta> camino, List<ParadaRuta> listaParadas, int nivel)
        {
            if (nivel < mapa.Count() * mapa.Count())
            {
                camino.Add(a);
                int caminosMejores = listaResultados.Count(o => o.Last().kmRecorridos < a.kmRecorridos);

                if (rutaCompleta(listaParadas, camino))
                {
                    listaResultados.Add(camino);
                }
                else if (caminosMejores == 0)
                {
                    PuntoTuristicoViewModel puntoTuristicoCurrent = mapa.Where(o => o.IdPuntoTuristico == a.numeroParada).First();

                    foreach (ParadaPuntoTuristicoViewModel paradaViewModel in puntoTuristicoCurrent.PuntosTuristicosConectados)
                    {
                        List<ParadaRuta> newCamino = new List<ParadaRuta>();
                        foreach (ParadaRuta aux in camino)
                        {
                            newCamino.Add(aux);
                        }

                        ParadaRuta nuevoOrigen = new ParadaRuta();
                        nuevoOrigen.numeroParada = paradaViewModel.IdParadaConectada;
                        nuevoOrigen.kmRecorridos = paradaViewModel.Distancia + a.kmRecorridos;
                        calcularRutaAB(nuevoOrigen, b, mapa, ref listaResultados, newCamino, listaParadas, ++nivel);
                    }
                }
            }

        }




    }
}
