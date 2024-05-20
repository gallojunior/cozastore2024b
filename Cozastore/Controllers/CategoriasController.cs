using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cozastore.Data;
using Cozastore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Cozastore.Controllers;

[Authorize(Roles = "Administrador, Funcionário")]
public class CategoriasController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _host;

    public CategoriasController(AppDbContext context, IWebHostEnvironment host)
    {
        _context = context;
        _host = host;
    }

    // GET: Categorias
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.Categorias.Include(c => c.CategoriaPai);
        return View(await appDbContext.ToListAsync());
    }

    // GET: Categorias/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias
            .Include(c => c.CategoriaPai)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // GET: Categorias/Create
    public IActionResult Create()
    {
        ViewData["CategoriaPaiId"] = new SelectList(_context.Categorias, "Id", "Nome");
        return View();
    }

    // POST: Categorias/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Foto,Filtrar,Banner,CategoriaPaiId")] Categoria categoria, IFormFile Foto)
    {
        if (ModelState.IsValid)
        {
            if (!CategoriaExists(categoria))
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                if (Foto != null)
                {
                    string fileName = categoria.Id + Path.GetExtension(Foto.FileName);
                    string upload = Path.Combine(_host.WebRootPath, "img\\categorias");
                    string newFile = Path.Combine(upload, fileName);
                    using (var stream = new FileStream(newFile, FileMode.Create))
                    {
                        Foto.CopyTo(stream);
                    }
                    categoria.Foto = "\\img\\categorias\\" + fileName;
                    await _context.SaveChangesAsync();
                }
                TempData["Success"] = $"A Categoria '{categoria.Nome}' foi adicionado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            else
                ModelState.AddModelError(string.Empty, "Nome já cadastrado!!");
        }        
        ViewData["CategoriaPaiId"] = new SelectList(_context.Categorias, "Id", "Nome", categoria.CategoriaPaiId);
        return View(categoria);
    }

    // GET: Categorias/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null)
        {
            return NotFound();
        }
        ViewData["CategoriaPaiId"] = new SelectList(_context.Categorias, "Id", "Nome", categoria.CategoriaPaiId);
        return View(categoria);
    }

    // POST: Categorias/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Foto,Filtrar,Banner,CategoriaPaiId")] Categoria categoria, IFormFile NovaFoto)
    {
        if (id != categoria.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (NovaFoto != null)
                {
                    string fileName = categoria.Id + Path.GetExtension(NovaFoto.FileName);
                    string upload = Path.Combine(_host.WebRootPath, "img\\categorias");
                    string newFile = Path.Combine(upload, fileName);
                    using (var stream = new FileStream(newFile, FileMode.Create))
                    {
                        NovaFoto.CopyTo(stream);
                    }
                    categoria.Foto = "\\img\\categorias\\" + fileName;
                    await _context.SaveChangesAsync();
                }

                _context.Update(categoria);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(categoria))
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
        ViewData["CategoriaPaiId"] = new SelectList(_context.Categorias, "Id", "Nome", categoria.CategoriaPaiId);
        return View(categoria);
    }

    // GET: Categorias/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias
            .Include(c => c.CategoriaPai)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // POST: Categorias/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoriaExists(Categoria categoria)
    {
        if (categoria.Id == 0)
            return _context.Categorias.Any(e => e.Nome == categoria.Nome);
        else
            return _context.Categorias.Any(e => e.Id == categoria.Id);
    }
}

