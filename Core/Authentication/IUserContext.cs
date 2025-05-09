using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Authentication
{
    public interface IUserContext
    {
        public UserDto loggedInUser { get; }
    }
}
