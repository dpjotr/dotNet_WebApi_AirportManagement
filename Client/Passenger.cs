using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportModel
{
    public class Passenger
    {
       
        public int PassengerId { get; set; }
        public List<Ticket> Trips { get; set; }

        public String Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
