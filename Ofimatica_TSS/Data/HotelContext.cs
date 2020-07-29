using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Ofimatica_TSS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ofimatica_TSS.Data
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options)
        {
            try
            {
                var databaseCreator = (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);
                databaseCreator.CreateTables();
            }
            catch (Exception e)
            {
                //A SqlException will be thrown if tables already exist. So simply ignore it.
            }
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<PuntoTuristico> PuntosTuristicos { get; set; }
        public DbSet<ParadaPuntoTuristico> ParadasPuntoTuristico { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<PuntoTuristico>().ToTable("PuntosTuristicos");


            modelBuilder
    .Entity<ParadaPuntoTuristico>(builder =>
    {
        //builder.HasNoKey();
        builder.ToTable("ParadasPuntoTuristico");
    });


        }
    }
}