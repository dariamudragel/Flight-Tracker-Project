using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapsui.Providers.Wfs.Utilities;

namespace OOD_Project_1
{
    public class NewsGenertor
    {
        List<INewsOutlet> providers = new List<INewsOutlet>();
        List<IReportable> reportable = new List<IReportable>();
        public NewsGenertor(List<INewsOutlet> _providers, List<IReportable> _reportable)
        {
            providers = _providers;
            reportable = _reportable;
            Television t1 = new Television("Abelian Television");
            Television t2 = new Television("Channel TV-Tensor");

            Radio r1 = new Radio("Quantifier radio");
            Radio r2 = new Radio("Shmem radio");

            Newspaper n1 = new Newspaper("Categories Journal");
            Newspaper n2 = new Newspaper("Polytechnical Gazette");
            providers.Add(t1);
            providers.Add(t2);
            providers.Add(r1);
            providers.Add(r2);
            providers.Add(n1);
            providers.Add(n2);
        }
      
        public IEnumerable<string> GenerateNextNews()
        
        {       
            foreach (var provider in providers)
            {
               foreach(var report in reportable)
               {
                   yield return report.Accept(provider);
               }
            }
        }

        public void PrintNews()
        {
            foreach (var news in GenerateNextNews())
            {
                Console.WriteLine(news);
            }
        }
    }
}
