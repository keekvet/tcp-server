using MessageCore.Models;
using MessageRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tcp_server.Services;

namespace tcp_server.Controllers
{
    class CheckUserExistController : BaseController
    {
        new public static void Handle(string data, Connection connection)
        {
            string userToFind = RequestConverter.DecomposeUserExist(data);
            User user = UserService.FindUser(userToFind);
            try
            {
                Package.Write(connection.TcpClient.GetStream(),
                    RequestConverter.ComposeUserExistResponse(!(user is null)));

            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is InvalidOperationException)
                    Console.WriteLine(ex);
                else
                    throw;
            }
        }
    }
}
