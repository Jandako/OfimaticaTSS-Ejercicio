using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Models
{
    
    public class ParadaPuntoTuristico
    {
        [Key]
        public int IdFila { get; set; }
        public int IdParada { get; set; }
        public int IdParadaConectada { get; set; }
        public int Distancia { get; set; }
  

    }
}
