using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Controllers
{
    class RegistrationController : BaseController
    {
        private static RegistrationController controller;

        private RegistrationController() { }

        new public static void Handle(string data)
        {
            throw new NotImplementedException();
        }
    }
}
