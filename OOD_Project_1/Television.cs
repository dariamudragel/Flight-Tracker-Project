using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_Project_1
{
    public class Television:INewsOutlet
    {
        //public Television television { get; set; }
        public string  name;
        public Television(string _name)
        {
            name = _name;
        }

        public string visitingAirport(Airport airport)
        {
           
            return $"An image of {airport.name} airport";
        }

        public string visitingCargoPlane(CargoPlane cargoPlane)
        {
            return  $"An image of {cargoPlane.serial} cargo plane";
        }

        public string visitingPassengerPlane(PassengerPlane passengerPlane)
        {
            return $"An image of {passengerPlane.serial} passenger plane";
        }
    }
}
