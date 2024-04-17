using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOD_Project_1
{
    public  class Crew:IBaseItem
    {
        public string? name { get; set; }
        public UInt64 age { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public UInt64 practice { get; set; }
        public string? role { get; set; }
        public string itemTypeName { get => "C"; }
        public UInt64 itemId { get; set; }
        public UInt32 followingMessageTotal { get; set; }
        public char[] lettersID { get => ['N', 'C', 'R']; }

        public Crew(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("Wrong Data! Incorrect parameter passed in Crew.");
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
                    UInt64 tmp;
                    if (UInt64.TryParse(words[3], out tmp))
                    {
                        age = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[4]))
                {
                    phone = words[4];
                }
                if (!string.IsNullOrEmpty(words[5]))
                {
                    email = words[5];
                }

                if (!string.IsNullOrEmpty(words[6]))
                {
                    UInt64 tmp;
                    if (UInt64.TryParse(words[6], out tmp))
                    {
                        practice = tmp;
                    }
                }
                if (!string.IsNullOrEmpty(words[7]))
                {
                    role = words[7];
                }
            }
        }
        public Crew(byte[] itemData)
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
            age = BitConverter.ToUInt16(_itemData, currOffset);

            currOffset += 2;
            char[] phoneCharArray = new char[12];
            j = 0;
            for (int i = currOffset; i < currOffset + 12; i++)
            {
                phoneCharArray[j] = (char)_itemData[i];
                j++;
            }
            phone = new string(phoneCharArray);

            currOffset += 12;
            UInt16 emailLength = BitConverter.ToUInt16(_itemData, currOffset);

            currOffset += 2;
            char[] emailCharArray = new char[emailLength];
            j = 0;
            for (int i = currOffset; i < currOffset + emailLength; i++)
            {
                emailCharArray[j] = (char)_itemData[i];
                j++;
            }
            email = new string(emailCharArray);

            currOffset += emailLength;
            practice = BitConverter.ToUInt16(_itemData, currOffset);

            currOffset += 2;
            char[] roleCharArray = new char[1];
            j = 0;
            for (int i = currOffset; i < currOffset + 1; i++)
            {
                roleCharArray[j] = (char)_itemData[i];
                j++;
            }
            role = new string(roleCharArray);
        }
    }
    public class ICrewFactory : IFactory<Crew>
    {
        public Crew CreateItem(string itemData)
        {
           return new Crew(itemData);
        }
        public Crew CreateItem(byte[] itemData)
        {
            return new Crew(itemData);
        }
    }
}
