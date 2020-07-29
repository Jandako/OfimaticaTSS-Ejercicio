using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Models
{
    public class ClienteViewModel
    {
        [MinLength(9, ErrorMessage = "Formato de DNI incorrecto"),MaxLength(9, ErrorMessage = "Formato de DNI incorrecto") ,Required(ErrorMessage = " El DNI no puede estar en blanco")]
        public string DNI { get; set; }


        [StringLength(50), Required(ErrorMessage = " El nombre no puede estar en blanco")]

        public string Nombre { get; set; }

        [StringLength(50), Required(ErrorMessage = " El primer apellido no puede estar en blanco")]

        public string PrimerApellido { get; set; }


        [StringLength(50), Required(ErrorMessage = " El segundo apellido no puede estar en blanco")]
        public string SegundoApellido { get; set; }


        public int? PuntoTuristico { get; set; }


        [StringLength(50), Required(ErrorMessage = "La contraseña no puede estar en blanco")]
        public string Password { get; set; }



        public Cliente ToEntity()
        {
            return new Cliente
            {
                DNI = DNI,
                Nombre = Nombre,
                PrimerApellido = PrimerApellido,
                SegundoApellido = SegundoApellido,
                PuntoTuristico = PuntoTuristico,
                Password = Password
            };
        }
    }
}
