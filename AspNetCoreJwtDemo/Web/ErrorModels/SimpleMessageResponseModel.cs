using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ErrorModels
{
    public class SimpleMessageResponseModel
    {
        public string Message { get; set; }

        public SimpleMessageResponseModel(string errorMessage)
        {
            Message = errorMessage;
        }
    }
}
