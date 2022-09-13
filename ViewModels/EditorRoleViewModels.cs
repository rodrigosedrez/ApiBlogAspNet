using System.ComponentModel.DataAnnotations;

namespace ApiBlogAspNet.ViewModels
{
    public class EditorRoleViewModels
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 3 e 40 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O slug e obrigatorio")]
        public string Slug { get; set; }
    }
}
