using System;

namespace tcp_server
{
    class Program
    {
        static void Main(string[] args)
        {
            MainController controller = new MainController();

            controller.Run();
        }
    }
}
