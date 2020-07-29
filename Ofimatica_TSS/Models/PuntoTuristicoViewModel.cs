using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Models
{
    public class PuntoTuristicoViewModel
    {
 
        public int IdPuntoTuristico { get; set; }

        public string NombrePunto { get; set; }

        public string Imagen { get; set; }
        public  List<ParadaPuntoTuristicoViewModel> PuntosTuristicosConectados { get; set; }

        public PuntoTuristico ToEntity()
        {
            return new PuntoTuristico
            {
                IdPuntoTuristico = IdPuntoTuristico,              
                NombrePunto = NombrePunto,
                Imagen = Imagen
            };
        }
    }
}
