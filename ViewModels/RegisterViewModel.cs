﻿using System.ComponentModel.DataAnnotations;

namespace ApiBlogAspNet.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="O  nome é obrigatório ")]
        public string Name { get; set; }

        [Required(ErrorMessage ="O E-mail é obrigatório")]
        [EmailAddress(ErrorMessage =" o E-mail é invalido")]
        public string Email { get; set; }
    }
}
