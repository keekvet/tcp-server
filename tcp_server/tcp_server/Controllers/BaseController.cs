using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Controllers
{
    abstract class BaseController
    {
        public static void Handle(string data, Connection connection)
        {
            throw new NotImplementedException();
        }
    }
}
