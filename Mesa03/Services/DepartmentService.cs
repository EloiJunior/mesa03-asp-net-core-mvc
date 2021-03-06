﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mesa03.Models;
using System.Threading.Tasks; // para usar o Task<> das operações assincronas
using Microsoft.EntityFrameworkCore; // para usar o .toListAsync() que vai chamar uma operação do Linq, poi o Linq só prepara, e espera outra ação chama-lo

namespace Mesa03.Services
{
    public class DepartmentService
    {
        private readonly Mesa03Context _context;
        //      readonly é usado para que essa dependencia não possa ser alterada

        //construtor para que essa injeção de dependencia possa ocorrer
        public DepartmentService(Mesa03Context context)
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
        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();  //isso vai acessar a minha tabela de dados relacionada a Operadores e me retornar em forma de lista
                   //await é pra avisar o compilador que vamos usar uma operação assincrona
        }

        //metodo personalizado Insert
        public async Task InsertAsync(Department department)
        {

            await _context.AddAsync(department);
            await _context.SaveChangesAsync();
            /*criado pelo Nelio Alves
            _context.Add(obj);      //adicionar o objeto recebido, na tabela de Seller, sozinho o Add não é suficiente, precisamos da linha abaixo
            _context.SaveChanges(); //porem é preciso confirmar a inserção através do SaveChanges
            */
        }
    }
}
