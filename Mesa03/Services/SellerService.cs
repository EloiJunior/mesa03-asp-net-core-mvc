using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mesa03.Models; //para reconhecer o Mesa03Context que esta no namespace Mesa03.Models
using Microsoft.EntityFrameworkCore; //para usar a operação ".ToListAsync()" na expressão lambda do metodo FindAll

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

        //Metodo FindAll ou ToListAsync

        /*implementando a operação findall  sincrona
        public List<Seller> FindAll()
        {
            return _context.Seller.ToList(); //vai acessar a tabela de Seller e converter para Lista
        }
        */

        //agora que temos a nossa dependencia do context, vamos criar uma operação FindAll para retornar uma lista com todos os operadores do banco de dados
        public async Task<List<Seller>> ToListAsync()
        {
            return await _context.Seller.OrderBy(x => x.Name).ToListAsync();  //isso vai acessar a minha tabela de dados relacionada a Operadores e me retornar em forma de lista
        }

    }

}
