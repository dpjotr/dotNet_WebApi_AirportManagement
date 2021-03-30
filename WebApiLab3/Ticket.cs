using System;

namespace AirportModel
{
    public class Ticket
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        
        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; }

    }
}
