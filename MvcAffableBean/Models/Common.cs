using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcAffableBean.Models
{
    public class Common
    {
        [Display(Name = "Created By"), MaxLength(100), DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CreatedBy { get; set; }
        
        [Display(Name = "Created Date"),  DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime ? CreatedDate { get; set; }

        [Display(Name = "Modified By"), MaxLength(100), DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date"), DisplayFormat(ConvertEmptyStringToNull = false)]
        public DateTime ? ModifiedDate{ get; set; }
    }
}