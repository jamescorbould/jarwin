﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace jarwin.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class jarwinEntities : DbContext
    {
        public jarwinEntities()
            : base("name=jarwinEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<feed> feeds { get; set; }
        public virtual DbSet<feed_history> feed_history { get; set; }
        public virtual DbSet<feed_item> feed_item { get; set; }
        public virtual DbSet<feed_item_history> feed_item_history { get; set; }
    }
}
