using GenomeNext.App;
using GenomeNext.App.Console;
using GenomeNext.Cloud.Compute;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GenomeNext.Billing;
using Amazon.EC2.Model;

namespace GenomeNext.App.Monitor
{
    public class ComputeCapacityMonitor : IConsoleApp
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Init()
        {
            InitServices();
        }

        public void Run()
        {
            Monitor();
        }

        private void InitServices()
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            LogUtil.Info(logger, GetType().Name + ".InitServices()...");
            System.Console.WriteLine("\n" + GetType().Name + ".InitServices()...");

            try
            {
                //do nothing for now
            }
            catch (Exception ex)
            {
                LogUtil.Warn(logger, ex.Message, ex);
                System.Console.WriteLine(ex.Message);
            }
        }

        private void Monitor()
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            LogUtil.Info(logger, GetType().Name + ".Monitor()...");
            System.Console.WriteLine("\n" + GetType().Name + ".Monitor()...");

            try
            {
                var awsComputeEnvironmentService = new AWSComputeEnvironmentService(new GNEntityModelContainer());
                awsComputeEnvironmentService.UpdateComputeCapacityInDB();
            }
            catch (Exception ex)
            {
                LogUtil.Warn(logger, ex.Message, ex);
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}
