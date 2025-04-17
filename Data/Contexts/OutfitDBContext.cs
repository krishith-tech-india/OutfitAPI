﻿using System;
using System.Collections.Generic;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public partial class OutfitDBContext : DbContext
{
    public OutfitDBContext()
    {
    }

    public OutfitDBContext(DbContextOptions<OutfitDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<ImageType> ImageTypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductGroup> ProductGroups { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("address_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.AddressAddedByNavigations).HasConstraintName("fk_address_added_by");

            entity.HasOne(d => d.User).WithMany(p => p.AddressUsers).HasConstraintName("fk_address_user_id");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cart_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.CartAddedByNavigations).HasConstraintName("fk_cart_added_by");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.CartLastUpdatedByNavigations).HasConstraintName("fk_cart_last_updated_by");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts).HasConstraintName("fk_cart_product_id");

            entity.HasOne(d => d.User).WithMany(p => p.CartUsers).HasConstraintName("fk_cart_user_id");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("images_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.ImageAddedByNavigations).HasConstraintName("fk_images_added_by");

            entity.HasOne(d => d.ImageType).WithMany(p => p.Images).HasConstraintName("fk_images_image_type_id");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.ImageLastUpdatedByNavigations).HasConstraintName("fk_images_last_updated_by");

            entity.HasOne(d => d.Product).WithMany(p => p.Images).HasConstraintName("fk_images_product_id");
        });

        modelBuilder.Entity<ImageType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("image_type_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.ImageTypeAddedByNavigations).HasConstraintName("fk_image_type_added_by");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.ImageTypeLastUpdatedByNavigations).HasConstraintName("fk_image_type_last_updated_by");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.OrderAddedByNavigations).HasConstraintName("fk_order_added_by");

            entity.HasOne(d => d.Address).WithMany(p => p.Orders).HasConstraintName("fk_order_address_id");

            entity.HasOne(d => d.CurrentStatus).WithMany(p => p.Orders).HasConstraintName("fk_order_current_status_id");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.OrderLastUpdatedByNavigations).HasConstraintName("fk_order_last_updated_by");

            entity.HasOne(d => d.Product).WithMany(p => p.Orders).HasConstraintName("fk_order_product_id");

            entity.HasOne(d => d.User).WithMany(p => p.OrderUsers).HasConstraintName("fk_order_user_id");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_item_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.OrderItemAddedByNavigations).HasConstraintName("fk_order_item_added_by");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.OrderItemLastUpdatedByNavigations).HasConstraintName("fk_order_item_last_updated_by");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("fk_order_item_order_id");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems).HasConstraintName("fk_order_item_product_id");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_status_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.OrderStatusAddedByNavigations).HasConstraintName("fk_order_status_added_by");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.OrderStatusLastUpdatedByNavigations).HasConstraintName("fk_order_status_last_updated_by");
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_status_history_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.OrderStatusHistoryAddedByNavigations).HasConstraintName("fk_order_status_history_added_by");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.OrderStatusHistoryLastUpdatedByNavigations).HasConstraintName("fk_order_status_history_last_updated_by");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderStatusHistories).HasConstraintName("fk_order_status_history_order_id");

            entity.HasOne(d => d.Status).WithMany(p => p.OrderStatusHistories).HasConstraintName("fk_order_status_history_status_id");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.ProductAddedByNavigations).HasConstraintName("fk_product_added_by");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.ProductLastUpdatedByNavigations).HasConstraintName("fk_product_last_updated_by");

            entity.HasOne(d => d.ProductGroup).WithMany(p => p.Products).HasConstraintName("fk_product_product_group_id");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_category_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.ProductCategoryAddedByNavigations).HasConstraintName("fk_product_category_added_by");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.ProductCategoryLastUpdatedByNavigations).HasConstraintName("fk_product_category_last_updated_by");
        });

        modelBuilder.Entity<ProductGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_group_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.ProductGroupAddedByNavigations).HasConstraintName("fk_product_group_added_by");

            entity.HasOne(d => d.Category).WithMany(p => p.ProductGroups).HasConstraintName("fk_product_group_category_id");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.ProductGroupLastUpdatedByNavigations).HasConstraintName("fk_product_group_last_updated_by");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("review_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.ReviewAddedByNavigations).HasConstraintName("fk_review_added_by");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.ReviewLastUpdatedByNavigations).HasConstraintName("fk_review_last_updated_by");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews).HasConstraintName("fk_review_product_id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.InverseAddedByNavigation).HasConstraintName("fk_user_added_by");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.InverseLastUpdatedByNavigation).HasConstraintName("fk_user_last_updated_by");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_role_id");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("wishlist_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.WishlistAddedByNavigations).HasConstraintName("fk_wishlist_added_by");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.WishlistLastUpdatedByNavigations).HasConstraintName("fk_wishlist_last_updated_by");

            entity.HasOne(d => d.Product).WithMany(p => p.Wishlists).HasConstraintName("fk_wishlist_product_id");

            entity.HasOne(d => d.User).WithMany(p => p.WishlistUsers).HasConstraintName("fk_wishlist_user_id");
        });
        modelBuilder.HasSequence("address_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("cart_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("image_type_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("images_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("order_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("order_item_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("order_status_history_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("order_status_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("product_category_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("product_group_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("product_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("review_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("role_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("user_id_seq").HasMax(2147483647L);
        modelBuilder.HasSequence("wishlist_id_seq").HasMax(2147483647L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
