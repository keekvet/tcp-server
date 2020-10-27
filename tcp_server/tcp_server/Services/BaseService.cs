using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcp_server.Services
{
    abstract class BaseService
    {
        protected static ApplicationContext context = ApplicationContext.getContext();
    }
}
