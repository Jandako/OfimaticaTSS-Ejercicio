using Ofimatica_TSS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Models
{
    public class ParadaPuntoTuristicoViewModelService
    {
        private HotelContext db;

        public ParadaPuntoTuristicoViewModelService(HotelContext context)
        {
            db = context;
        }


        public virtual IQueryable<ParadaPuntoTuristicoViewModel> GetParadasByIdPunto(int id)
        {
            IQueryable<ParadaPuntoTuristicoViewModel> result = db.ParadasPuntoTuristico.Where(parada => parada.IdParada==id).Select(parada => new ParadaPuntoTuristicoViewModel
            {
                IdParada = parada.IdParada,
                IdParadaConectada = parada.IdParadaConectada,
                Distancia = parada.Distancia           
            });
            return result;
        }


       /* public virtual IQueryable<ParadaPuntoTuristicoViewModel> GetAll()
        {
            IQueryable<ParadaPuntoTuristicoViewModel> result = db.ParadasPuntoTuristico.Select(parada => new ParadaPuntoTuristicoViewModel
            {
                idParada = parada.idParada,
                // = parada.idPuntoTuristico,
               // puntoTuristico = new PuntoTuristicoViewModel { Id=parada.puntoTuristico.Id}
            }) ;
            return result;
        }*/
    }
}
