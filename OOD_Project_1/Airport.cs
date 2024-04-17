using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using NetTopologySuite.Index.HPRtree;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OOD_Project_1
{
    public class Airport :IBaseItem, IReportable
    {
        public char[] lettersID { get => ['N', 'A', 'I']; }
        public string? name { get; set; }
        public string? code { get; set; }
        public Single longitude { get; set; }
        public Single latitude { get; set; }
        public Single amsl { get; set; }
        public string? country { get; set; }
        public string itemTypeName { get => "AI"; }
        public UInt64 itemId { get; set; }
        public Airport(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("Wrong Data! Incorrect parameter passed in Airport.");
            }
            PopulateData(data);              
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
                    name = words[2];
                }
                if (!string.IsNullOrEmpty(words[3]))
                {
                    code = words[3];
                }
                if (!string.IsNullOrEmpty(words[4]))
                {
                    Single tmp;
                    if (Single.TryParse(words[4], out tmp))
                    {
                        longitude = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[5]))
                {
                    Single tmp;
                    if (Single.TryParse(words[5], out tmp))
                    {
                        latitude = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[6]))
                {
                    Single tmp;
                    if (Single.TryParse(words[6], out tmp))
                    {
                        amsl = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[7]))
                {
                    country = words[7];
                }
               
            }
        }
        public Airport(byte[] itemData)
        {
            PopulateObject(itemData);
        }
        private void PopulateObject(byte[] _itemData)
        {
            int currOffset = 7;
            itemId = BitConverter.ToUInt64(_itemData, currOffset);

            currOffset = 15;
            UInt16 nameLength = BitConverter.ToUInt16(_itemData, currOffset);

            currOffset = 17;
            char[] nameCharArray = new char[nameLength];
            int j = 0;
            for (int i = currOffset; i < currOffset + nameLength; i++)
            {
                nameCharArray[j] = (char)_itemData[i];
                j++;
            }
            name = new string(nameCharArray);

            currOffset += nameLength;
            char[] codeCharArray = new char[3];
            j = 0;
            for (int i = currOffset; i < currOffset + 3; i++)
            {
                codeCharArray[j] = (char)_itemData[i];
                j++;
            }
            code = new string(codeCharArray);

            currOffset += 3;
            longitude = BitConverter.ToSingle(_itemData, currOffset);

            currOffset += 4;
            latitude = BitConverter.ToSingle(_itemData, currOffset);

            currOffset += 4;
            amsl = BitConverter.ToSingle(_itemData, currOffset);

            currOffset += 4;
            char[] isoCharArray = new char[3];
            j = 0;
            for (int i = currOffset; i < currOffset + 3; i++)
            {
                isoCharArray[j] = (char)_itemData[i];
                j++;
            }
            country = new string(isoCharArray);
        }

        public string Accept(INewsOutlet ir)
        {
            return ir.visitingAirport(this);
        }
    }
    
    public class IAirportFactory : /*IAirportBaseFactory*/ IFactory<Airport>
    { 
   
        public List<Airport> airportsFTR = new List<Airport>();
        public List<Airport> airportsBinary = new List<Airport>();
        public Airport CreateItem(string itemData)
        {
            Airport a = new Airport(itemData);
            airportsFTR.Add(a);
            return a;

        }

        public Airport CreateItem(byte[] itemData)
        {
            Airport a = new Airport(itemData);
            airportsBinary.Add(a);
            return a;
        }
    }
}

