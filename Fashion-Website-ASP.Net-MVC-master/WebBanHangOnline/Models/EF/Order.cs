﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Order")]
    public class Order:CommonAbstract
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required(ErrorMessage ="Tên không được để trống! ")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống! ")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Điện thoại không được để trống! ")]
        public string Address { get; set; }
        public string Email { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public int TypePayment { get; set; }
        public int Status { get; set; }
        [Required(ErrorMessage = "CustomerId không được để trống!")]
        [StringLength(128)]
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public ApplicationUser User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}