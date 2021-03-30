using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportModel
{
    public class Company
    {
        public Company() => new List<Airplane>();
        public int CompanyId { get; set; }      
        public List<Airplane> Airplanes {get; set;} 

        public String Name { get; set; }
        public String Country { get; set; }
    }
}
