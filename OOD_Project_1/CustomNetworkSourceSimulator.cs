using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using DynamicData;
using NetworkSourceSimulator;

namespace OOD_Project_1
{
    public class CustomNetworkSourceSimulator : NetworkSourceSimulator.NetworkSourceSimulator
    {
        public string userCommand;
        public static List<INewsOutlet> providers = new List<INewsOutlet>();
        public static List<IReportable> reportable = new List<IReportable>();

        private readonly List<IBaseItem> _newAirportItems;
        public static List<IBaseItem> AirportItems = new List<IBaseItem>();

        public List<Airport> airports;
        public List<Flight> flights;

        public static List<Airport> listOfAirports = new List<Airport>();
        public static List<Crew> listOfCrew = new List<Crew>();
        public static List<Cargo> listOfCargo = new List<Cargo>();
        public static List<CargoPlane> listOfCargoPlane = new List<CargoPlane>();
        public static List<Passenger> listOfPassenger = new List<Passenger>();
        public static List<PassengerPlane> listOfPassengerPlane = new List<PassengerPlane>();
        public static List<Flight> listOfFlight = new List<Flight>();

        public List<Airport> _newAirports;
        public List<Flight> _newFlights;
        public CustomNetworkSourceSimulator(List<IBaseItem> newAirportItem, string ftrFilePath, int minOffsetInMs, int maxOffsetInMs) : base(ftrFilePath, minOffsetInMs, maxOffsetInMs)
        {
            _newAirportItems = newAirportItem;
            _newAirports = new List<Airport>();
            _newFlights = new List<Flight>();
        }

        public CustomNetworkSourceSimulator(List<Airport> newAirports, List<Flight> newFlights, string ftrFilePath, int minOffsetInMs, int maxOffsetInMs) : base(ftrFilePath, minOffsetInMs, maxOffsetInMs)
        {
            airports = newAirports;
            flights = newFlights;
        }
        public void HandleNewDataReadyEvent(object sender, NewDataReadyArgs args)
        {
            int index = args.MessageIndex;
            var currMessage = this.GetMessageAt(index);
            char char1 = (char)(currMessage.MessageBytes[0]);
            char char2 = (char)(currMessage.MessageBytes[1]);
            char char3 = (char)(currMessage.MessageBytes[2]);
            char[] lettersID = new char[] { char1, char2, char3 };

            if (new string(lettersID) == "NAI")
            {
                IAirportFactory airportFactory = new IAirportFactory();
                listOfAirports.Add(airportFactory.CreateItem(currMessage.MessageBytes));
            }
            if (new string(lettersID) == "NCR")
            {
                ICrewFactory airportFactory = new ICrewFactory();
                listOfCrew.Add(airportFactory.CreateItem(currMessage.MessageBytes));
            }
            if (new string(lettersID) == "NCP")
            {
                ICargoPlaneFactory airportFactory = new ICargoPlaneFactory();
                listOfCargoPlane.Add(airportFactory.CreateItem(currMessage.MessageBytes));
            }
            if (new string(lettersID) == "NCA")
            {
                ICargoFactory airportFactory = new ICargoFactory();
                listOfCargo.Add(airportFactory.CreateItem(currMessage.MessageBytes));
            }
            if (new string(lettersID) == "NPA")
            {
                IPassengerFactory airportFactory = new IPassengerFactory();
                listOfPassenger.Add(airportFactory.CreateItem(currMessage.MessageBytes));
            }
            if (new string(lettersID) == "NPP")
            {
                IPassengerPlaneFactory airportFactory = new IPassengerPlaneFactory();
                listOfPassengerPlane.Add(airportFactory.CreateItem(currMessage.MessageBytes));
            }
            if (new string(lettersID) == "NFL")
            {
                IFlightFactory airportFactory = new IFlightFactory();
                listOfFlight.Add(airportFactory.CreateItem(currMessage.MessageBytes));
            }

            AirportItems.AddRange(listOfAirports);
            AirportItems.AddRange(listOfCrew);
            AirportItems.AddRange(listOfCargo);
            AirportItems.AddRange(listOfPassenger);
            AirportItems.AddRange(listOfCargoPlane);
            AirportItems.AddRange(listOfPassengerPlane);
            AirportItems.AddRange(listOfFlight);
        }

        public void HandleFlightsAndAirports(object sender, NewDataReadyArgs args)
        {
            int index = args.MessageIndex;
            var currMessage = this.GetMessageAt(index);
            char char1 = (char)(currMessage.MessageBytes[0]);
            char char2 = (char)(currMessage.MessageBytes[1]);
            char char3 = (char)(currMessage.MessageBytes[2]);
            char[] lettersID = new char[] { char1, char2, char3 };

            if (new string(lettersID) == "NAI")
            {
                IAirportFactory airportFactory = new IAirportFactory();
                airports.Add(airportFactory.CreateItem(currMessage.MessageBytes));
            }
            if (new string(lettersID) == "NFL")
            {
                IFlightFactory airportFactory = new IFlightFactory();
                flights.Add(airportFactory.CreateItem(currMessage.MessageBytes));
            }
        }

        public Task InitNetworkSimulator( CancellationTokenSource cts)
        {
            this.OnNewDataReady += this.HandleFlightsAndAirports;

            return Task.Run(() => this.Run(), cts.Token);
        }

        public static void CommandsHandler(string[] args)
        {
            string filePath = args[0];

            CustomNetworkSourceSimulator netSourceSim = new CustomNetworkSourceSimulator(AirportItems, filePath, 1, 10);

            netSourceSim.OnNewDataReady += netSourceSim.HandleNewDataReadyEvent;

            var cancellationTokenSource = new CancellationTokenSource();
            var t = Task.Run(() => netSourceSim.Run(), cancellationTokenSource.Token);

            var userCommand = Console.ReadLine();
            while (userCommand != null)
            {
                if (userCommand == "print")
                {
                    Console.WriteLine($"User entered : {userCommand}");

                    var newOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };

                    var newJsonAirport = JsonSerializer.Serialize(AirportItems, newOptions);
                    File.WriteAllText($"snapshot_{DateTime.Now.ToString("h_mm_ss tt")}.json", newJsonAirport);
                }
                if (userCommand == "exit")
                {
                    Console.WriteLine($"User entered : {userCommand}");

                    netSourceSim.OnNewDataReady -= netSourceSim.HandleNewDataReadyEvent;
                    cancellationTokenSource.Cancel();
                    break;
                }
                userCommand = Console.ReadLine();
            }
        }
    }
}
