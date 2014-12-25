using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BYS.XiAnComDept.BussLogic
{
    public class ProjectField
    {
        public ProjectField()
        {
            this.FieldGuid = Guid.NewGuid();
        }

        [Key]
        public Guid FieldGuid { get; set; }

        public string FieldName { get; set; }

        public string FieldValue { get; set; }

        public Fund Fund { get; set; }
    }
}
