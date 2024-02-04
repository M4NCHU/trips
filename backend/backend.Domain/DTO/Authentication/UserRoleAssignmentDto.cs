using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.DTO.Authentication
{
    public class UserRoleAssignmentDto
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}
