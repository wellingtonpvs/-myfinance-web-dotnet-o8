using System.ComponentModel.DataAnnotations;

namespace myfinance_web_netcore.Models
{
    public class PlanoContas
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O código é obrigatório")]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(100, ErrorMessage = "A descrição deve ter no máximo 100 caracteres")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo é obrigatório")]
        [Display(Name = "Tipo")]
        public TipoPlanoContas Tipo { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Propriedade para exibição formatada do tipo
        public string TipoDescricao => Tipo == TipoPlanoContas.Receita ? "Receita" : "Despesa";
    }

    public enum TipoPlanoContas
    {
        [Display(Name = "Despesa")]
        Despesa = 0,
        
        [Display(Name = "Receita")]
        Receita = 1
    }
}
