using System.Text.Json.Serialization;

namespace StudentCrudAppWithEFCoreCodeFirst.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int StudentId { get; set; }
       [JsonIgnore]
        public Student?  Student{ get; set; }//navigation property
    }
}
