using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLauncher
{
    class ConsoleHost
    {

        static string TITLE_TEXT = "MembershipSystemService -- Console Host ({0})" + (System.Diagnostics.Debugger.IsAttached ? " [DEBUG]" : "");
        static void Main(string[] args)
        {
            Console.Title = String.Format(TITLE_TEXT, "Not Running");
            try
            {
                ServiceHost host = new ServiceHost(typeof(MembershipSystem.Program));

                Console.Title = String.Format(TITLE_TEXT, "Starting");
                host.Open();

                Console.Title = String.Format(TITLE_TEXT, "Running");
                Console.WriteLine("Service is started. Press any key to exit.");
                Console.ReadyKey();

                Console.Title = String.Format(TITLE_TEXT, "Closing");
                host.Close();
                host = null;
                Console.Title = String.Format(TITLE_TEXT, "Closed");
            }
            catch (Exception ex)
            {
                Console.Title = String.Format(TITLE_TEXT, "Exception");
                Console.WriteLine("An error occurred while running the host");
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
