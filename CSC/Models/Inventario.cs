using CSC.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace CSC.Models
{
    public class Inventario
    {
        [Required]
        public int ClienteID { get; set; }
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public Software Software { get; set; }
        public Cliente Cliente { get; set; }
        public int Quantidade { get; set; }

    }
}
