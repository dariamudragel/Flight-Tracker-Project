using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_Project_1
{
    public interface IAirportBaseFactory
    {
        public IBaseItem CreateItem(string itemData);
        public IBaseItem CreateItem(byte[] messageBytes);
    }
}
