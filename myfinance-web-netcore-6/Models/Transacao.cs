using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myfinance_web_netcore.Models
{
    public class Transacao
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O histórico é obrigatório")]
        [StringLength(200, ErrorMessage = "O histórico deve ter no máximo 200 caracteres")]
        [Display(Name = "Histórico")]
        public string Historico { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data é obrigatória")]
        [Display(Name = "Data")]
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O plano de contas é obrigatório")]
        [Display(Name = "Plano de Contas")]
        public int PlanoContasId { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Display(Name = "Valor")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Propriedade de navegação
        public virtual PlanoContas? PlanoContas { get; set; }

        // Propriedades calculadas para exibição
        public string ValorFormatado => Valor.ToString("C2");
        public string DataFormatada => Data.ToString("dd/MM/yyyy HH:mm");
        public string TipoTransacao => PlanoContas?.TipoDescricao ?? "";
    }
}
