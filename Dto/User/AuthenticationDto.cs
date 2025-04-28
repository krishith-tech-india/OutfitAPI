using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class AuthenticationDto
    {
        public string Password { get; set; } = null!;
        public string EmailOrPhone { get; set; } = null!;
    }
}
