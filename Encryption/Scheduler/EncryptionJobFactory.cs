using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Encryption.Scheduler
{
    public class EncryptionJobFactory : SimpleJobFactory
    {
        IServiceProvider serviceProvider;
        public EncryptionJobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                return (IJob)this.serviceProvider.GetService(bundle.JobDetail.JobType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
