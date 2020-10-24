using System;

namespace tcp_server
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationContext applicationContext = new ApplicationContext();

            Controller controller = new Controller();

            controller.RunListener();

            Console.WriteLine("started");

            while (true)
            {
                controller.getClient();
            }
        }
    }
}
