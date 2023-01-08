using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeathbetraysJenkinsBuildWatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] _args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //string url = "http://127.0.0.1:8080";
            string url = "http://localhost:8080";
            if (_args.Length > 1)
            {
                url = _args[1];
            }

            Logger.Log(Logger.LogLevel.Spam, "Initialising build watcher using url {0}.", url);

            try
            {
                Application.Run(new DeathbetraysJenkinsBuildWatcher(url));
            }
            catch (Exception e)
            {
                Logger.Log(Logger.LogLevel.Error, "Unhandled exception: {0}", e.Message);
                Logger.Log(Logger.LogLevel.Error, "Stack trace:\n{0}", e.StackTrace);
            }
        }
    }
}
