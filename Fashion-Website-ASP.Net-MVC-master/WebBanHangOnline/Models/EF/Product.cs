﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Spatial;
namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Product")]
    public class Product:CommonAbstract
    {
        public Product()
        {
            this.ReviewProducts = new HashSet<ReviewProduct>();
            this.ProductColors = new HashSet<ProductColor>();
            this.ProductSizes = new HashSet<ProductSize>();
            this.ProductImage = new HashSet<ProductImage>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.ProductQuantities = new HashSet<ProductQuantity>();
            this.Wishlists = new HashSet<Wishlist>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        public string Alias { get; set; }
        [StringLength(50)]
        public string ProductCode { get; set; }
        public string Description { get; set; }
        [AllowHtml]
        public string Detail { get; set; }
        public string Image { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceSale { get; set; }
       public int Quantity { get; set; }
        public int ViewCount { get; set; }
      
        public bool IsHome { get; set; }
        public bool IsSale { get; set; }
        public bool IsFeature { get; set; }
        public bool IsHot { get; set; }
        public bool IsActive { get; set; }
        public int ProductCategoryID { get; set; }
      
        [StringLength(250)]
        public string SeoTitle { get; set; }
        [StringLength(500)]
        public string SeoDescription { get; set; }
        [StringLength(250)]
        public string SeoKeyWords { get; set; }
    
        public virtual ProductCategory ProductCategory { get; set; }
     
        public ICollection<ProductSize> ProductSizes { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<ProductImage> ProductImage { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ReviewProduct> ReviewProducts{ get; set; }
        public virtual ICollection<ProductQuantity> ProductQuantities { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}