using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myfinance_web_netcore.Data;
using myfinance_web_netcore.Models;

namespace myfinance_web_netcore.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RelatoriosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> RelatorioTransacoes(DateTime? dataInicio, DateTime? dataFim, string? filtroTipo, int? planoContasId)
        {
            // Definir período padrão (último mês se não informado)
            if (!dataInicio.HasValue)
                dataInicio = DateTime.Now.AddMonths(-1).Date;
            
            if (!dataFim.HasValue)
                dataFim = DateTime.Now.Date.AddDays(1).AddTicks(-1); // Final do dia atual

            var query = _context.Transacoes
                .Include(t => t.PlanoContas)
                .Where(t => t.Data >= dataInicio && t.Data <= dataFim)
                .AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filtroTipo))
            {
                if (filtroTipo == "Receita")
                {
                    query = query.Where(t => t.PlanoContas!.Tipo == TipoPlanoContas.Receita);
                }
                else if (filtroTipo == "Despesa")
                {
                    query = query.Where(t => t.PlanoContas!.Tipo == TipoPlanoContas.Despesa);
                }
            }

            if (planoContasId.HasValue)
            {
                query = query.Where(t => t.PlanoContasId == planoContasId.Value);
            }

            var transacoes = await query.OrderBy(t => t.Data).ToListAsync();

            // Calcular totais
            var totalReceitas = transacoes.Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Receita).Sum(t => t.Valor);
            var totalDespesas = transacoes.Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Despesa).Sum(t => t.Valor);
            var saldoPeriodo = totalReceitas - totalDespesas;

            // Agrupar por tipo para facilitar a exibição
            var receitasPorPlano = transacoes
                .Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Receita)
                .GroupBy(t => t.PlanoContas)
                .Select(g => new
                {
                    PlanoContas = g.Key,
                    Total = g.Sum(t => t.Valor),
                    Quantidade = g.Count()
                })
                .OrderBy(x => x.PlanoContas?.Codigo)
                .ToList();

            var despesasPorPlano = transacoes
                .Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Despesa)
                .GroupBy(t => t.PlanoContas)
                .Select(g => new
                {
                    PlanoContas = g.Key,
                    Total = g.Sum(t => t.Valor),
                    Quantidade = g.Count()
                })
                .OrderBy(x => x.PlanoContas?.Codigo)
                .ToList();

            // Preparar dados para a view
            ViewBag.DataInicio = dataInicio?.ToString("yyyy-MM-dd");
            ViewBag.DataFim = dataFim?.ToString("yyyy-MM-dd");
            ViewBag.FiltroTipo = filtroTipo;
            ViewBag.TotalReceitas = totalReceitas;
            ViewBag.TotalDespesas = totalDespesas;
            ViewBag.SaldoPeriodo = saldoPeriodo;
            ViewBag.ReceitasPorPlano = receitasPorPlano;
            ViewBag.DespesasPorPlano = despesasPorPlano;

            // Carregar planos de contas para o filtro
            ViewBag.PlanoContas = new SelectList(
                await _context.PlanoContas.OrderBy(p => p.Codigo).ToListAsync(),
                "Id", "Descricao", planoContasId);

            return View(transacoes);
        }

        public async Task<IActionResult> GraficoReceitasDespesas(DateTime? dataInicio, DateTime? dataFim)
        {
            // Definir período padrão (último mês se não informado)
            if (!dataInicio.HasValue)
                dataInicio = DateTime.Now.AddMonths(-1).Date;
            
            if (!dataFim.HasValue)
                dataFim = DateTime.Now.Date.AddDays(1).AddTicks(-1);

            var transacoes = await _context.Transacoes
                .Include(t => t.PlanoContas)
                .Where(t => t.Data >= dataInicio && t.Data <= dataFim)
                .ToListAsync();

            // Calcular totais
            var totalReceitas = transacoes.Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Receita).Sum(t => t.Valor);
            var totalDespesas = transacoes.Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Despesa).Sum(t => t.Valor);

            // Dados para gráfico por categoria
            var receitasPorCategoria = transacoes
                .Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Receita)
                .GroupBy(t => t.PlanoContas?.Descricao)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Total = g.Sum(t => t.Valor)
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            var despesasPorCategoria = transacoes
                .Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Despesa)
                .GroupBy(t => t.PlanoContas?.Descricao)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Total = g.Sum(t => t.Valor)
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            // Dados para gráfico mensal (últimos 6 meses)
            var dadosMensais = new List<object>();
            for (int i = 5; i >= 0; i--)
            {
                var mesInicio = DateTime.Now.AddMonths(-i).Date.AddDays(1 - DateTime.Now.AddMonths(-i).Day);
                var mesFim = mesInicio.AddMonths(1).AddDays(-1);

                var receitasMes = transacoes
                    .Where(t => t.Data >= mesInicio && t.Data <= mesFim && t.PlanoContas?.Tipo == TipoPlanoContas.Receita)
                    .Sum(t => t.Valor);

                var despesasMes = transacoes
                    .Where(t => t.Data >= mesInicio && t.Data <= mesFim && t.PlanoContas?.Tipo == TipoPlanoContas.Despesa)
                    .Sum(t => t.Valor);

                dadosMensais.Add(new
                {
                    Mes = mesInicio.ToString("MMM/yyyy"),
                    Receitas = receitasMes,
                    Despesas = despesasMes,
                    Saldo = receitasMes - despesasMes
                });
            }

            // Preparar dados para a view
            ViewBag.DataInicio = dataInicio?.ToString("yyyy-MM-dd");
            ViewBag.DataFim = dataFim?.ToString("yyyy-MM-dd");
            ViewBag.TotalReceitas = totalReceitas;
            ViewBag.TotalDespesas = totalDespesas;
            ViewBag.SaldoPeriodo = totalReceitas - totalDespesas;
            ViewBag.ReceitasPorCategoria = receitasPorCategoria;
            ViewBag.DespesasPorCategoria = despesasPorCategoria;
            ViewBag.DadosMensais = dadosMensais;

            return View();
        }

        // API para dados do gráfico (AJAX)
        [HttpGet]
        public async Task<IActionResult> DadosGrafico(DateTime? dataInicio, DateTime? dataFim, string tipo = "pizza")
        {
            if (!dataInicio.HasValue)
                dataInicio = DateTime.Now.AddMonths(-1).Date;
            
            if (!dataFim.HasValue)
                dataFim = DateTime.Now.Date.AddDays(1).AddTicks(-1);

            var transacoes = await _context.Transacoes
                .Include(t => t.PlanoContas)
                .Where(t => t.Data >= dataInicio && t.Data <= dataFim)
                .ToListAsync();

            if (tipo == "pizza")
            {
                var totalReceitas = transacoes.Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Receita).Sum(t => t.Valor);
                var totalDespesas = transacoes.Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Despesa).Sum(t => t.Valor);

                return Json(new
                {
                    labels = new[] { "Receitas", "Despesas" },
                    data = new[] { totalReceitas, totalDespesas },
                    backgroundColor = new[] { "#28a745", "#dc3545" }
                });
            }

            return Json(new { error = "Tipo não suportado" });
        }
    }
}
