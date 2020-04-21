using System;
using System.Collections.Generic;   //para usar ICollection
using System.Linq;                  // para usar as funções do Linq no metodo TotalSales
using System.ComponentModel.DataAnnotations; //para usar os displays de anotations, ou formatação
namespace Mesa03.Models
{
    public class Seller
    {
        //primeiro atributos basicos
        public int Id { get; set; }                  //atributo basico

        [Required(ErrorMessage = "{0} required")]                      //campo requerido, com mensagem de erro
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be {2} and {1}")]  //tamanho do texto: maximo 60, minimo 3, mensagem de erro
        public string Name { get; set; }             //atributo basico

        [Required(ErrorMessage = "{0} required")]                      //campo requerido, com mensagem de erro
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [DataType(DataType.EmailAddress)]                             //DataType: muda o tipo de texto para email
        public string Email { get; set; }            //atributo basico

        [Required(ErrorMessage = "{0} required")]                      //campo requerido, com mensagem de erro
        [Display(Name = "Birth Date")]                                //Diplay: muda a apresentação para o que colocarmos entre aspas
        [DataType(DataType.Date)]                                     //muda o tipo de data e hora para somente data mm/dd/aaaa
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}")]           // muda a apresentação de mm/dd/aaa para dd/mm/aaaa
        public DateTime BirthDate { get; set; }      //atributo basico

        [Required(ErrorMessage = "{0} required")]                      //campo requerido, com mensagem de erro
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]                     //de valor sem decimais para 2 decimais
        public double BaseSalary { get; set; }       //atributo basico

        //segundo associações
        public Department Department { get; set; }            //associação com outra classe, de 1 pra 1
        public int DepartmentId { get; set; }                 //associação criada para Chave estrangeira não Nula, ou seja...
                                                              // ...ninguem conseguira criar um vendedor sem o Id do departamento dele...
                                                              //...pois o tipo int criado no DB não permite campo nulo

        public ICollection<SalesRecord> Sales { get; set; }  //associação com outra classe, de 1 pra varios

        //terceiro Construtores
        public Seller()                                      //construtor vazio
        {
        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department) //construtores com argumento
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        //quarto Metodos Customizados
        public void AddSales(SalesRecord sr)    //metodo para inserir uma venda do vendedor para a Tabela de registro de vendas
        {
            Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr) // metodo para remover uma venda do vendedor da Tabela de registro de vendas
        {
            Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final) //metodo para retornar o Total de Vendas do vendedor em um periodo
        {
            //chama a lista ICollection instanciada na associação acima "Sales",
            //           Filtra essa lista pegando os objetos que acabamos de chamar sr "Where"
            //                    Tal que "=>", os objetos que tenham a data maiores ou iguais a data inicial
            //                                          E "&&" menores ou iguais a data final fiquem nessa lista filtrada
            //                                                              Então somasse os valores desses objetos
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
            //Chamando a lista Sales
            //          Agora chamando operações do Linq ".Where"
            //                 Colocando uma expressão Lambda para filtrar as vendas "sr => ..."
            //                                                               Calcular a Soma baseado em outra expressão Lambda "sr =>..."
        }
    }
}
