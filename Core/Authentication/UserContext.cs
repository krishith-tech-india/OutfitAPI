using Core.Authentication;
using Dto;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.Authentication
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserDto loggedInUser { get; }

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
            loggedInUser = new UserDto();

            if (claimsPrincipal != null)
            {
                var userId = claimsPrincipal.FindFirst("id")?.Value;
                if(userId != null)
                    loggedInUser.Id = Convert.ToInt32(userId);

                var email = claimsPrincipal.FindFirst("email")?.Value;
                if (!string.IsNullOrEmpty(email))
                    loggedInUser.Email = email;

                var phoneNo = claimsPrincipal.FindFirst("phone")?.Value;
                if (!string.IsNullOrEmpty(phoneNo))
                    loggedInUser.PhNo = phoneNo;
            }
        }
    }
}
