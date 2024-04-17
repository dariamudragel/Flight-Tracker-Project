using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OOD_Project_1
{
    public class Cargo : IBaseItem
    {
        public Single weight { get; set; }
        public string? description { get; set; }
        public string? code { get; set; }
        public string itemTypeName { get => "CA"; }
        public UInt64 itemId { get; set; }
        public char[] lettersID { get => ['N', 'C', 'A']; }
        public Cargo(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("Wrong Data! Incorrect parameter passed in Cargo.");
            }

            PopulateData(data); 
        }
        private void PopulateData(string itemData)
        {
            string[] words = itemData.Split(',');
            if (words.Length >= 5)
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
                    Single tmp;
                    if (Single.TryParse(words[2], out tmp))
                    {
                        weight = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[3]))
                {
                    code = words[3];
                }
                if (!string.IsNullOrEmpty(words[4]))
                {
                    description = words[4];
                }
            }
        }
        public Cargo(byte[] itemData)
        {
            PopulateObject(itemData);
        }
        private void PopulateObject(byte[] _itemData)
        {
            int currOffset = 7;
            itemId = BitConverter.ToUInt64(_itemData, currOffset);

            currOffset = 15;
            weight = BitConverter.ToSingle(_itemData, currOffset);

            currOffset = 19;
            char[] codeCharArray = new char[6];
            int j = 0;
            for (int i = currOffset; i < currOffset + 6; i++)
            {
                codeCharArray[j] = (char)_itemData[i];
                j++;
            }
            code = new string(codeCharArray);

            currOffset += 6;
            UInt16 descriptionLength = BitConverter.ToUInt16(_itemData, currOffset);

            currOffset += 2;
            char[] descriptionCharArray = new char[descriptionLength];
            j = 0;
            for (int i = currOffset; i < currOffset + descriptionLength; i++)
            {
                descriptionCharArray[j] = (char)_itemData[i];
                j++;
            }
            description = new string(descriptionCharArray);
        }
    }
    public class ICargoFactory : IFactory<Cargo>
    {
        public Cargo CreateItem(string itemData)
        {
            return new Cargo(itemData);
        }
        public Cargo CreateItem(byte[] itemData)
        {
            return new Cargo(itemData);
        }
    }
}
