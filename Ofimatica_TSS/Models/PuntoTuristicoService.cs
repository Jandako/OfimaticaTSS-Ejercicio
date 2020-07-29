using Ofimatica_TSS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Models
{
    public class PuntoTuristicoService
    {
        private HotelContext db;
        private ParadaPuntoTuristicoViewModelService paradasService;

        public PuntoTuristicoService(HotelContext context)
        {
            db = context;
            paradasService = new ParadaPuntoTuristicoViewModelService(context);

        }

        public virtual IQueryable<PuntoTuristicoViewModel> GetAll()
        {

            IQueryable<PuntoTuristicoViewModel> result = db.PuntosTuristicos.Select(punto => new PuntoTuristicoViewModel
            {
                IdPuntoTuristico = punto.IdPuntoTuristico,
                NombrePunto = punto.NombrePunto,
                Imagen = punto.Imagen
            });

            foreach (PuntoTuristicoViewModel punto in result)
            {
                punto.PuntosTuristicosConectados = paradasService.GetParadasByIdPunto(punto.IdPuntoTuristico).ToList();
            }

            return result;
        }
        public virtual IQueryable<PuntoTuristicoViewModel> GetDestinos()
        {

            IQueryable<PuntoTuristicoViewModel> result = db.PuntosTuristicos.Where(punto => punto.IdPuntoTuristico != 0).Select(punto => new PuntoTuristicoViewModel
            {
                IdPuntoTuristico = punto.IdPuntoTuristico,
                NombrePunto = punto.NombrePunto,
                PuntosTuristicosConectados = db.ParadasPuntoTuristico.Where(o => o.IdParada == punto.IdPuntoTuristico).Select(parada => new ParadaPuntoTuristicoViewModel
                {
                    IdParada = parada.IdParada,
                    IdParadaConectada = parada.IdParadaConectada,
                    Distancia = parada.Distancia
                }).ToList(),
                Imagen = punto.Imagen
            });
            return result;
        }
    }
}
