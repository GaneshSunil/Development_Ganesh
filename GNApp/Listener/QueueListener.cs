using GenomeNext.App;
using GenomeNext.App.Console;
using GenomeNext.Cloud.Messaging;
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

namespace GenomeNext.App.Listener
{
    public class QueueListener<T> : IConsoleApp
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Init()
        {
            //do nothing for now...
        }

        public void Run()
        {
            ListenForMessages();
        }

        public virtual IGNCloudMessageService<T> GetService()
        {
            return default(IGNCloudMessageService<T>);
        }

        private void ListenForMessages()
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            LogUtil.Info(logger, GetType().Name + ".ListenForMessages()...");
            System.Console.WriteLine("\n" + GetType().Name + ".ListenForMessages()...");

            LogUtil.Info(logger, "ConsumeMessages()...");
            System.Console.WriteLine("ConsumeMessages()...");
            System.Console.WriteLine(GetService().ToString());

            try
            {
                GetService().ConsumeMessages();
            }
            catch (Exception ex)
            {
                LogUtil.Warn(logger, ex.Message, ex);
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}
