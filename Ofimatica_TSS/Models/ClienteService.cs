using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Ofimatica_TSS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Models
{
    public class ClienteService
    {
        private HotelContext db;

        public ClienteService(HotelContext context)
        {
            db = context;
        }

        public virtual IQueryable<ClienteViewModel> GetAll()
        {
            IQueryable<ClienteViewModel> result = db.Clientes.Select(cliente => new ClienteViewModel
            {
                DNI = cliente.DNI,
                Nombre = cliente.Nombre,
                PrimerApellido = cliente.PrimerApellido,
                SegundoApellido = cliente.SegundoApellido,
                PuntoTuristico = cliente.PuntoTuristico,
                Password = cliente.Password
            });

            return result;
        }
        public virtual ClienteViewModel GetByDNI(string DNI)
        {
            IQueryable<ClienteViewModel> result = db.Clientes.Where(cliente=> cliente.DNI.Equals(DNI)).Select(cliente => new ClienteViewModel
            {
                DNI = cliente.DNI,
                Nombre = cliente.Nombre,
                PrimerApellido = cliente.PrimerApellido,
                SegundoApellido = cliente.SegundoApellido,
                PuntoTuristico = cliente.PuntoTuristico,
                Password = cliente.Password
            });
            return result.First();
        }
        public virtual IQueryable<ClienteViewModel> GetCLientesConParada()
        {
            IQueryable<ClienteViewModel> result = db.Clientes.Where(cliente => cliente.PuntoTuristico!=null && cliente.PuntoTuristico!=0).Select(cliente => new ClienteViewModel
            {
                DNI = cliente.DNI,
                Nombre = cliente.Nombre,
                PrimerApellido = cliente.PrimerApellido,
                SegundoApellido = cliente.SegundoApellido,
                PuntoTuristico = cliente.PuntoTuristico,
                Password = cliente.Password
            });
            return result;
        }
        public virtual bool Insert(ClienteViewModel clienteModel, ModelStateDictionary modelState)
        {
            IQueryable<ClienteViewModel> result = db.Clientes.Where(cliente => cliente.DNI.ToLower().Equals(clienteModel.DNI.ToLower())).Select(cliente => new ClienteViewModel
            {
                DNI = cliente.DNI,
                Nombre = cliente.Nombre,
                PrimerApellido = cliente.PrimerApellido,
                SegundoApellido = cliente.SegundoApellido,
                PuntoTuristico = cliente.PuntoTuristico,
                Password = cliente.Password
            });

            if (result.Count() == 0)
            {
                var entity = clienteModel.ToEntity();
                db.Clientes.Add(entity);
                if (db.SaveChanges() == 1)
                {
                    return true;
                }
            }
            else
            {
                modelState.AddModelError("DNI", "Ya existe un usuario con este DNI");
            }



            return false;
        }

        public virtual void Update(ClienteViewModel cliente, ModelStateDictionary modelState)
        {
            var entity = cliente.ToEntity();
            db.Clientes.Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
