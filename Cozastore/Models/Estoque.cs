using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cozastore.Models;

[Table("Estoque")]
public class Estoque
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Display(Name = "Produto")]
    [Required(ErrorMessage = "Por favor, informe o Produto")]
    public int ProdutoId { get; set; }
    [ForeignKey("ProdutoId")]
    public Produto Produto { get; set; }

    [Display(Name = "Tamanho")]
    [Required(ErrorMessage = "Por favor, informe o Tamanho")]
    public int TamanhoId { get; set; }
    [ForeignKey("TamanhoID")]
    public Tamanho Tamanho { get; set; }

    [Display(Name = "Cor")]
    [Required(ErrorMessage = "Por favor, informe a Cor")]
    public int CorId { get; set; }
    [ForeignKey("CorId")]
    public Cor Cor { get; set; }

    [Display(Name = "Preço de Venda")]
    [Column(TypeName = "decimal(12,2)")] // 9.999.999.999,99
    public decimal? Preco { get; set; }

    [Display(Name = "Preço com Desconto")]
    [Column(TypeName = "decimal(2,2)")] // 999.999,99
    public decimal? PrecoDesconto { get; set; }

    [Display(Name = "Qtde em Estoque")]
    [Required(ErrorMessage = "Por favor, informe a Qtde em Estoque")]
    public int QtdeEstoque { get; set; }
}