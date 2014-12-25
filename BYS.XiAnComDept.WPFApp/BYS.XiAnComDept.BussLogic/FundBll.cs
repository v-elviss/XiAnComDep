using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BYS.AAPT.Common;
using LinqToExcel;
using LinqToExcel.Query;
using System.IO;
using System.Data.Entity;

namespace BYS.XiAnComDept.BussLogic
{
    public class FundBll
    {
        public Result ImportFunds(string excelFileName)
        {
            Result result = new Result(true);
            // file exists?
            if (!File.Exists(excelFileName))
            {
                return new Result(false, "Excel 文件不存在！");
            }

            try
            {
                ExcelQueryFactory excel = new ExcelQueryFactory(excelFileName);
                List<Row> rows = (from a in excel.Worksheet("资金") select a).ToList();
                if (rows.Count == 0)
                {
                    return new Result(false, "Excel 文件没有资金数据！");
                }

                int countCompaniesImprted = 0;
                int countFundImported = 0;
                int countFundUpdated = 0;
                int countFundTypesImported = 0;

                using (ComDeptContext db = new ComDeptContext())
                {
                    string[] companyNames = db.Companies.Select(x=>x.CompanyName.Trim()).ToArray();
                    IEnumerable<Company> companiesNeedAdd = rows.Where(x => !companyNames.Contains(x["企业名称"].ToString().Trim()))
                        .Select(y=>y["企业名称"].ToString().Trim())
                        .Distinct()
                        .Select(z => 
                            new Company() 
                        {
                            CompanyName = z
                        });

                    db.Companies.AddRange(companiesNeedAdd);
                    countCompaniesImprted = companiesNeedAdd.Count();

                    string[] fundTypeNames = db.FundTypes.Select(x => x.FundTypeName.Trim()).ToArray();
                    IEnumerable<FundType> fundTypesNeedAdd = rows.Where(x => !fundTypeNames.Contains(x["项目类别"].ToString().Trim()))
                        .Select(y=>y["项目类别"].ToString().Trim())
                        .Distinct()
                        .Select(z =>
                            new FundType() 
                            {
                                FundTypeName = z
                            });
                    db.FundTypes.AddRange(fundTypesNeedAdd);
                    countFundTypesImported = fundTypesNeedAdd.Count();
                    db.SaveChanges();

                    foreach (var row in rows)
                    {
                        string companyName = row["企业名称"].ToString().Trim();
                        Company company = db.Companies.SingleOrDefault(x => x.CompanyName.Trim() == companyName);
                        string fundTypeName = row["项目类别"].ToString().Trim();
                        FundType fundType = db.FundTypes.SingleOrDefault(x => x.FundTypeName.Trim() == fundTypeName);
                        Fund newFund = new Fund()
                        {
                            Amount = Convert.ToDecimal(row["申报金额"]),
                            Comment = row["备注"].ToString(),
                            ComGuid = company.ComGuid,
                            Company = company,
                            DueAmount = Convert.ToDecimal(row["尾款"]),
                            FundStatus = row["申请状态"].ToString(),
                            FundType = fundType,
                            FundTypeGuid = fundType.FundTypeGuid,
                            Year = Convert.ToDateTime(row["年度"].ToString() + "-1-1")
                        };
                        IEnumerable<string> projectColumns = row.ColumnNames.Where(x => x.StartsWith("project_"));
                        newFund.ProjectFields = new List<ProjectField>();
                        foreach (string colName in projectColumns)
                        {
                            newFund.ProjectFields.Add(new ProjectField()
                            {
                                FieldName = colName.Replace("project_", ""),
                                FieldValue = row[colName].ToString()
                            });
                        }

                        db.Funds.Attach(newFund);
                        if (db.Funds.SingleOrDefault(x => x.Year == newFund.Year && x.FundType.FundTypeGuid == newFund.FundType.FundTypeGuid && x.Company.ComGuid == newFund.Company.ComGuid) != null)
                        {
                            db.Entry(newFund).State = EntityState.Modified;
                            countFundUpdated++;
                        }
                        else
                        {
                            db.Entry(newFund).State = EntityState.Added;
                            countFundImported++;
                        }
                    }
                    db.SaveChanges();
                }
                result = new Result<Dictionary<string, string>>(true, new Dictionary<string, string>() 
                {
                    {"专项资金数据导入数据", countFundImported.ToString()},
                    {"专项资金数据更新数据", countFundUpdated.ToString()},
                    {"企业数据",countCompaniesImprted.ToString()},
                    {"项目类别", countFundTypesImported.ToString()}
                });
            }
            catch (Exception ex)
            {
                result = new Result<Exception>(false, ex, ex.Message);
            }

            return result;
        }

        public void FundBll_ImportFunds_SmokeTest()
        {
            ImportFunds(@"D:\my\aapt\Project\XianComDeptProj\Source\BYS.XiAnComDept.WPFApp\BYS.XiAnComDept.App\FundDemo.xlsx");
        }
    }
}
