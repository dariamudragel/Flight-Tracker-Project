using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;

namespace OOD_Project_1
{

    public static class ParsingString
    {
        public static List<IBaseItem> AirportItems = new List<IBaseItem>();
        public static List<IBaseItem> reportableObjects = new List<IBaseItem>();
        public static List <Airport> listOfAirports = new List<Airport>();
        public static List<Crew> listOfCrew = new List<Crew>();
        public static List<Cargo> listOfCargo = new List<Cargo>();
        public static List<CargoPlane> listOfCargoPlane = new List<CargoPlane>();
        public static List<Passenger> listOfPassenger = new List<Passenger>();
        public static List<PassengerPlane> listOfPassengerPlane = new List<PassengerPlane>();
        public static List<Flight> listOfFlight = new List<Flight>();
        public static void PString(string[] args) 
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("Please provide file path! Press any key to exit.");
                Console.ReadLine();
                return;
            }

            string filePath = args[0];
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Please provide correct file path! File name is empty. Press any key to exit.");
                Console.ReadLine();
                return;
            }

            StreamReader sr = new StreamReader("example_data.ftr");
            string? line = sr.ReadLine();
            while (line != null)
            {
                string[] words = line.Split(',');
                if (words.Length > 0)
                {
                    if (words[0] == "AI")
                    {
                        IAirportFactory airportFactory = new IAirportFactory();
                        listOfAirports.Add(airportFactory.CreateItem(line));
                    }
                    if (words[0] == "C")
                    {
                        ICrewFactory airportFactory = new ICrewFactory();
                        listOfCrew.Add(airportFactory.CreateItem(line));
                    }
                    if (words[0] == "CP")
                    {
                        ICargoPlaneFactory airportFactory = new ICargoPlaneFactory();
                        listOfCargoPlane.Add(airportFactory.CreateItem(line));
                    }
                    if (words[0] == "CA")
                    {
                        ICargoFactory airportFactory = new ICargoFactory();
                        listOfCargo.Add(airportFactory.CreateItem(line));
                    }
                    if (words[0] == "P")
                    {
                        IPassengerFactory airportFactory = new IPassengerFactory();
                        listOfPassenger.Add(airportFactory.CreateItem(line));
                    }
                    if (words[0] == "PP")
                    {
                        IPassengerPlaneFactory airportFactory = new IPassengerPlaneFactory();
                        listOfPassengerPlane.Add(airportFactory.CreateItem(line));
                    }
                    if (words[0] == "FL")
                    {
                        IFlightFactory airportFactory = new IFlightFactory();
                        listOfFlight.Add(airportFactory.CreateItem(line));
                    }
                }
                else
                {
                    Console.WriteLine("Type of an object is not identified.");
                }
                line = sr.ReadLine();
            }

            sr.Close();

            AirportItems.AddRange(listOfAirports);
            AirportItems.AddRange(listOfCrew);
            AirportItems.AddRange(listOfCargo);
            AirportItems.AddRange(listOfPassenger);
            AirportItems.AddRange(listOfCargoPlane);
            AirportItems.AddRange(listOfPassengerPlane);
            AirportItems.AddRange(listOfFlight);

            reportableObjects.AddRange(listOfAirports);
            reportableObjects.AddRange(listOfCargoPlane);
            reportableObjects.AddRange(listOfPassengerPlane);

           
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var jsonAirport = JsonSerializer.Serialize(AirportItems, options);
            File.WriteAllText("jsonAirport.json", jsonAirport);

        }       
    }
}
