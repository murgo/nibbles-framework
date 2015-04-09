using System;

namespace SnakeInvaders
{
    class Program
    {
        static void Main(string[] args)
        {
            int port;
            var launcher = new Launcher(args.Length > 1 ? args[1] : "box.murgo.iki.fi", args.Length > 2 && int.TryParse(args[2], out port) ? port : 6969);

            launcher.Go();

            Console.WriteLine("Hit enter to quit");
            Console.ReadLine();
        }
    }
}
