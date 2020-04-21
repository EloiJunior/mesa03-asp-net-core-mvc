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
using Mesa03.Services.Exceptions;
using System.Diagnostics;

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
        //


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

        //metodo personalizado Details
        // GET: Sellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                /*erro simples
                return NotFound();
                */

                //erro personalizado
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            /*operação criada pelo CRUD vamos transferir para o Serviço
            var seller = await _sellerService.Seller
                .FirstOrDefaultAsync(m => m.Id == id);
            */

            //Codigo reescrito para chamar o resultado do Serviço
            var seller = await _sellerService.FindByIdAsync(id.Value);
            //

            if (seller == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(seller);
        }



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
            if (ModelState.IsValid) //para testar se o que veio na requisição é valido, ou seja que cumpriu todos os criterior de validação
            {
                /*passei a codificação abaixo criada pelo CRUD no controlador para o Serviço
                _context.Add(seller);
                await _context.SaveChangesAsync();
                */
                await _sellerService.InsertAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            //return View(seller); // se o que veio na requisição não é validado volta pra view as informações que vieram para serem corrigidas
            var departments = await _departmentService.FindAllAsync(); //como a view Edit exige viewModel, precisamos criar uma tabelinha de departamentos
            var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments }; //e depois de criada a tabelinha de departments, geramos um SellerFormViewModel, com o seller trazido na requisição com a listinha de departamentos
            return View(viewModel);  ////...já retorna pra view, o viewModel que acabamos de criar, com base na requisição mais a tabelinha de departmentos

        }


        // GET: Sellers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            /*Operação criada pelo CRUD no Controlador, que vamo transpor para o Serviço
            var seller = await _context.Seller.FindAsync(id);
            */

            //operação acima chamando do Serviço
            var seller = await _sellerService.FindByIdAsync(id.Value);
            //

            if (seller == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            /*essa operação foi criada pelo CRUD quando o Seller ainda não tinha SellerFormViewModel fazendo associação com Departamento
            return View(seller);
            */

            //Agora vamos criar abaixo as condições para que seja apresentado para edição do vendedor as opções de departamento tambem
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
            return View(viewModel);
            //
        }



        // POST: Sellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,/* [Bind("Id,Name,Email,BirthDate,BaseSalary")] */Seller seller)
        {
            if (!ModelState.IsValid)  //outra forma de testar a validação antes de passar para as outras codificações, ou seja, se não for validado,...
            {
                //return View(seller); //como o View Edit espera um SellerFormViewModel, precisamos criar ele antes de devolver a requisição
                var departments = await _departmentService.FindAllAsync(); //como a view Edit exige viewModel, precisamos criar uma tabelinha de departamentos
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments }; //e depois de criada a tabelinha de departments, geramos um SellerFormViewModel, com o seller trazido na requisição com a listinha de departamentos
                return View(viewModel);  ////...já retorna pra view, o viewModel que acabamos de criar, com base na requisição mais a tabelinha de departmentos
            }

            if (id != seller.Id) //se o id que veio da requisição não for o mesmo Id do seller no DB
            {
                /*veio do CRUD
                return NotFound();
                */

                //mudamos para o erro abaixo
                //return BadRequest();
                //

                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            try
            {
                /*Veio do CRUD passamos para o Serviço
                _context.Update(seller);
                await _context.SaveChangesAsync();
                */

                //Chamando a operação acima do Serviço
                await _sellerService.UpdateAsync(seller);
                //
            }

            //não veio no CRUD mas vamos colocar
            catch (NotFoundException e)
            {
                //return NotFound();
                //erro personalizado voltando a mensagem padrao do framework e.Message
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            //

            catch (DbConcurrencyException e)  //veio do CRUD DbUpdateConcurrencyException, que pegamos no serviço como esse ao lado
            {
                /*Veio no CRUD
                if (!SellerExists(seller.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                */

                //porenquanto fizemos como abaixo
                //return BadRequest();

                return RedirectToAction(nameof(Error), new { message = e.Message });
                //

            }
            return RedirectToAction(nameof(Index));


            /*veio no CRUD
            return View(seller);
            */

        }




        // GET: Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            //Feito pelo CRUD e alterado para serviço
            /*
            var seller = await _context.Seller
                .FirstOrDefaultAsync(m => m.Id == id);
            */

            //passando a codificação acima para o Serviço, e chamando a ação do serviço
            var seller = await _sellerService.FindByIdAsync(id.Value);
            //

            if (seller == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(seller);
        }
    



        // POST: Sellers/Delete/6
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            /*
            var seller = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(seller);
            await _context.SaveChangesAsync();
            */

            //passando a codificação acima para o Serviço, e chamando a ação do serviço
            await _sellerService.RemoveAsync(id);
            //

            return RedirectToAction(nameof(Index));
        }

        //Ação de Erro personalizado
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel  // instanciar um viewmodel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier //macete pra pegar o Id interno da requisição
            };
            return View(viewModel);
        }


        /*
        private bool SellerExists(int id)
        {
            return _context.Seller.Any(e => e.Id == id);
        }
        */
    }
}
