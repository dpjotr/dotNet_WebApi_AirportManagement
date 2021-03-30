using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportModel
{
    public class Trip
    {
        public int TripId { get; set; }

        public String From {get; set;}
        public String To {get; set;}
        public DateTime Takeof {get; set;}
        public DateTime Arrival { get; set; }

        public int? AirplaneId { get; set; }
        public Airplane Airplane { get; set; }

        public List<Ticket> Passengers { get; set; }

        public override string ToString()
        {
            int passengers = Passengers == null ? 0 : Passengers.Count;
            return $@"id:{TripId}, {From} {Takeof} -- {To}{Arrival}";
        }


    }
}
