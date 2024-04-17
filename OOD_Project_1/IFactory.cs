using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_Project_1
{
    public interface IFactory<T>
    {
        T CreateItem(string itemData);
        T CreateItem(byte[] itemData);
    }
}
