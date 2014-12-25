using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BYS.XiAnComDept.BussLogic
{
    public class Fund
    {
        [Key]
        [Column(Order = 1)]
        [Display(Name = "年度")]
        [DisplayFormat(DataFormatString ="yyyy")]
        public DateTime Year { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid ComGuid { get; set; }

        [ForeignKey("ComGuid")]
        public Company Company { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid FundTypeGuid { get; set; }

        [ForeignKey("FundTypeGuid")]
        public FundType FundType { get; set; }

        public string FundStatus { get; set; }

        public decimal Amount { get; set; }

        [Display(Name="尾款")]
        public decimal DueAmount { get; set; }

        public string Comment { get; set; }

        public ICollection<ProjectField> ProjectFields { get; set; }
    }

    public class FundType
    {
        public FundType() 
        {
            this.FundTypeGuid = Guid.NewGuid();
        }

        [Key]
        public Guid FundTypeGuid { get; set; }

        public string FundTypeName { get; set;}
    }
}
