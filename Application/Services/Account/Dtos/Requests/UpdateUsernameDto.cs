using System.ComponentModel.DataAnnotations;

namespace Application.Services.Account.Dtos.Requests
{
    public class UpdateUsernameDto
    {
        [Required(ErrorMessage = "Campo Id é obrigatório.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo nome de usuário é obrigatório.")]
        public string Username { get; set; }
    }
}
