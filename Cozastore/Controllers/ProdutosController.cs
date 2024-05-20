using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cozastore.Data;
using Cozastore.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cozastore.Controllers;

[Authorize(Roles = "Administrador, Funcionário")]
public class ProdutosController : Controller
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Produtos
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetProdutos()
    {
        var data = _context.Produtos
            .Include(p => p.Categoria)
            .ToList();
        return Ok(new { data = data });
    }

    public async Task<IActionResult> GetAll()
    {
        var produtos = await _context.Produtos
            .Include(p => p.Categoria)
            .ToListAsync();
        return Json(new { data = produtos });
    }

    // GET: Produtos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (produto == null)
        {
            return NotFound();
        }

        return View(produto);
    }

    // GET: Produtos/Create
    public IActionResult Create()
    {
        ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
        return View();
    }

    // POST: Produtos/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,DescricaoResumida,DescricaoCompleta,SKU,Preco,PrecoDesconto,Destaque,Peso,Material,Dimensao,CategoriaId")] Produto produto)
    {
        if (ModelState.IsValid)
        {
            if (!ProdutoExists(produto))
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
            }
            else
                ModelState.AddModelError(string.Empty, "Nome já cadastrado");

            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", produto.CategoriaId);
        return View(produto);
    }

    // GET: Produtos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
        {
            return NotFound();
        }
        ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", produto.CategoriaId);
        return View(produto);
    }

    // POST: Produtos/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DescricaoResumida,DescricaoCompleta,SKU,Preco,PrecoDesconto,Destaque,Peso,Material,Dimensao,CategoriaId")] Produto produto)
    {
        if (id != produto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(produto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(produto))
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
        ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", produto.CategoriaId);
        return View(produto);
    }

    // GET: Produtos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await _context.Produtos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (produto == null)
        {
            return NotFound();
        }

        return View(produto);
    }

    // POST: Produtos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto != null)
        {
            _context.Produtos.Remove(produto);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProdutoExists(Produto produto)
    {
        if (produto.Id == 0)
            return _context.Produtos.Any(e => e.Nome == produto.Nome);
        else
            return _context.Produtos.Any(e => e.Id == produto.Id);
    }
}
