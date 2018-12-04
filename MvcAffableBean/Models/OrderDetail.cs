using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcAffableBean.Models
{
    public class OrderDetail :Common
    {
        [Key]
        public int Id { get; set; }
        // [Required]
        
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        // [Required]
        [Display(Name = "User Id"), MaxLength(50)]
        public string UserId { get; set; }

        //[Required]
        [Display(Name = "Address"), MaxLength(500), DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Address { get; set; }
        [Display(Name = "Cooking Instruction"), MaxLength(500), DisplayFormat(ConvertEmptyStringToNull = true)]
        public string CookingInstruction { get; set; }

        [Display(Name = "Delivery Status"),  DisplayFormat(ConvertEmptyStringToNull = false)]
        public int ? DeliveryStatus { get; set; }

        [Display(Name = "Delivery Note"), MaxLength(500), DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DeliveryNote { get; set; }

        [Display(Name = "Amount")]
        public decimal ? Amount { get; set; }








    }
}