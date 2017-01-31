using GenomeNext.App;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenomeNext.App.Console
{
    public class ConsoleApp : IConsoleApp
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void DoMain(string[] args, Type appType)
        {
            System.Console.WriteLine("Initializing...");
            LogUtil.Info(logger, "Starting...");

            ConsoleApp app = (ConsoleApp)Activator.CreateInstance(appType);

            if (app.GetConsoleApps() != null && app.GetConsoleApps().Length != 0)
            {
                app.Init();
                app.Run();
            }
        }

        public virtual string[] GetConsoleApps()
        {
            return new string[0];
        }

        public virtual int GetPollInterval()
        {
            return 5;
        }

        public void Init()
        {
            foreach (var consoleApp in GetConsoleApps())
            {
                try
                {
                    var consoleAppObj = Activator.CreateInstance(Type.GetType(consoleApp));
                    ((IConsoleApp)consoleAppObj).Init();
                }
                catch (Exception ex)
                {
                    LogUtil.Warn(logger, ex.Message, ex);
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        public void Run()
        {
            while (1 == 1)
            {
                foreach (var consoleApp in GetConsoleApps())
                {
                    try
                    {
                        var consoleAppObj = Activator.CreateInstance(Type.GetType(consoleApp));
                        ((IConsoleApp)consoleAppObj).Run();
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Warn(logger, ex.Message, ex);
                        System.Console.WriteLine(ex.Message);
                    }
                }

                Thread.Sleep(GetPollInterval() * 1000);
            }
        }
    }
}