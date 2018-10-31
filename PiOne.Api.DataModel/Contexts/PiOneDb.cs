namespace PiOne.Api.DataModel.Context
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using PiOneEntities;

    public partial class PiOneDb : DbContext
    {
        public PiOneDb()
            : base("name=PiOneDb")
        {
        }

        public virtual DbSet<Award> Awards { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<NotificationSetting> NotificationSettings { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<MemberLocation> MemberLocations { get; set; }
        public virtual DbSet<GeneralSetting> GeneralSettings { get; set; }
        public virtual DbSet<MemberNotification> MemberNotifications { get; set; }
        public virtual DbSet<ChattingTemplate> ChattingTemplates { get; set; }
        public virtual DbSet<MemberCard> MemberCards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Order>()
            .HasMany(e => e.Order1)
            .WithOptional(e => e.Order2)
            .HasForeignKey(e => e.ParentID);

            modelBuilder.Entity<Order>()
            .HasMany(e => e.OrderDetails)
            .WithRequired(e => e.Order)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderDetail>()
            .Property(e => e.Quantity)
            .HasPrecision(10, 2);

            modelBuilder.Entity<OrderDetail>()
            .HasMany(e => e.OrderDetail1)
            .WithOptional(e => e.OrderDetail2)
            .HasForeignKey(e => e.ParentID);

            modelBuilder.Entity<Member>()
            .HasMany(e => e.Awards)
            .WithOptional(e => e.Member)
            .HasForeignKey(e => e.MemberID);


            modelBuilder.Entity<Member>()
            .HasMany(e => e.Orders)
            .WithOptional(e => e.Member)
            .HasForeignKey(e => e.MemberID);

            modelBuilder.Entity<Store>()
            .HasMany(e => e.Orders)
            .WithRequired(e => e.Store)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
            .HasMany(e => e.Awards)
            .WithOptional(e => e.Order)
            .HasForeignKey(e => e.OrderID);

        }
    }
}
