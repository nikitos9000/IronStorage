using System;
using System.Drawing;
using System.Windows.Forms;

namespace IronPeasant
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (new MainDispatcher())
            {
                Application.Run();
            }
        }
    }
}