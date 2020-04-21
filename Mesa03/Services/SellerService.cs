using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mesa03.Models; //para reconhecer o Mesa03Context que esta no namespace Mesa03.Models
using Microsoft.EntityFrameworkCore; //para usar a operação ".ToListAsync()" na expressão lambda do metodo FindAll, e para usar...
                                     // e para usar o Include(seller => seller.Department), que usaremos para fazer o Join das tabelas

namespace Mesa03.Services
{

    //injeção de dependencia para o DbContext
    public class SellerService                       
    {
        private readonly Mesa03Context _context;
        //      readonly é usado para que essa dependencia não possa ser alterada

        //construtor para que essa injeção de dependencia possa ocorrer
        public SellerService(Mesa03Context context)
        {
            _context = context;  //esse construtor vai atribuir para o _context esse context que veio como argumento
        }

        //Metodo FindAllAsync

        /*implementando a operação findall  sincrona
        public List<Seller> FindAll()
        {
            return _context.Seller.ToList(); //vai acessar a tabela de Seller e converter para Lista
        }
        */

        //agora que temos a nossa dependencia do context, vamos criar uma operação FindAll para retornar uma lista com todos os operadores do banco de dados
        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.Include(x => x.Department).OrderBy(x => x.Name).ToListAsync();  //isso vai acessar a minha tabela de dados relacionada a Operadores e me retornar em forma de lista
        }

        //metodo personalizado Insert
        public async Task InsertAsync(Seller seller)
        {
            
            await _context.AddAsync(seller);
            await _context.SaveChangesAsync();
            /*criado pelo Nelio Alves
            _context.Add(obj);      //adicionar o objeto recebido, na tabela de Seller, sozinho o Add não é suficiente, precisamos da linha abaixo
            _context.SaveChanges(); //porem é preciso confirmar a inserção através do SaveChanges
            */
        }

        //metodo personalizado FindByIdAsync
        public async Task<Seller> FindByIdAsync(int id)
        {
            /*Sem fazer o Join das tabelas, só retorna o que esta na tabela Seller
            return await _context.Seller.FirstOrDefaultAsync(seller => seller.Id == id);
            */

            //fazendo o Join das Tabelas Seller e Department usando Include
            return await _context.Seller.Include(seller => seller.Department).FirstOrDefaultAsync(seller => seller.Id == id);
                                         /////////////////////////////////////
        }

        //metodo personalizado RemoveAsync
        public async Task RemoveAsync(int id)
        {
            var obj =  await _context.Seller.FindAsync(id);
            _context.Seller.Remove(obj);
            await _context.SaveChangesAsync();
        }


    }

}
