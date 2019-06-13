using Encryption.Constant;
using Encryption.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Encryption.Scheduler
{
    public class EncryptionJob : IJob
    {
        private readonly IEncryptionRepository encryptionRepository;
        public EncryptionJob(IEncryptionRepository encryptionRepository)
        {
            this.encryptionRepository = encryptionRepository;
        }
        public Task Execute(IJobExecutionContext context)
        {
            encryptionRepository.ExecuteEncrpytion();
            return Task.FromResult(0);
        }
    }
}
