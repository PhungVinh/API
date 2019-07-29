using AccountManagement.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementUnitTest.Test
{
    [TestFixture]
    public class AccountManagementTest
    {
        private AccountController _ac;

        [SetUp]
        public void SetUp()
        {
            _ac = new AccountController();   
        }

        [Test]
        public void TestGetAll()
        {
           var obj =  _ac.GetUserById(14);
        }
    }
}
