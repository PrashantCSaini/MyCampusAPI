using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCampus
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string username, int id, char type);
    }
}
