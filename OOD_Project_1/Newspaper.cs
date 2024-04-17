using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_Project_1
{
    public class Newspaper:INewsOutlet
    {
        public Newspaper newspaper { get; set; }
        public string name;

        public Newspaper(string _name)
        {
            name = _name;
        }

        public string visitingAirport(Airport airport)
        {
            return $"{name} - A report from the {airport.name} airport, {airport.country}";
        }

        public string visitingCargoPlane(CargoPlane cargoPlane)
        {
            return $"{name} - An interview with the crew of {cargoPlane.serial}.";
        }

        public string visitingPassengerPlane(PassengerPlane passengerPlane)
        {
            return $"{name} - Breaking news! {passengerPlane.model} aircraft loses EASA fails certification after inspection of {passengerPlane.serial}.";
        }
    }
}
