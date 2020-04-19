using System;
using Mesa03.Models.Enums;

namespace Mesa03.Models
{
    public class SalesRecord
    {
        //primeiro atributos basicos
        public int Id { get; set; }               //atributo basico
        public DateTime Date { get; set; }        //atributo basico
        public double Amount { get; set; }        //atributo basico
        public SaleStatus Status { get; set; }    //atributo basico

        //segundo associações com outras classes
        public Seller Seller { get; set; }        //associação com outra classe, de 1 pra 1

        //terceiro construtores                   // construtor vazio
        public SalesRecord()
        {
        }

        public SalesRecord(int id, DateTime date, double amount, SaleStatus status, Seller seller)  //construtor com argumento, como todos atributos e associações não são coleções, criamos construtores para todos atributos e tambem associações
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }
    }
}
