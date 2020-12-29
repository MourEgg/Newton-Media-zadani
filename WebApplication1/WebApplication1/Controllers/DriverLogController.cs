using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]


    public class DriverLogController : ControllerBase
    {


        private Brands[] brands;

        private readonly ILogger<DriverLogController> _logger;

        public DriverLogController(ILogger<DriverLogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "hello";
        }

        [HttpPost]

        public IEnumerable<object> Post([FromBody] Driver[] drivers)
        {
            string jsonString = System.IO.File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "brands.json"));
            brands = JsonSerializer.Deserialize<Brands[]>(jsonString);

            List<object> ret = new List<object>();
            ret.Add(avgAge(drivers));
            ret.Add(birthYears(drivers));
            ret.Add(sameLastname(drivers));
            ret.Add(topThreeBrands(drivers));
            ret.Add(avgBrandAge(drivers));
            ret.Add(engineTypes(drivers));
            ret.Add(blueEyed(drivers));
            ret.Add(sameEngine(drivers));
            

            return ret;

            //return retJson;
            //TODO join car brands with ids
            //return part jsons
        }


        //returns average age of all drivers
        public object avgAge(Driver[] drivers)
        {
            int avg = (int)drivers.Average(s => age(s.driver.birthDate));

            var x = new
            {
                avgAge = avg
            };

            return x;
        }

        //returns birth years of drivers and and number of drivers born in coresponding years
        public object birthYears(Driver[] drivers)
        {
            var grouped = from d in drivers
                          group d by new { d.driver.birthDate.Year } into g
                          select new { g.Key.Year, count = g.Count() };

            var x = new
            {
                birthYears = grouped.ToList()
            };
            return x;

           
        }

        //returns matching lastnames of drivers and their ids
        public object sameLastname(Driver[] drivers)
        {
            var grouped = from d in drivers
                          group d.id by d.driver.lastName into g
                          where g.Count() >= 2
                          select new { name = g.Key, id = g.ToList() };

            var x = new
            {
                sameLastNameIds = grouped.ToList()
            };

            return x;
           
            
        }

        //returns three most common brands
        public object topThreeBrands(Driver[] drivers)
        {
            var grouped = from d in drivers
                          from c in d.vehicleInfo
                          group c by c.brandId into g
                          orderby g.Count() descending
                          select new { Id = g.Key } into f
                          join b in brands
                          on new { f.Id }
                          equals new { b.Id } into gj
                          from v in gj.DefaultIfEmpty()
                          select new { brandName = v?.BrandName ?? "Unknown brand id" };

            

            grouped = grouped.Take(3);

            var x = new
            {
                topThreeBrands = grouped.ToList()
            };
            return x;

            
        }

        //returns brand name and its average age of vehicles
        public object avgBrandAge(Driver[] drivers)
        {
            var grouped = from d in drivers
                          from c in d.vehicleInfo
                          group c by c.brandId into g
                          select new { Id = g.Key , avgYear = (int)(from a in g.ToList()
                                              select a.modelYear).Average()};
            var f = from g in grouped
                    join b in brands
                    on new { g.Id }
                    equals new { b.Id } into gj
                    from v in gj.DefaultIfEmpty()
                    select new { BrandName = v?.BrandName ?? "unknown brand id", g.avgYear };


            

            var x = new
            {
                avgBrandAge = f.ToList()
            };
            return x;


        }

        //returns all engine types and the percentage of drivers owning a car with this engine type
        public object engineTypes(Driver[] drivers)
        {
            var sum = from d in drivers
                      group d by d.vehicleInfo into g
                      select new { g = g.Key.Count() };

            var q = sum.Sum(g => g.g);

            var grouped = from d in drivers
                          from c in d.vehicleInfo
                          group c by c.engineType into g
                          select new { Engine = g.Key, percent = (double)g.Count()*100/q };

            var x = new
            {
                engineTypesPercents = grouped.ToList()
            };
            return x;

        }


        //returns IDs of blue eyed drivers that own hybrid or electric car
        public object blueEyed(Driver[] drivers)
        {
            var grouped = from d in drivers
                          from c in d.vehicleInfo
                          where c.engineType == ("Electric") ||
                          c.engineType == ("Hybrid")
                          group d by d.id into g
                          select new { id = g.Key };


            var x = new
            {
                blueEyedWithHybrid = grouped.ToList()
            };
            return x;
        }

        //returns IDs of drivers who own multiple cars with one type of engine
        public object sameEngine(Driver[] drivers)
        {

            var grouped = from d in drivers
                          where d.vehicleInfo.Count() >= 2
                          select new
                          {
                              d.id,
                              a = from a in d.vehicleInfo
                                  group a by a.engineType into f
                                  where f.Count() >= 2
                                  select f.Key
                          } into f
                          where f.a.Count() != 0
                          select new { id = f.id };

            var x = new
            {
                multipleCarsSameEngineIds = grouped.ToList()
            };
            return x;
            

        }
        

        public int age(DateTime date)
        {
            int age;
            var today = DateTime.Today;
            age = today.Year - date.Year;

            if (age > 0)
            {
                age -= Convert.ToInt32(today.Date < date.Date.AddYears(age));
            }
            else
            {
                age = 0;
            }

            return age;
        }
    }
}
