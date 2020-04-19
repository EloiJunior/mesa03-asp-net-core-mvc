using System.Collections.Generic;  // para usar ICollection
using System; // para usar o DateTime no metodo TotalSales
using System.Linq; // para usar expressões Linq no metodo TotalSales

namespace Mesa03.Models
{
    public class Department
    {
        //primeiro atributos basicos
        public int Id { get; set; }          //atributo basico
        public string Name { get; set; }     //atributo basico

        //segundo associações com outras classes
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>(); //associação com outra classe 1 pra varios, instanciando a lista

        //terceiro construtores
        public Department()                  //construtor vazio obs:só preciso criar construtor vazio, pq vou criar construtor com argumento
        {                                    // se não fosse criar construtor com argumento, não precisaria criar construtor vazio
        }

        public Department(int id, string name)   //construtor com argumento, para os atributos que não são coleções
        {
            Id = id;
            Name = name;
        }

        //quarto Metodos
        public void AddSeller(Seller seller) //metodo para adicionar 1 vendedor para a lista de vendedores desse departamento instanciada acima "Sellers"
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            //operação do Linq
            //                 expressão Lambda
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
                                                //esse metodo "TotalSales" foi criado na classe Seller, não é o método descrito na classe departamento
            //retorna a soma de todos vendedores desse departamento que estão na lista instanciada Sellers, 
            //                 mas somentes os vendedores tal que "=>" retorne os valores do metodo "TotalSales" usando a data inicial e final que entrou como argumento aqui
        }
          
    }
}
