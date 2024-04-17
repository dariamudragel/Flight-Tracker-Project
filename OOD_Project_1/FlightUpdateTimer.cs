using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Markup.Xaml.MarkupExtensions;
using FlightTrackerGUI;
using Mapsui.Utilities;
using ExCSS;
using Mapsui.Projections;
using DynamicData;

namespace OOD_Project_1
{ 
    internal class FlightUpdateTimer:IDisposable
    {
        private FlightsGUIData _flightsGUIData;

        private bool disposedValue;
        private System.Timers.Timer _timer;
        private List<FlightGUI> _flightsGUI = new List<FlightGUI>();

        public List<Airport> _airportsList;
        public List<Flight> _flights;
        public StreamReader sr = new StreamReader("example_data.ftr");

        private List<Flight> _newFlights;

        public FlightUpdateTimer(int timeOut)
        {
            _flights = new List<Flight>();
            _airportsList = new List<Airport>();
            _timer = new System.Timers.Timer(timeOut);
            _timer.Elapsed += OnTimedEvent;
            //_timer.Elapsed += OnTimedEventBinary;
            _timer.AutoReset = true;       
        }

        public void CreatingAirportsAndFlights()
        {
            string? line = sr.ReadLine();
            while (line != null)
            {
                string[] words = line.Split(',');
                if (words.Length > 0)
                {
                    if (words[0] == "AI")
                    {
                        IAirportFactory airportFactory = new IAirportFactory();
                        _airportsList.Add(airportFactory.CreateItem(line));
                    }

                    if (words[0] == "FL")
                    {
                        IFlightFactory flightFactory = new IFlightFactory();
                        _flights.Add(flightFactory.CreateItem(line));
                    }
                }
                line = sr.ReadLine();
            }
            sr.Close();
        }
        
        public void CreateAirportsAndFlightBinary(CancellationTokenSource cts)
        {
            _newFlights = new List<Flight>();
            CustomNetworkSourceSimulator netSourceSim = new CustomNetworkSourceSimulator(_airportsList, _newFlights, "example_data.ftr", 1, 10);
            Task t = netSourceSim.InitNetworkSimulator(cts);
        }
        double CalculateAngle(double xArr, double yArr, double xDep, double yDep)
        {
            double x = xArr - xDep;
            double y = yArr - yDep;
            double angleRadians = Math.Atan2(x, y);
            if (angleRadians < 0)
            {
                angleRadians += 2 * Math.PI;
            }
            return angleRadians;
        }

