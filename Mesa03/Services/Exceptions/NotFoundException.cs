using System;

namespace Mesa03.Services.Exceptions
{
    public class NotFoundException : ApplicationException    //essa classe que criei vai herdar ":" do Application Exception
    {
        //construtor basico
        public NotFoundException(string message) : base(message)   //recebendo um string message, e esse construtor vai repassa a mensagem para a classe base
        {
        }
    }
}
