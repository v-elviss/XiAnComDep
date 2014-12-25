using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using BYS.XiAnComDept.BussLogic;

namespace BYS.XiAnComDept.BussLogic.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Init()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<ComDeptContext>());
        }

        [TestMethod]
        public void Migration()
        {
            BYS.XiAnComDept.BussLogic.Migrations.t9 m = new Migrations.t9();
            m.Down();
            m.Up();
        }
    }
}
