using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class UpdateRoleDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? AddedOn { get; set; }
    }
}
