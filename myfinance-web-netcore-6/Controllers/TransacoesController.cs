using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myfinance_web_netcore.Data;
using myfinance_web_netcore.Models;

namespace myfinance_web_netcore.Controllers
{
    public class TransacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transacoes
        public async Task<IActionResult> Index(string? filtroTipo, DateTime? dataInicio, DateTime? dataFim, int? planoContasId)
        {
            var query = _context.Transacoes.Include(t => t.PlanoContas).AsQueryable();

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

            if (dataInicio.HasValue)
            {
                query = query.Where(t => t.Data >= dataInicio.Value);
            }

            if (dataFim.HasValue)
            {
                query = query.Where(t => t.Data <= dataFim.Value);
            }

            if (planoContasId.HasValue)
            {
                query = query.Where(t => t.PlanoContasId == planoContasId.Value);
            }

            var transacoes = await query.OrderByDescending(t => t.Data).ToListAsync();

            // Carregar dados para os filtros
            ViewBag.PlanoContas = new SelectList(
                await _context.PlanoContas.OrderBy(p => p.Codigo).ToListAsync(),
                "Id", "Descricao", planoContasId);

            ViewBag.FiltroTipo = filtroTipo;
            ViewBag.DataInicio = dataInicio?.ToString("yyyy-MM-dd");
            ViewBag.DataFim = dataFim?.ToString("yyyy-MM-dd");

            // Calcular resumo
            ViewBag.TotalReceitas = transacoes.Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Receita).Sum(t => t.Valor);
            ViewBag.TotalDespesas = transacoes.Where(t => t.PlanoContas?.Tipo == TipoPlanoContas.Despesa).Sum(t => t.Valor);
            ViewBag.Saldo = ViewBag.TotalReceitas - ViewBag.TotalDespesas;

            return View(transacoes);
        }

        // GET: Transacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transacao = await _context.Transacoes
                .Include(t => t.PlanoContas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (transacao == null)
            {
                return NotFound();
            }

            return View(transacao);
        }

        // GET: Transacoes/Create
        public async Task<IActionResult> Create()
        {
            await CarregarPlanoContas();
            return View();
        }

        // POST: Transacoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Historico,Data,PlanoContasId,Valor")] Transacao transacao)
        {
            if (ModelState.IsValid)
            {
                transacao.DataCriacao = DateTime.Now;
                _context.Add(transacao);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Transação criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            await CarregarPlanoContas(transacao.PlanoContasId);
            return View(transacao);
        }

        // GET: Transacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transacao = await _context.Transacoes.FindAsync(id);
            if (transacao == null)
            {
                return NotFound();
            }

            await CarregarPlanoContas(transacao.PlanoContasId);
            return View(transacao);
        }

        // POST: Transacoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Historico,Data,PlanoContasId,Valor,DataCriacao")] Transacao transacao)
        {
            if (id != transacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transacao);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Transação atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransacaoExists(transacao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            await CarregarPlanoContas(transacao.PlanoContasId);
            return View(transacao);
        }

        // GET: Transacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transacao = await _context.Transacoes
                .Include(t => t.PlanoContas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (transacao == null)
            {
                return NotFound();
            }

            return View(transacao);
        }

        // POST: Transacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transacao = await _context.Transacoes.FindAsync(id);
            
            if (transacao != null)
            {
                _context.Transacoes.Remove(transacao);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Transação excluída com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TransacaoExists(int id)
        {
            return _context.Transacoes.Any(e => e.Id == id);
        }

        private async Task CarregarPlanoContas(object? selectedValue = null)
        {
            var planoContas = await _context.PlanoContas
                .OrderBy(p => p.Codigo)
                .Select(p => new
                {
                    Id = p.Id,
                    Display = $"{p.Codigo} - {p.Descricao} ({(p.Tipo == TipoPlanoContas.Receita ? "Receita" : "Despesa")})"
                })
                .ToListAsync();

            ViewBag.PlanoContasId = new SelectList(planoContas, "Id", "Display", selectedValue);
        }
    }
}
