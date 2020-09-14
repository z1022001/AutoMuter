using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoMuter
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Mutex mutex = new Mutex(false, "SingletonWinAppMutexAutoMuter", out bool createdNew);
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new AutoMuter());
                }
                mutex.Dispose();
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText("./AutoMuter.log", e.ToString());
                Console.WriteLine(e);
            }
        }
    }
}
