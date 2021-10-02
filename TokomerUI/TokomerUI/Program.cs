using DisableDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CerealPotter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("starting");
            
            
            if(args.Length==1)
            {
                try
                {
                    DeviceHelper.SetDeviceEnabled(new Guid("{4d36e978-e325-11ce-bfc1-08002be10318}"), args[0].ToString(), false);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show($"Failed to disable port {ex}");
                }
                try
                {
                    DeviceHelper.SetDeviceEnabled(new Guid("{4d36e978-e325-11ce-bfc1-08002be10318}"), args[0].ToString(), true);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show($"Failed to enable port {ex}");
                }
                
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            
        }
    }
}