        public FlightGUI CalculateFlightPosition(Flight flight)
        {
            UInt64 departureAirportID = flight.originAsId;
            UInt64 arrivalAirportID = flight.targetAsId;
            DateTime takeOffDateTime = DateTime.Parse(flight.takeOffTime);
            DateTime landingDateTime = DateTime.Parse(flight.landingTime);
            if (takeOffDateTime > landingDateTime)
            {
                landingDateTime = landingDateTime.AddDays(1);
            }

            var elapsedTime = DateTime.Now - takeOffDateTime;
            var flightDuration = landingDateTime - takeOffDateTime;

            double fraction = elapsedTime.TotalSeconds / flightDuration.TotalSeconds;

            WorldPosition depPos = new WorldPosition();
            depPos.Longitude = GetAirportPositionLongitude(departureAirportID);
            depPos.Latitude = GetAirportPositionLatitude(departureAirportID);
            WorldPosition arrPos = new WorldPosition();
            arrPos.Latitude = GetAirportPositionLatitude(arrivalAirportID);
            arrPos.Longitude = GetAirportPositionLongitude(arrivalAirportID);
            WorldPosition interpolatedPosition = InterpolatePosition(depPos, arrPos, fraction);

            (double x, double y) mapCoordsDep = SphericalMercator.FromLonLat(depPos.Longitude, depPos.Latitude);
            (double x, double y) mapCoordsArr = SphericalMercator.FromLonLat(arrPos.Longitude, arrPos.Latitude);
            return new FlightGUI { ID = flight.itemId, WorldPosition = interpolatedPosition, MapCoordRotation =  CalculateAngle(mapCoordsArr.x, mapCoordsArr.y, mapCoordsDep.x, mapCoordsDep.y) };
        }
        public FlightGUI CalculateFlightPositionBinary(Flight flight)
        {
            UInt64 departureAirportID = flight.originAsId;
            UInt64 arrivalAirportID = flight.targetAsId;
            DateTime takeOffDateTime = DateTime.Parse(flight.takeOffTime);
            DateTime landingDateTime = DateTime.Parse(flight.landingTime);
            if (takeOffDateTime > landingDateTime)
            {
                landingDateTime = landingDateTime.AddDays(1);
            }

            var elapsedTime = DateTime.Now - takeOffDateTime;
            var flightDuration = landingDateTime - takeOffDateTime;

            double fraction = elapsedTime.TotalSeconds / flightDuration.TotalSeconds;

            WorldPosition depPos = new WorldPosition();
            depPos.Longitude = GetAirportPositionLongitude(departureAirportID);
            depPos.Latitude = GetAirportPositionLatitude(departureAirportID);
            WorldPosition arrPos = new WorldPosition();
            arrPos.Latitude = GetAirportPositionLatitude(arrivalAirportID);
            arrPos.Longitude = GetAirportPositionLongitude(arrivalAirportID);
            WorldPosition interpolatedPosition = InterpolatePosition(depPos, arrPos, fraction);           

            (double x, double y) mapCoordsDep = SphericalMercator.FromLonLat(depPos.Longitude, depPos.Latitude);
            (double x, double y) mapCoordsArr = SphericalMercator.FromLonLat(arrPos.Longitude, arrPos.Latitude);
            return new FlightGUI { ID = flight.itemId, WorldPosition = interpolatedPosition, MapCoordRotation = CalculateAngle(mapCoordsArr.x, mapCoordsArr.y, mapCoordsDep.x, mapCoordsDep.y) };
        }
        public double GetAirportPositionLongitude(UInt64 airportID)
        {
            Airport airport = null;
            foreach (Airport a in _airportsList)
            {
                if (airportID == a.itemId)
                {
                    airport = a;
                    break;
                }
            }
            return airport.longitude;
        }
        public double GetAirportPositionLatitude(UInt64 airportID)
        {
            Airport airport = null;
            foreach (Airport a in _airportsList)
            {
                if (airportID == a.itemId)
                {
                    airport = a;
                    break;
                }
            }
            return airport.latitude;
        }
        private WorldPosition InterpolatePosition(WorldPosition departure, WorldPosition arrival, double fraction)
        {
            double interpolatedLatitude = departure.Latitude + (arrival.Latitude - departure.Latitude) * fraction;
            double interpolatedLongitude = departure.Longitude + (arrival.Longitude - departure.Longitude) * fraction;

            return new WorldPosition { Latitude = interpolatedLatitude, Longitude = interpolatedLongitude };
        }

        public void LoadFlights()
        {
            CreatingAirportsAndFlights();
        }
        public void Stop()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
        public void Start()
        {
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            DateTime t = e.SignalTime;

            _flightsGUI.Clear();

            foreach (Flight fl in _flights)
            {
                if (!string.IsNullOrEmpty(fl.takeOffTime) && !string.IsNullOrEmpty(fl.landingTime))
                {
                    DateTime takeOffDateTime = DateTime.Parse(fl.takeOffTime);
                    DateTime landingDateTime = DateTime.Parse(fl.landingTime);
                    if (takeOffDateTime > landingDateTime)
                    {
                        landingDateTime = landingDateTime.AddDays(1);
                    }

                    if (DateTime.Now >= takeOffDateTime && DateTime.Now < landingDateTime)
                    {
                        _flightsGUI.Add(CalculateFlightPosition(fl));
                    }
                }
            }

            _flightsGUIData = new FlightsGUIData(_flightsGUI);
            Runner.UpdateGUI(_flightsGUIData);
        }

        private void OnTimedEventBinary(object? sender, ElapsedEventArgs e)
        {
            DateTime t = e.SignalTime;

            _flightsGUI.Clear();

            List<Flight> currFlights = new List<Flight>(_newFlights);
            foreach (Flight fl in currFlights)
            {
                if (!string.IsNullOrEmpty(fl.takeOffTime) && !string.IsNullOrEmpty(fl.landingTime))
                {
                    DateTime takeOffDateTime = DateTime.Parse(fl.takeOffTime);
                    DateTime landingDateTime = DateTime.Parse(fl.landingTime);
                    if (takeOffDateTime > landingDateTime)
                    {
                        landingDateTime = landingDateTime.AddDays(1);
                    }

                    if (DateTime.Now >= takeOffDateTime && DateTime.Now < landingDateTime)
                    {
                        _flightsGUI.Add(CalculateFlightPositionBinary(fl));
                    }                   
                }
            }

            _flightsGUIData = new FlightsGUIData(_flightsGUI);
            Runner.UpdateGUI(_flightsGUIData);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
