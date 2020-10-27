using System;

namespace tcp_server
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionController controller = new ConnectionController();

            controller.Run();
        }
    }
}
