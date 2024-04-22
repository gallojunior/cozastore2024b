using System.ComponentModel.DataAnnotations;

namespace CozaStore.ViewModels;

public class LoginVM
{
    [Display(Name = "Email ou Nome de Usuário")]
    [Required(ErrorMessage = "Por favor, informe seu Email ou Nome de Usuário")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Senha de Acesso")]
    [Required(ErrorMessage = "Por favor, informe sua Senha")]
    public string Senha { get; set; }

    [Display(Name = "Manter Conectado?")]
    public bool Lembrar { get; set; } = false;

    public string UrlRetorno { get; set; }
}