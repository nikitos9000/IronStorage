using System;
using System.ServiceModel;

namespace IronLord
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
//        	new LordRegistrationService().RegisterUser("yodawg", "mypass");
        	new LordPeasantService().StartFileCreate("MyFileName", new Guid(), 100, new byte[] {1, 2, 3});

            using (ServiceHost host = new ServiceHost(typeof(LordPeasantService)))
            {
                host.Open();
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();
                host.Close();
            }

//            ServiceBase[] servicesToRun = new ServiceBase[]
//                                              {
//                                                  new IronService()
//                                              };
//            ServiceBase.Run(servicesToRun);
        }
    }
}