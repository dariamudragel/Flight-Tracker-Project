using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OOD_Project_1
{
    public class PassengerPlane :IBaseItem, IReportable
    {
        public string? serial { get; set; }
        public string? country { get; set; }
        public string? model { get; set; }
        public UInt64 firstClassSize { get; set; }
        public UInt64 businessClassSize { get; set; }
        public UInt64 economyClassSize { get; set; }
        public string itemTypeName { get => "PP"; }
        public UInt64 itemId { get; set; }
        public char[] lettersID { get => ['N', 'P', 'P']; }
        public PassengerPlane(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("Wrong Data! Incorrect parameter passed in PassengerPlane.");
            }

            PopulateData(data);
        }
        public PassengerPlane(byte[] itemData)
        {
            PopulateObject(itemData);
        }

        private void PopulateObject(byte[] _itemData)
        {
            int currOffset = 7;
            itemId = BitConverter.ToUInt64(_itemData, currOffset);

            currOffset = 15;
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
            firstClassSize = BitConverter.ToUInt16(_itemData, currOffset);

            currOffset += 2;
            businessClassSize = BitConverter.ToUInt16(_itemData, currOffset);

            currOffset += 2;
            economyClassSize = BitConverter.ToUInt16(_itemData, currOffset);
        }
        private void PopulateData(string itemData)
        {
            string[] words = itemData.Split(',');
            if (words.Length >= 8)
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
                    UInt64 tmp;
                    if (UInt64.TryParse(words[5], out tmp))
                    {
                        firstClassSize = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[6]))
                {
                    UInt64 tmp;
                    if (UInt64.TryParse(words[6], out tmp))
                    {
                        businessClassSize = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[7]))
                {
                    UInt64 tmp;
                    if (UInt64.TryParse(words[7], out tmp))
                    {
                        economyClassSize = tmp;
                    }
                }
            }
        }

        public string Accept(INewsOutlet ir)
        {
            return ir.visitingPassengerPlane(this); 
        }
    }
    public class IPassengerPlaneFactory : IFactory<PassengerPlane>
    {
        public PassengerPlane CreateItem(string itemData)
        {
            return new PassengerPlane(itemData);
        }

        public PassengerPlane CreateItem(byte[] itemData)
        {
            return new PassengerPlane(itemData);
        }
    }
}
