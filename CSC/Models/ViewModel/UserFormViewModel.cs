using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Models.ViewModel
{
    public class UserFormViewModel
    {
        public User User { get; set; }
        public ICollection<Funcionario> Funcionarios { get; set; }
    }
}
