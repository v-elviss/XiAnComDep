using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BYS.XiAnComDept.BussLogic
{
    [DisplayColumn("企业")]
    public class Company
    {
        public Company()
        {
            this.ComGuid = Guid.NewGuid();
        }

        [Key]
        [Editable(false,AllowInitialValue= false)]
        public Guid ComGuid { get; set; }

        [Display(Name="企业名称")]
        public string CompanyName { get; set; }

        [Display(Name="曾用名")]
        public string CompanyPreName { get; set; }
    }
}
