﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ch16.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ChuHou1120Entities : DbContext
    {
        public ChuHou1120Entities()
            : base("name=ChuHou1120Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CustomerMember> CustomerMember { get; set; }
        public virtual DbSet<CustomerReport> CustomerReport { get; set; }
        public virtual DbSet<Delivery> Delivery { get; set; }
        public virtual DbSet<Like> Like { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuCategory> MenuCategory { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<Score> Score { get; set; }
        public virtual DbSet<StoreMember> StoreMember { get; set; }
        public virtual DbSet<StoreReport> StoreReport { get; set; }
        public virtual DbSet<StoreCategory> StoreCategory { get; set; }
        public virtual DbSet<MenuCate> MenuCate { get; set; }
        public virtual DbSet<smmu> smmu { get; set; }
        public virtual DbSet<DetailMenu> DetailMenu { get; set; }
        public virtual DbSet<OrderDel> OrderDel { get; set; }
        public virtual DbSet<OrderReport> OrderReport { get; set; }
    }
}