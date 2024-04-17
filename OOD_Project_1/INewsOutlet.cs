using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OOD_Project_1
{
    public interface INewsOutlet
    {
        public string visitingAirport(Airport airport);
        public string visitingCargoPlane(CargoPlane cargoPlane);
        public string visitingPassengerPlane(PassengerPlane passengerPlane);

    }
}
