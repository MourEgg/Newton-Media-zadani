using System;
using System.Collections.Generic;

namespace WebApplication1
{
    public class Driver
    {

        public int id { get; set; }
        public Person driver { get; set; }
        public List<Vehicle> vehicleInfo { get; set; }
        
        /*
        public Driver(int id, Person person, Vehicle vehicle)
        {
            this.id = id;
            this.person = person;
            this.vehicle = vehicle;
        }
*/
    }
}