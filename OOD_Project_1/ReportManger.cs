using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Splat;

namespace OOD_Project_1
{
    public class ReportManger
    {
        public string userCommand;
        List<INewsOutlet> providers = new List<INewsOutlet>();
        List<IReportable> reportable = new List<IReportable>();
        public void Reader()
        {
            userCommand = Console.ReadLine();
        }
        public void Print()
        {          
            if(userCommand =="report")
            {
                reportable.AddRange(ParsingString.listOfPassengerPlane);
                reportable.AddRange(ParsingString.listOfCargoPlane);
                reportable.AddRange(ParsingString.listOfAirports);
                NewsGenertor newsGenerator = new NewsGenertor(providers, reportable);
                newsGenerator.PrintNews();
            }
        }
    }
}
