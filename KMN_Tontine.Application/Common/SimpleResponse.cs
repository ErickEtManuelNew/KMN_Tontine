using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Application.Common
{
    public class SimpleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        // Méthodes statiques pour simplifier la création d'instances
        public static SimpleResponse Ok(string message = "Operation successful")
        {
            return new SimpleResponse { Success = true, Message = message };
        }

        public static SimpleResponse Error(string message = "Operation failed")
        {
            return new SimpleResponse { Success = false, Message = message };
        }
    }
}
