using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myfinance_web_netcore.Data;
using myfinance_web_netcore.Models;

namespace myfinance_web_netcore.Controllers
{
    public class PlanoContasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanoContasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlanoContas
        public async Task<IActionResult> Index()
        {
            var planoContas = await _context.PlanoContas
                .OrderBy(p => p.Codigo)
                .ToListAsync();
            
            return View(planoContas);
        }

        // GET: PlanoContas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planoContas = await _context.PlanoContas
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (planoContas == null)
            {
                return NotFound();
            }

            return View(planoContas);
        }

        // GET: PlanoContas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlanoContas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Descricao,Tipo")] PlanoContas planoContas)
        {
            if (ModelState.IsValid)
            {
                // Verificar se o código já existe
                var codigoExiste = await _context.PlanoContas
                    .AnyAsync(p => p.Codigo == planoContas.Codigo);

                if (codigoExiste)
                {
                    ModelState.AddModelError("Codigo", "Já existe um plano de contas com este código.");
                    return View(planoContas);
                }

                planoContas.DataCriacao = DateTime.Now;
                _context.Add(planoContas);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Plano de contas criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            
            return View(planoContas);
        }

        // GET: PlanoContas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planoContas = await _context.PlanoContas.FindAsync(id);
            if (planoContas == null)
            {
                return NotFound();
            }
            
            return View(planoContas);
        }

        // POST: PlanoContas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Descricao,Tipo,DataCriacao")] PlanoContas planoContas)
        {
            if (id != planoContas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar se o código já existe em outro registro
                    var codigoExiste = await _context.PlanoContas
                        .AnyAsync(p => p.Codigo == planoContas.Codigo && p.Id != planoContas.Id);

                    if (codigoExiste)
                    {
                        ModelState.AddModelError("Codigo", "Já existe um plano de contas com este código.");
                        return View(planoContas);
                    }

                    _context.Update(planoContas);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Plano de contas atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanoContasExists(planoContas.Id))
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
            
            return View(planoContas);
        }

        // GET: PlanoContas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planoContas = await _context.PlanoContas
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (planoContas == null)
            {
                return NotFound();
            }

            return View(planoContas);
        }

        // POST: PlanoContas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planoContas = await _context.PlanoContas.FindAsync(id);
            
            if (planoContas != null)
            {
                _context.PlanoContas.Remove(planoContas);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Plano de contas excluído com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlanoContasExists(int id)
        {
            return _context.PlanoContas.Any(e => e.Id == id);
        }
    }
}
