using Microsoft.VisualStudio.TestTools.UnitTesting;
using VIR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIR.Tests
{
    [TestClass()]
    public class LoginFormTests
    {
        [TestMethod()]
        public void HomeClosedTest()
        {
            try
            {
                LoginForm l = new LoginForm();
                l.HomeClosed();
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod()]
        public void LoginTest()
        {
            LoginForm l = new LoginForm();
            Assert.IsNotNull(l);
        }
    }
}