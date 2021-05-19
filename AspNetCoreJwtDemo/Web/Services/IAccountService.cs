using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Services
{
    public interface IAccountService
    {
        Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest);
    }
}
