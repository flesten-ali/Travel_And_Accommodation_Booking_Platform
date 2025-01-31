using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TABP.Domain.Interfaces.Auth;
public interface IPasswordHasher
{
    string Hash(string password);
    bool VerifyPassword(string hashedPassword, string providedPassword);
}
