using MessageCore.Models;
using MessageRequest;
using System;
using System.Collections.Generic;
using System.Text;
using tcp_server.Services;

namespace tcp_server.Controllers
{
    class LoginController : BaseController
    {

        new public static void Handle(string data, Connection connection)
        {
            User user = RequestConverter.DecomposeLogin(data);

            User userFromDb = UserService.FindUser(user?.Name);

            connection.ConnectedUser = userFromDb;

            Package.Write(connection.TcpClient.GetStream(),
                RequestConverter.ComposeLoginResponse(user.Equals(userFromDb)));
        }
    }
}
