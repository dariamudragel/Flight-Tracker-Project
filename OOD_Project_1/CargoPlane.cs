using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OOD_Project_1
{
    public class CargoPlane : IBaseItem, IReportable
    {
        public string? serial { get; set; }
        public string? country { get; set; }
        public Single maxload { get; set; }
        public string? model { get; set; }
        public string itemTypeName { get => "CP"; }
        public UInt64 itemId { get; set; }
        public char[] lettersID { get => ['N', 'C', 'P']; }
        public CargoPlane(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("Wrong Data! Incorrect parameter passed in CargoPlane.");
            }

            PopulateData(data);        
        }
        private void PopulateData(string itemData)
        {
            string[] words = itemData.Split(',');
            if (words.Length >= 6)
            {
                if (!string.IsNullOrEmpty(words[1]))
                {
                    UInt64 tmp;
                    if (UInt64.TryParse(words[1], out tmp))
                    {
                        itemId = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[2]))
                {
                    serial = words[2];
                }
                if (!string.IsNullOrEmpty(words[3]))
                {
                    country = words[3];
                }
                if (!string.IsNullOrEmpty(words[4]))
                {
                    model = words[4];
                }
                if (!string.IsNullOrEmpty(words[5]))
                {
                    Single tmp;
                    if (Single.TryParse(words[5], out tmp))
                    {
                        maxload = tmp;
                    }
                }
            }
        }
        public CargoPlane(byte[] itemData)
        {
            PopulateObject(itemData);
        }
        private void PopulateObject(byte[] _itemData)
        {
            int currOffset = 7;
            itemId = BitConverter.ToUInt64(_itemData, currOffset);

            currOffset += 8;
            char[] serialCharArray = new char[10];
            int j = 0;
            for (int i = currOffset; i < currOffset + 10; i++)
            {
                serialCharArray[j] = (char)_itemData[i];
                j++;
            }
            serial = new string(serialCharArray).TrimEnd('\0');

            currOffset += 10;
            char[] isoCharArray = new char[3];
            j = 0;
            for (int i = currOffset; i < currOffset + 3; i++)
            {
                isoCharArray[j] = (char)_itemData[i];
                j++;
            }
            country = new string(isoCharArray);

            currOffset += 3;
            UInt16 ml = BitConverter.ToUInt16(_itemData, currOffset);

            currOffset += 2;
            char[] modelCharArray = new char[ml];
            j = 0;
            for (int i = currOffset; i < currOffset + ml; i++)
            {
                modelCharArray[j] = (char)_itemData[i];
                j++;
            }
            model = new string(modelCharArray);

            currOffset += ml;
            maxload = BitConverter.ToSingle(_itemData, currOffset);
        }

        public string Accept(INewsOutlet ir)
        {
            return ir.visitingCargoPlane(this);
        }
    }
    public class ICargoPlaneFactory : IFactory<CargoPlane>
    {
        public CargoPlane CreateItem(string itemData)
        {
            return new CargoPlane(itemData);
        }

        public CargoPlane CreateItem(byte[] itemData)
        {
            return new CargoPlane(itemData);
        }
    }
}
