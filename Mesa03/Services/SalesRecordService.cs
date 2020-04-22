using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // para usar as Tasks de encapsulamento do assincrono
using Mesa03.Models; //para reconhecer o Mesa03Context que esta no namespace Mesa03.Models
using Mesa03.Services.Exceptions;
using Microsoft.EntityFrameworkCore; //para usar a operação ".ToListAsync()" na expressão lambda do metodo FindAll, e para usar...
                                     // e para usar o Include(seller => seller.Department), que usaremos para fazer o Join das tabelas

namespace Mesa03.Services
{
    public class SalesRecordService
    {
        private readonly Mesa03Context _context;

        public SalesRecordService(Mesa03Context context)
        {
            _context = context;
        }

        /*
        // GET: SalesRecords
        public async Task<IActionResult> Index()
        {
            return View(await _context.SalesRecord.ToListAsync());
        }
        */



        //Metodo personalizado FindByDateAsync, GET
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            //construir um objeto IQueryable, que é o objeto que podemos construir as consultas em cima dele, a partir do Db context do SalesRecord
            var result = from obj in _context.SalesRecord select obj; //de um objeto DbSet do context, seleciona transformando esse objeto em um objeto IQueryable

            //Agora que tenho uma variavel IQueryable, podemos acrescentar outros detalhes na consulta
            if (minDate.HasValue) // se no argumento de entrada tiver uma data minima
            {
                result = result.Where(x => x.Date >= minDate.Value); //eu pego a variavel criada, e filtro, solicitando que pegue os objetos x => que tenham data => maior ou igual, ao minDate que chegou no argumento, trazendo os Value(conteudo) desses objetos filtrados
            }

            if (maxDate.HasValue) // se no argumento de entrada tiver uma data maxima
            {
                result = result.Where(x => x.Date <= maxDate.Value); //eu pego a variavel criada, e filtro, solicitando que pegue os objetos x => que tenham data => menor ou igual, ao maxDate que chegou no argumento, trazendo os Value(conteudo) desses objetos filtrados
            }

            /*Essa ação retornaria a lista para a View, só que como vamos um Join com a tabela de vendedor e tambem com a tabela de Departamento, vai ficar somente como didatica
            return result.ToList();
            */

            //vai ficar assim:
            return await result                       //retorna a variavel lista filtrada pelos criterios acima
                .Include(x => x.Seller)              //em cada objeto x dessa lista faz um join com o vendedor da tabela Seller
                .Include(x => x.Seller.Department)   //em cada objeto x dessa lista com o join do vendedor, faz outro join com o departamento desse vendedor da tabela Department                                 
                .OrderByDescending(x => x.Date)      //agora que a variavel esta completa, ordena os objetos pela data dos objetos
                .ToListAsync();                      //transformando essa variavel em lista para apresentar uma List<SalesRecord>

        }

        /*
        // GET: SalesRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            { 
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesRecord == null)
            {
                return NotFound();
            }

            return View(salesRecord);
        }
        */

        /*
        // GET: SalesRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalesRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Amount,Status")] SalesRecord salesRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salesRecord);
        }
        */

        /*
        // GET: SalesRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord.FindAsync(id);
            if (salesRecord == null)
            {
                return NotFound();
            }
            return View(salesRecord);
        }
        */

        /*
        // POST: SalesRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Amount,Status")] SalesRecord salesRecord)
        {
            if (id != salesRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesRecordExists(salesRecord.Id))
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
            return View(salesRecord);
        }

        // GET: SalesRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesRecord = await _context.SalesRecord
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salesRecord == null)
            {
                return NotFound();
            }

            return View(salesRecord);
        }
        */

        /*
        // POST: SalesRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesRecord = await _context.SalesRecord.FindAsync(id);
            _context.SalesRecord.Remove(salesRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        /*
        private bool SalesRecordExists(int id)
        {
            return _context.SalesRecord.Any(e => e.Id == id);
        }
        */

    }
}
