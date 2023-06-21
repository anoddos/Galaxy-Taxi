using GalaxyTaxi.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxiTests
{
    public static class Helper
    {

        public static Db CreateDBContextInstance()
        {

            var connectionstring = "Server=127.0.0.1;Port=5432;Database=GalaxyTaxiDb;User Id=postgres;Password=password;";

            var optionsBuilder = new DbContextOptionsBuilder<Db>();
            optionsBuilder.UseNpgsql(connectionstring);

            return new Db(optionsBuilder.Options); ;

        }
    }
}
