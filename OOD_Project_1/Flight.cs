using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Avalonia.Controls.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OOD_Project_1
{
    public class Flight :IBaseItem
    {
        public UInt64 originAsId { get; set; }
        public UInt64 targetAsId { get; set; }
        public string? takeOffTime { get; set; }
        public string? landingTime { get; set; }
        public Single longitude { get; set; }
        public Single latitude { get; set; }
        public Single amsl { get; set; }
        public UInt64 planeId { get; set; }
        public UInt64[]? crewAsIds { get; set; }
        public UInt64[]? loadAsIds { get; set; }
        public string itemTypeName { get => "FL"; }
        public UInt64 itemId { get; set; }
        public char[] lettersID { get => ['N', 'F', 'L']; }

        public Flight(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("Wrong Data! Incorrect parameter passed in Flight.");
            }

            PopulateData(data);
        }
        private void PopulateData(string itemData)
        {
            string[] words = itemData.Split(',');
            if (words.Length >= 12)
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
                    UInt64 tmp;
                    if (UInt64.TryParse(words[2], out tmp))
                    {
                        originAsId = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[3]))
                {
                    UInt64 tmp;
                    if (UInt64.TryParse(words[3], out tmp))
                    {
                        targetAsId = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[4]))
                {
                    takeOffTime = words[4];
                }
                if (!string.IsNullOrEmpty(words[5]))
                {
                    landingTime = words[5];
                }
                if (!string.IsNullOrEmpty(words[6]))
                {
                    Single tmp;
                    if (Single.TryParse(words[6], out tmp))
                    {
                        longitude = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[7]))
                {
                    Single tmp;
                    if (Single.TryParse(words[7], out tmp))
                    {
                        latitude = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[8]))
                {
                    Single tmp;
                    if (Single.TryParse(words[8], out tmp))
                    {
                        amsl = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[9]))
                {
                    UInt64 tmp;
                    if (UInt64.TryParse(words[9], out tmp))
                    {
                        planeId = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[10]))
                {

                    string[] ids = words[10].Trim('[', ']').Split(';');
                    crewAsIds = new UInt64[ids.Length];
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (UInt64.TryParse(ids[i], out UInt64 tmp))
                        {
                            crewAsIds[i] = tmp;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(words[11]))
                {
                    string[] ids = words[11].Trim('[', ']').Split(';');
                    loadAsIds = new UInt64[ids.Length];
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (UInt64.TryParse(ids[i], out UInt64 tmp))
                        {
                            loadAsIds[i] = tmp;
                        }
                    }
                }
            }
        }
        public Flight(byte[] itemData)
        {
            PopulateObject(itemData);
        }
        private void PopulateObject(byte[] _itemData)
        {
            int CurOffset = 7;
            itemId = BitConverter.ToUInt64(_itemData, CurOffset);

            CurOffset = 15;
            originAsId = BitConverter.ToUInt64(_itemData, CurOffset);

            CurOffset = 23;
            targetAsId = BitConverter.ToUInt64(_itemData, CurOffset);

            CurOffset = 31;

            long NtakeOffTime = BitConverter.ToInt64(_itemData, CurOffset);
            DateTimeOffset takeOffTimeDTOffset = DateTimeOffset.FromUnixTimeMilliseconds(NtakeOffTime);
            DateTime takeoffTime = takeOffTimeDTOffset.DateTime;
            takeOffTime = takeoffTime.ToString("H:mm");

            CurOffset = 39;

            long NlandingTime = BitConverter.ToInt64(_itemData, CurOffset);
            DateTimeOffset landingTimeDTOffset = DateTimeOffset.FromUnixTimeMilliseconds(NlandingTime);
            DateTime lanTime = landingTimeDTOffset.DateTime;
            landingTime = lanTime.ToString("H:mm");

            CurOffset = 47;
            planeId = BitConverter.ToUInt64(_itemData, CurOffset);

            CurOffset = 55;
            UInt16 crewCount = BitConverter.ToUInt16(_itemData, CurOffset);

            CurOffset = 57;
            crewAsIds = new UInt64[crewCount];
            int j = 0;
            for (int i = CurOffset; i < CurOffset + (crewCount * 8); i += 8)
            {
                if (i < _itemData.Length)
                {
                    crewAsIds[j] = BitConverter.ToUInt64(_itemData, i);
                }
                else
                {
                    crewAsIds[j] = 0;
                }
                j++;
            }

            CurOffset += (crewCount * 8);
            UInt16 passengersCount = BitConverter.ToUInt16(_itemData, CurOffset);

            CurOffset += 2;
            loadAsIds = new UInt64[passengersCount];
            j = 0;
            for (int i = CurOffset; i < CurOffset + (passengersCount * 8); i += 8)
            {
                if (i < _itemData.Length)
                {
                    loadAsIds[j] = BitConverter.ToUInt64(_itemData, i);
                }
                else
                {
                    loadAsIds[j] = 0;
                }
                j++;
            }
        }
    }
    public class IFlightFactory : /*IAirportBaseFactory*/ IFactory<Flight>
    {
        public List<Flight> flightsFTR = new List<Flight>();
        public List<Flight> flightsBinary = new List<Flight>();
        public Flight CreateItem(string itemData)
        {
            Flight f = new Flight(itemData);
            flightsFTR.Add(f);
            return f;
        }
        public Flight CreateItem(byte[] itemData)
        {
            Flight f = new Flight(itemData);
            flightsBinary.Add(f);
            return f;
        }
    }
}
