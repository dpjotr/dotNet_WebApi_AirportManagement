using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportModel
{
    public class Airplane
    {
        public int AirplaneId { get; set; }
      
        public String Manufacturer { get; set; }
        public String Model { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime LastInspection { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public List<Trip> Trips { get; set; }

        public override string ToString()
        {
            return $@"id{AirplaneId}, {Manufacturer}-{Model} last inspected on {LastInspection} ";
        }

    }
}
