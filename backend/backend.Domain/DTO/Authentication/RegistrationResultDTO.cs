using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTO.Authentication
{
    public class RegistrationResultDTO
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
    }

}
