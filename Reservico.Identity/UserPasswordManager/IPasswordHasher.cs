using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservico.Identity.UserPasswordManager
{
    public interface IPasswordHasher
    {
        string Hash(string password, string securityStamp);

        bool Verify(string input, string securityStamp, string passwordHash);
    }
}
