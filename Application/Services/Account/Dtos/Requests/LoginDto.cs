using System.ComponentModel.DataAnnotations;

namespace Application.Services.Account.Dtos.Requests
{
    public class LoginDto
    {
        [Required(ErrorMessage = "O campo e-mail é obrigatório"),
         EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório."),
         StringLength(8, MinimumLength = 4, ErrorMessage = "Senha deve ter no mínimo 4 e no máximo 8 caracteres.")]
        public string Password { get; set; }
    }
}
