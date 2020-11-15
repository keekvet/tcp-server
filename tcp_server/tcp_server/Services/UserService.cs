using MessageCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Services
{
    class UserService : BaseService
    {
        public static User FindUser(string name)
        {
            return context.Users.Find(name);
        }
    }
}
