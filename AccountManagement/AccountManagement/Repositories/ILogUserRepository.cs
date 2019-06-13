using AccountManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.Repositories
{
    interface ILogUserRepository
    {
        void AddLog(TblLogUser log);
    }
}
