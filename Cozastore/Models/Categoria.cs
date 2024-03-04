using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cozastore.Models;

[Table("Categoria")]
public class Categoria
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Por favor, informe o Nome")]
    [StringLength(30, ErrorMessage = "O Nome deve possuir no máximo 30 caracteres")]
    public string Nome { get; set; }

    [StringLength(300)]
    public string Foto { get; set; }

    [Display(Name = "Exibir como Filtro?")]
    public bool Filtrar { get; set; } = false;

    [Display(Name = "Exibir como Banner?")]
    public bool Banner { get; set; } = false;

    [Display(Name = "Categoria Pai")]
    public int? CategoriaPaiId { get; set; }
    [ForeignKey("CategoriaPaiId")]
    public Categoria CategoriaPai { get; set; }

    public ICollection<Produto> Produtos { get; set; }
}