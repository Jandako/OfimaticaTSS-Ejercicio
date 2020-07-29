using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Models
{
    public class PuntoTuristico
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [NotNull]
        public int IdPuntoTuristico { get; set; }
          
        [StringLength(50), NotNull]
        public string NombrePunto { get; set; }

        public string Imagen { get; set; }

        //public virtual ICollection<ParadaPuntoTuristico> PuntosTuristicosConectados { get; set; }
    }
}
