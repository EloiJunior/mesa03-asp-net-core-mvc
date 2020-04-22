using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mesa03.Models;
using Mesa03.Services; //para usar o SalesRecordService

namespace Mesa03.Controllers
{
    public class SalesRecordsController : Controller
    {
        /*Framework, declaração de dependencia do context do Db, passamos para o serviço
        private readonly Mesa03Context _context;
        */

        //Ao invés disso temos que fazer declaração de dependencia com o serviço
        private readonly SalesRecordService _salesRecordService;
        //              //Serviço Usado    //nome da variavel que vamos dar

        /*Framework, construtor da dependencia, passamos para o serviço
        public SalesRecordsController(Mesa03Context context)
        {
            _context = context;
        }
        */

        //ao inves disso temos que fazer um construtor de dependencia com o serviço
        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }
        //

        
        // GET: SalesRecords // Framework
        public async Task<IActionResult> Index()
        {
            return View();
        }
        

        // GET: SalesRecords // Personalizado
        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            //A operação comentada já retornaria sozinha o resultado, mas vamos criar um macete para mandar tambem a minDate e maxDate que chegaram no argumento para a tela de resposta
            //primeiro, vamos formatar que: se não entrar no argumento o minDate, vamos definir o primeiro dia do ano como minDate
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            //segundo, vamos formatar que: se não entrar no argumento o maxDate, vamos definir o dia de hoje como maxDate
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }
            //terceiro, vamos passar o minDate e maxDate utilizado no filtro, na view tela de resposta do SimpleSearch,...
            //...utilizando o dicionario ViewData
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            //essa operação sozinha já mandaria o resultado para a view
            return View(await _salesRecordService.FindByDateAsync(minDate, maxDate));
            //
        }


        
        // GET: SalesRecords // Personalizado
        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            //A operação comentada já retornaria sozinha o resultado, mas vamos criar um macete para mandar tambem a minDate e maxDate que chegaram no argumento para a tela de resposta
            //primeiro, vamos formatar que: se não entrar no argumento o minDate, vamos definir o primeiro dia do ano como minDate
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            //segundo, vamos formatar que: se não entrar no argumento o maxDate, vamos definir o dia de hoje como maxDate
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }
            //terceiro, vamos passar o minDate e maxDate utilizado no filtro, na view tela de resposta do SimpleSearch,...
            //...utilizando o dicionario ViewData
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            //essa operação sozinha já mandaria o resultado para a view
            return View(await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate));
            //
            
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
        */

        /*
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
