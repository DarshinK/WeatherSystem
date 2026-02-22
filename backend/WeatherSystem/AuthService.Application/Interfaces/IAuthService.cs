using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(string email, string password);
    Task<string> LoginAsync(string email, string password);
}

