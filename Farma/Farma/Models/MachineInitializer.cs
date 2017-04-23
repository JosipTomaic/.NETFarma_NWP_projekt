using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Farma.Models
{
    public class MachineInitializer: System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var machines = new List<Stroj>
            {
                new Stroj {Naziv="Traktor", Model="554", Marka="IMT", Svrha="Oranje", GodisnjeDoba="proljece"}
            };
            machines.ForEach(s => context.Strojevi.Add(s));
            context.SaveChanges();

        }
    }
}