using System;

namespace Mesa03.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string Message { get; set; }  //criado para retornar as mensagens de erro customizadas

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}