namespace BYS.XiAnComDept.BussLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        ComGuid = c.Guid(nullable: false),
                        CompanyName = c.String(maxLength: 4000),
                        CompanyPreName = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.ComGuid);
            
            CreateTable(
                "dbo.Funds",
                c => new
                    {
                        Year = c.DateTime(nullable: false),
                        FundTypeGuid = c.Guid(nullable: false),
                        ComGuid = c.Guid(nullable: false),
                        FundStatus = c.String(maxLength: 4000),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DueAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comment = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => new { t.Year, t.FundTypeGuid, t.ComGuid })
                .ForeignKey("dbo.Companies", t => t.ComGuid, cascadeDelete: true)
                .ForeignKey("dbo.FundTypes", t => t.FundTypeGuid, cascadeDelete: true)
                .Index(t => t.FundTypeGuid)
                .Index(t => t.ComGuid);
            
            CreateTable(
                "dbo.FundTypes",
                c => new
                    {
                        FundTypeGuid = c.Guid(nullable: false),
                        FundTypeName = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.FundTypeGuid);
            
            CreateTable(
                "dbo.ProjectFields",
                c => new
                    {
                        FieldGuid = c.Guid(nullable: false),
                        FieldName = c.String(maxLength: 4000),
                        FieldValue = c.String(maxLength: 4000),
                        Fund_Year = c.DateTime(),
                        Fund_FundTypeGuid = c.Guid(),
                        Fund_ComGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.FieldGuid)
                .ForeignKey("dbo.Funds", t => new { t.Fund_Year, t.Fund_FundTypeGuid, t.Fund_ComGuid })
                .Index(t => new { t.Fund_Year, t.Fund_FundTypeGuid, t.Fund_ComGuid });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectFields", new[] { "Fund_Year", "Fund_FundTypeGuid", "Fund_ComGuid" }, "dbo.Funds");
            DropForeignKey("dbo.Funds", "FundTypeGuid", "dbo.FundTypes");
            DropForeignKey("dbo.Funds", "ComGuid", "dbo.Companies");
            DropIndex("dbo.ProjectFields", new[] { "Fund_Year", "Fund_FundTypeGuid", "Fund_ComGuid" });
            DropIndex("dbo.Funds", new[] { "ComGuid" });
            DropIndex("dbo.Funds", new[] { "FundTypeGuid" });
            DropTable("dbo.ProjectFields");
            DropTable("dbo.FundTypes");
            DropTable("dbo.Funds");
            DropTable("dbo.Companies");
        }
    }
}
