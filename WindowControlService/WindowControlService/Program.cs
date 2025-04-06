using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace WindowControlService
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        static void Main()
        {

            //while (Debugger.IsAttached == false)
            //{

            //    System.Threading.Thread.Sleep(100);
            //}
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Controller()
            };
            ServiceBase.Run(ServicesToRun);
        }


    }
}
