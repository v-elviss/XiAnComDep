using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace BYS.XiAnComDept.BussLogic
{
    public class ComDeptContext : DbContext
    {
        public ComDeptContext():base()
        {}

        public DbSet<Company> Companies { get; set; }

        public DbSet<Fund> Funds { get; set; }

        public DbSet<FundType> FundTypes { get; set; }

        public DbSet<ProjectField> ProjectFields { get; set; }

        public void ComDeptContext_SmokeTest()
        {
            using (ComDeptContext context = new ComDeptContext())
            {
                Fund fund = new Fund()
                {
                    Company = new Company() { CompanyName = "TestCom", CompanyPreName = "TestCom1, TestCom2" },
                    Amount = 12.2M,
                    Comment = "Test",
                    DueAmount = 10,
                    FundStatus = "通过",
                    FundType = new FundType() { FundTypeName = "TestType" },
                    ProjectFields = new ProjectField[]{
                        new ProjectField(){FieldName = "出口资金", FieldValue = "12"},
                        new ProjectField(){FieldName = "进口金额", FieldValue = "52"}
                    },
                    Year = DateTime.Now
                };

                context.Funds.Add(fund);
                context.SaveChanges();
            }
        }
    }
}
