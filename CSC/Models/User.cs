﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSC.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "O campo usuário não pode estar em branco!")]
        [Remote(action: "VerifyLogon", controller: "Home", AdditionalFields = nameof(Senha))]
        public string NomeLogon { get; set; }
        [Required(ErrorMessage = "O campo senha não pode estar em branco!")]
        public string Senha { get; set; }
        public Funcionario Funcionario { get; set; }
        public int FuncionarioId { get; set; }

        public User() { }


        public User(string name, string senha)
        {
            this.NomeLogon = name;
            this.Senha = senha;
        }
    }
}
