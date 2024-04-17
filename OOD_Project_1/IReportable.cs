using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OOD_Project_1
{
    public interface IReportable
    {
        public string Accept(INewsOutlet ir);
    }
}
