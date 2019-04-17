using CSC.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSC.Models
{
    public class Inventario
    {
        [Required]
        public int ClienteID { get; set; }
        [Required] 
        public Software Software { get; set; }
        public Cliente Cliente { get; set; }
        public int Quantidade { get; set; }

    }
}
