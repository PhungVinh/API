using AccountManagement.Controllers;
using AccountManagement.Models;
using AccountManagement.Models.DataAccess;
using AccountManagement.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private AccountDA _ac;
        [SetUp]
        public void Setup()
        {
            _ac = new AccountDA();
        }

        [Test]
        public void TestLogin()
        {
            // Valid username, password
            string username0 = "haihm";
            string pw0 = "21232f297a57a5a743894a0e4a801fc3";

            Assert.AreEqual(6, _ac.GetUsersLogin(username0, pw0).Id);

            // Invalid info user
            string username1 = "haihm";
            string pw1 = "21232f297a57a5a743894a0e4a801fc4";
            Assert.AreEqual(null, _ac.GetUsersLogin(username1, pw1));
        }

        // [Test]
        public void TestAddUser()
        {
            int save = 0;
            UserAndOrgViewModel model = new UserAndOrgViewModel();

            TblOrganization org = new TblOrganization()
            {
                OrganizationId = 2,
                OrganizationCode = "MP",
                OrganizationParentCode = null,
                OrganizationName = "CT TNHH Minh Phúc",
                OrganizationAddress = "Mễ Trì, Nam Từ Liêm",
                OrganizationEmail = "mpt@mptelecom.com.vn",
                OrganizationPhone = null,
                OrganizationTaxCode = null,
                OrganizationRemark = null,
                OrganizationFrom = null,
                CreateBy = null,
                CreateDate = null,
                UpdateBy = null,
                UpdateDate = Convert.ToDateTime("2019-03-03 00:00:00.000"),
                IsActive = false,
                IsLock = null,
                IsDelete = false,
                OrganizationTo = null,
                OrganizationHomePage = null
            };
            // Add model
            model.tblUsers = GetUser(1);
            model.tblOrganization = org;

            Assert.AreEqual(save, 1);
        }

        //[Test]
        public void TestEditUser()
        {
            UserAndOrgViewModel model = new UserAndOrgViewModel();

            TblUsers user = new TblUsers
            {
                //Id = 
                UserName = "unitTestAdd",
                Password = "admin",
                FullName = "Hoang Manh Hai",
                Email = "haihm@mpsoftware.com.vn",
                PhoneNumber = "1234567890",
                CreateBy = "Admin",
                CreateDate = Convert.ToDateTime("2018-12-26T11 =53 =47.327"),
                UpdateBy = "",
                UpdateDate = Convert.ToDateTime("2019-03-29T11 =53 =00.263"),
                IsDelete = false,
                IsLock = false,
                Avatar = null,
                Address = null,
                LastLogin = null,
                Position = null,
                EmailConfirmed = true,
                Gender = 1,
                LoginFail = 0,
                HistoryPassword = "21232f297a57a5a743894a0e4a801fc3",
                DateUpdatePassword = Convert.ToDateTime("2019-04-01T00 =00 =00")
            };

            TblOrganization org = new TblOrganization()
            {
                OrganizationId = 2,
                OrganizationCode = "MP",
                OrganizationParentCode = null,
                OrganizationName = "CT TNHH Minh Phúc",
                OrganizationAddress = "Mễ Trì, Nam Từ Liêm",
                OrganizationEmail = "mpt@mptelecom.com.vn",
                OrganizationPhone = null,
                OrganizationTaxCode = null,
                OrganizationRemark = null,
                OrganizationFrom = null,
                CreateBy = null,
                CreateDate = null,
                UpdateBy = null,
                UpdateDate = Convert.ToDateTime("2019-03-03 00:00:00.000"),
                IsActive = false,
                IsLock = null,
                IsDelete = false,
                OrganizationTo = null,
                OrganizationHomePage = null
            };
            // Add model
            model.tblUsers = user;
            model.tblOrganization = org;

            //Assert.AreEqual(save, _ac.Compare(2, 1));
        }

        /// <summary>
        /// HaiHM
        /// Test Lock User
        /// </summary>
        [Test]
        public void TestLockUser()
        {
            // has userId
            bool lockUser0 = _ac.LockUser(GetUser(18));
            Assert.AreEqual(true, lockUser0);

            // Not has userId
            TblUsers userNotId = new TblUsers
            {
                //Id = 
                UserName = "unitTestAdd",
                Password = "admin",
                FullName = "Hoang Manh Hai",
                Email = "haihm@mpsoftware.com.vn",
                PhoneNumber = "1234567890",
                CreateBy = "Admin",
                CreateDate = Convert.ToDateTime("2018-12-26"),
                UpdateBy = "",
                UpdateDate = Convert.ToDateTime("2019-03-29"),
                IsDelete = false,
                IsLock = false,
                Avatar = null,
                Address = null,
                LastLogin = null,
                Position = null,
                EmailConfirmed = true,
                Gender = 1,
                LoginFail = 0,
                HistoryPassword = "21232f297a57a5a743894a0e4a801fc3",
                DateUpdatePassword = Convert.ToDateTime("2019-04-01")
            };

            bool lockUser1 = _ac.LockUser(userNotId);
            Assert.AreEqual(false, lockUser1, "Lock has not id user", null);
        }

        /// <summary>
        /// HaiHM
        /// Test Active User
        /// </summary>
        [Test]
        public void TestActiveUser()
        {
            // has userId
            bool activeUser0 = _ac.ActiveUser(GetUser(18));
            Assert.AreEqual(true, activeUser0);

            // Not has userId
            TblUsers userNotId = new TblUsers
            {
                //Id = 
                UserName = "unitTestAdd",
                Password = "admin",
                FullName = "Hoang Manh Hai",
                Email = "haihm@mpsoftware.com.vn",
                PhoneNumber = "1234567890",
                CreateBy = "Admin",
                CreateDate = Convert.ToDateTime("2018-12-26"),
                UpdateBy = "",
                UpdateDate = Convert.ToDateTime("2019-03-29"),
                IsDelete = false,
                IsLock = false,
                Avatar = null,
                Address = null,
                LastLogin = null,
                Position = null,
                EmailConfirmed = true,
                Gender = 1,
                LoginFail = 0,
                HistoryPassword = "21232f297a57a5a743894a0e4a801fc3",
                DateUpdatePassword = Convert.ToDateTime("2019-04-01")
            };

            bool activeUser1 = _ac.ActiveUser(userNotId);
            Assert.AreEqual(false, activeUser1, "Active has not id user", null);
        }

        /// <summary>
        /// HaiHm
        /// Test Delete User
        /// </summary>
        [Test]
        public void DeleteUser()
        {
            // has userId
            int deleteUser0 = _ac.DeleteUser(18);
            Assert.AreEqual(1, deleteUser0);

            // Not has userId
            TblUsers userNotId = new TblUsers
            {
                //Id = 
                UserName = "unitTestAdd",
                Password = "admin",
                FullName = "Hoang Manh Hai",
                Email = "haihm@mpsoftware.com.vn",
                PhoneNumber = "1234567890",
                CreateBy = "Admin",
                CreateDate = Convert.ToDateTime("2018-12-26"),
                UpdateBy = "",
                UpdateDate = Convert.ToDateTime("2019-03-29"),
                IsDelete = false,
                IsLock = false,
                Avatar = null,
                Address = null,
                LastLogin = null,
                Position = null,
                EmailConfirmed = true,
                Gender = 1,
                LoginFail = 0,
                HistoryPassword = "21232f297a57a5a743894a0e4a801fc3",
                DateUpdatePassword = Convert.ToDateTime("2019-04-01")
            };

            int deleteUser1 = _ac.DeleteUser(10000);
            Assert.AreEqual(0, deleteUser1, "Delete has not id user", null);
        }

        /// <summary>
        /// HaiHM
        /// Test Search User
        /// </summary>
        [Test]
        public void TestSearchUser()
        {
            String strFilter = ",,,1,15";
            object obj = _ac.SearchUser(strFilter);
            object objCheck = null;
        }

        /// <summary>
        /// List User Test
        /// </summary>
        /// <param name="idTest"></param>
        /// <returns></returns>
        TblUsers GetUser(int idTest)
        {
            List<TblUsers> lstUser = new List<TblUsers>();

            TblUsers userAdd = new TblUsers
            {
                //Id = 
                UserName = "unitTestAdd",
                Password = "admin",
                FullName = "Hoang Manh Hai",
                Email = "haihm@mpsoftware.com.vn",
                PhoneNumber = "1234567890",
                CreateBy = "Admin",
                CreateDate = Convert.ToDateTime("2018-12-26"),
                UpdateBy = "",
                UpdateDate = Convert.ToDateTime("2019-03-29"),
                IsDelete = false,
                IsLock = false,
                Avatar = null,
                Address = null,
                LastLogin = null,
                Position = null,
                EmailConfirmed = true,
                Gender = 1,
                LoginFail = 0,
                HistoryPassword = "21232f297a57a5a743894a0e4a801fc3",
                DateUpdatePassword = Convert.ToDateTime("2019-04-01")
            };

            TblUsers userLock = new TblUsers
            {
                Id = 18,
                UserName = "superCRM",
                Password = "admin",
                FullName = "Hoang Manh Hai",
                Email = "haihm@mpsoftware.com.vn",
                PhoneNumber = "1234567890",
                CreateBy = "Admin",
                CreateDate = Convert.ToDateTime("2018-12-26"),
                UpdateBy = "",
                UpdateDate = Convert.ToDateTime("2019-03-29"),
                IsDelete = false,
                IsLock = false,
                Avatar = null,
                Address = null,
                LastLogin = null,
                Position = null,
                EmailConfirmed = true,
                Gender = 1,
                LoginFail = 0,
                HistoryPassword = "21232f297a57a5a743894a0e4a801fc3",
                DateUpdatePassword = Convert.ToDateTime("2019-04-01")
            };

            lstUser.Add(userAdd);
            lstUser.Add(userLock);

            return lstUser.Find(u => u.Id == idTest);
        }

    }
}