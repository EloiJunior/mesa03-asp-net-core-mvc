using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;  //para usar o .ToListAsync() na expressão lambda
using Mesa03.Models;
using Mesa03.Services; //para usar SellerService
using Mesa03.Models.ViewModels; //para usar o ViewModel SellerFormViewModel no Metodo Create Get

namespace Mesa03.Controllers
{
    public class SellersController : Controller
    {
        /* Criado pelo CRUD, mas vamos mudar para o controller interagir somente com o Serviço, e o Serviço que irá interagir com o Banco de Dados
        private readonly Mesa03Context _context;

        public SellersController(Mesa03Context context)
        {
            _context = context;
        }
        */

        //declarar dependencia para o SellerService
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;
        //construtor para ele injetar a dependencia
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;  // o _departmentService da classe, recebe o departmentService do argumento
        }

        // metodos personalizados do CRUD e tambem criados
        
        //metodo personalizado Index
        // GET: Sellers
        public async Task<IActionResult> Index()
        {
            /*metodo gerado pelo CRUD
            return View(await _context.Seller.ToListAsync());
            */

            /*outra forma dada pelo Nelio Alves, sincrona
             var list = _sellerService.FindAll();
             return View(list);
            */

            //forma atual do metodo
            return View(await _sellerService.FindAllAsync());

        }
        /*
         //metodo personalizado Details
        // GET: Sellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _sellerService.Seller
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }
        */

        //Metodo personalizado Create Get
        // GET: Sellers/Create
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }
        
        // POST: Sellers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] //precisamos colocar esse anotation pra indicar que a ação abaixo é de Post e não de Get
        [ValidateAntiForgeryToken] //colocando outra anotação para impedir ataque CSRF: é quando alguem aproveita a seção de autenticação e coloca dados maliciosos
        public async Task<IActionResult> Create(/*[Bind("Id,Name,Email,BirthDate,BaseSalary,Department")]*/ Seller seller)
        {
            if (ModelState.IsValid)
            {
                /*passei a codificação abaixo criada pelo CRUD no controlador para o Serviço
                _context.Add(seller);
                await _context.SaveChangesAsync();
                */
                await _sellerService.InsertAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            return View(seller);
        }
        /*
        // GET: Sellers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Seller.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,BirthDate,BaseSalary")] Seller seller)
        {
            if (id != seller.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seller);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerExists(seller.Id))
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
            return View(seller);
        }

        // GET: Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Seller
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seller = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(seller);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerExists(int id)
        {
            return _context.Seller.Any(e => e.Id == id);
        }
        */
    }
}
