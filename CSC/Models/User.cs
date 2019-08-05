using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSC.Models
{
    public class User : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Nome { get; set; }
        public DateTime Admissao { get; set; }
        public DateTime? Demissao { get; set; }
    }
}

