using System;
using System.Collections.Generic;
using CafeteriaAromas.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaAromas.Data;

public partial class CafeteriaDbContext : DbContext
{
    public CafeteriaDbContext()
    {
    }

    public CafeteriaDbContext(DbContextOptions<CafeteriaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryPurchase> InventoryPurchases { get; set; }

    public virtual DbSet<JobPosition> JobPositions { get; set; }

    public virtual DbSet<Machine> Machines { get; set; }

    public virtual DbSet<MachineMaintenance> MachineMaintenances { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeSupply> RecipeSupplies { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleDetail> SaleDetails { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Supply> Supplies { get; set; }

    public virtual DbSet<SupplyCategory> SupplyCategories { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07C97E3F8D");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.DocumentId, "UQ__Customer__1ABEEF0E91316B52").IsUnique();

            entity.Property(e => e.DocumentId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SecondLastName)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07D41C950A");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.DocumentId, "UQ__Employee__1ABEEF0EDE63C55A").IsUnique();

            entity.Property(e => e.DocumentId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SecondLastName)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.JobPosition).WithMany(p => p.Employees)
                .HasForeignKey(d => d.JobPositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_JobPosition");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC073FF428E7");

            entity.ToTable("Inventory");

            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Stock).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 4)");

            entity.HasOne(d => d.Supply).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.SupplyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Supply");
        });

        modelBuilder.Entity<InventoryPurchase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC078359DD8A");

            entity.ToTable("InventoryPurchase");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Inventory).WithMany(p => p.InventoryPurchases)
                .HasForeignKey(d => d.InventoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryPurchase_Inventory");

            entity.HasOne(d => d.Supplier).WithMany(p => p.InventoryPurchases)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryPurchase_Supplier");
        });

        modelBuilder.Entity<JobPosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobPosit__3214EC07C39E1818");

            entity.ToTable("JobPosition");

            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Machine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Machine__3214EC07E5A0DED0");

            entity.ToTable("Machine");

            entity.HasIndex(e => e.SerialNumber, "UQ__Machine__048A0008068F4B45").IsUnique();

            entity.Property(e => e.MachineFunction)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Machines)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Machine_Warehouse");
        });

        modelBuilder.Entity<MachineMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MachineM__3214EC071D3FBE1E");

            entity.ToTable("MachineMaintenance");

            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(2000)
                .IsUnicode(false);

            entity.HasOne(d => d.Machine).WithMany(p => p.MachineMaintenances)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MachineMaintenance_Machine");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC0729710F4F");

            entity.ToTable("Product");

            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.ProductionCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SellingPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCategory");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductC__3214EC07477EDA34");

            entity.ToTable("ProductCategory");

            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipe__3214EC07F4CB0516");

            entity.ToTable("Recipe");

            entity.Property(e => e.Instructions)
                .HasMaxLength(2000)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recipe_Product");
        });

        modelBuilder.Entity<RecipeSupply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RecipeSu__3214EC0795B483DB");

            entity.ToTable("RecipeSupply");

            entity.Property(e => e.IngredientQuantity).HasColumnType("decimal(10, 3)");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeSupplies)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecipeSupply_Recipe");

            entity.HasOne(d => d.Supply).WithMany(p => p.RecipeSupplies)
                .HasForeignKey(d => d.SupplyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecipeSupply_Supply");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sale__3214EC078DDC265D");

            entity.ToTable("Sale");

            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Sale_Customer");

            entity.HasOne(d => d.Employee).WithMany(p => p.Sales)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Employee");
        });

        modelBuilder.Entity<SaleDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SaleDeta__3214EC07A682AA09");

            entity.ToTable("SaleDetail");

            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetail_Product");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetail_Sale");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC07207BF793");

            entity.ToTable("Supplier");

            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Nit)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Supply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supply__3214EC077E1C71C3");

            entity.ToTable("Supply");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StoredQuantity).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.SupplyCategory).WithMany(p => p.Supplies)
                .HasForeignKey(d => d.SupplyCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supply_SupplyCategory");
        });

        modelBuilder.Entity<SupplyCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SupplyCa__3214EC07E09AC686");

            entity.ToTable("SupplyCategory");

            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warehous__3214EC073B0BF1BB");

            entity.ToTable("Warehouse");

            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
