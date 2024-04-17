using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_Project_1
{
    public class Radio:INewsOutlet
    {
        public Radio radio { get; set; }
        public string name;

        public Radio(string _name)
        {
            name = _name;
        }

        public string visitingAirport(Airport airport)
        {
            return $"Reporting for {name}, Ladies and Gentlemen, we are at the {airport.name} airport.";
        }

        public string visitingCargoPlane(CargoPlane cargoPlane)
        {
            return $"Reporting for {name}, Ladies and Gentlemen, we are seeing the  {cargoPlane.serial} aircraft fly above us.";
        }

        public string visitingPassengerPlane(PassengerPlane passengerPlane)
        {
            return $"Reporting for {name}, Ladies and Gentlemen, we’ve just witnessed {passengerPlane.serial} take off.";
        }
    }
}
