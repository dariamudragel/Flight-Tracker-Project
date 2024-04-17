#define stage1
//#define stage2
#define stage3
#define stage4
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Timers;
using FlightTrackerGUI;
using NetworkSourceSimulator;

namespace OOD_Project_1
{  
    class MainClass
    {
        static void Main(string[] args)
        {
#if stage1
            ParsingString.PString(args);
#endif
#if stage2
            CustomNetworkSourceSimulator.CommandsHandler(args);
#endif
#if stage3
            FlightUpdateTimer flightUpdater = new FlightUpdateTimer(1000);
            CancellationTokenSource cts = new CancellationTokenSource();

            flightUpdater.CreateAirportsAndFlightBinary(cts);
            flightUpdater.LoadFlights();
            flightUpdater.Start();
            Thread thread = new Thread(Runner.Run);
            thread.Start();
            //flightUpdater.Stop();
            //cts.Cancel();
#endif
#if stage4
            ReportManger reportManger = new ReportManger();

            reportManger.Reader();
            reportManger.Print();
#endif 
        }
    }
}
