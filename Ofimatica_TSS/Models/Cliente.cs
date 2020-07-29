using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Models
{
    public class Cliente
    {

        [Key]
        [StringLength(9), NotNull]
        public string DNI { get; set; }

        [StringLength(50), NotNull]
        public string Nombre { get; set; }

        [StringLength(50), NotNull]
        public string PrimerApellido { get; set; }

        [StringLength(50), NotNull]
        public string SegundoApellido { get; set; }

        [NotNull]
        public string Password { get; set; }

        public int? PuntoTuristico { get; set; }
    }
}