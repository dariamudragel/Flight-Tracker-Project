using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OOD_Project_1
{
    [JsonDerivedType(typeof(Passenger), typeDiscriminator: "Passenger")]
    [JsonDerivedType(typeof(Airport), typeDiscriminator: "Airport")]
    [JsonDerivedType(typeof(PassengerPlane), typeDiscriminator: "PassengerPlane")]
    [JsonDerivedType(typeof(Crew), typeDiscriminator: "Crew")]
    [JsonDerivedType(typeof(Cargo), typeDiscriminator: "Cargo")]
    [JsonDerivedType(typeof(CargoPlane), typeDiscriminator: "CargoPlane")]
    [JsonDerivedType(typeof(Flight), typeDiscriminator: "Flight")]
    public interface IBaseItem
    {
        public string itemTypeName { get; }
        public UInt64 itemId { get; set; }
        public char[] lettersID { get; }
    }
}
