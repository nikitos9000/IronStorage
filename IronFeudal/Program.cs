using System;
using System.ServiceModel;

namespace IronFeudal
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
			using (ServiceHost host = new ServiceHost(typeof(FeudalPeasantService)))
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