using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Models
{
    public class ParadaPuntoTuristicoViewModel
    {

        public int IdParada { get; set; }
        public int IdParadaConectada { get; set; }
        public int Distancia { get; set; }
        public ParadaPuntoTuristico ToEntity()
        {
            return new ParadaPuntoTuristico
            {
                IdParada = IdParada,
                IdParadaConectada = IdParadaConectada,
                Distancia = Distancia
            };
        }
    }
}
