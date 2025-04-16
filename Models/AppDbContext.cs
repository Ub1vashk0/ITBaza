using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ITBaza.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Access> Accesses { get; set; }

    public virtual DbSet<AccessOperation> AccessOperations { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<MobileOperator> MobileOperators { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }

    public virtual DbSet<PhoneNumberOperation> PhoneNumberOperations { get; set; }

    public virtual DbSet<Placement> Placements { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<ResourceRole> ResourceRoles { get; set; }

    public virtual DbSet<ResourceType> ResourceTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SystemUser> SystemUsers { get; set; }

    public virtual DbSet<Vacation> Vacations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=195.191.126.26,64121;Initial Catalog=AccessManagementSystem;Persist Security Info=True;User ID=superadmin;Password=123;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Access>(entity =>
        {
            entity.ToTable("Access");

            entity.Property(e => e.Login).HasMaxLength(100);

            entity.HasOne(d => d.Resource).WithMany(p => p.Accesses)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Access_Resource");

            entity.HasOne(d => d.ResourceRole).WithMany(p => p.Accesses)
                .HasForeignKey(d => d.ResourceRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Access_ResourceRole");
        });

        modelBuilder.Entity<AccessOperation>(entity =>
        {
            entity.ToTable("AccessOperation");

            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Access).WithMany(p => p.AccessOperations)
                .HasForeignKey(d => d.AccessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccessOperation_Access");

            entity.HasOne(d => d.Executor).WithMany(p => p.AccessOperations)
                .HasForeignKey(d => d.ExecutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccessOperation_SystemUser");

            entity.HasOne(d => d.Person).WithMany(p => p.AccessOperations)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccessOperation_Person");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.ToTable("Division");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.Divisions)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Division_Department");
        });

        modelBuilder.Entity<MobileOperator>(entity =>
        {
            entity.ToTable("MobileOperator");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Corporation).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.IdCountryNavigation).WithMany(p => p.MobileOperators)
                .HasForeignKey(d => d.IdCountry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MobileOperator_Country");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.PersonalPhone).HasMaxLength(20);
            entity.Property(e => e.WorkType).HasMaxLength(50);

            entity.HasOne(d => d.Placement).WithMany(p => p.People)
                .HasForeignKey(d => d.PlacementId)
                .HasConstraintName("FK_Person_Placement");

            entity.HasOne(d => d.Vacation).WithMany(p => p.People)
                .HasForeignKey(d => d.VacationId)
                .HasConstraintName("FK_Person_Vacation");
        });

        modelBuilder.Entity<PhoneNumber>(entity =>
        {
            entity.ToTable("PhoneNumber");

            entity.Property(e => e.Number).HasMaxLength(20);
            entity.Property(e => e.Pin1).HasMaxLength(10);
            entity.Property(e => e.Pin2).HasMaxLength(10);
            entity.Property(e => e.Puk1).HasMaxLength(20);
            entity.Property(e => e.Puk2).HasMaxLength(20);
            entity.Property(e => e.SimSerialNumber).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Operator).WithMany(p => p.PhoneNumbers)
                .HasForeignKey(d => d.OperatorId)
                .HasConstraintName("FK_PhoneNumber_MobileOperator");
        });

        modelBuilder.Entity<PhoneNumberOperation>(entity =>
        {
            entity.ToTable("PhoneNumberOperation");

            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.ActionDate).HasColumnType("datetime");

            entity.HasOne(d => d.Executor).WithMany(p => p.PhoneNumberOperations)
                .HasForeignKey(d => d.ExecutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhoneNumberOperation_Role");

            entity.HasOne(d => d.Person).WithMany(p => p.PhoneNumberOperations)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhoneNumberOperation_Person");

            entity.HasOne(d => d.PhoneNumber).WithMany(p => p.PhoneNumberOperations)
                .HasForeignKey(d => d.PhoneNumberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhoneNumberOperation_PhoneNumber");
        });

        modelBuilder.Entity<Placement>(entity =>
        {
            entity.ToTable("Placement");

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Office).HasMaxLength(100);

            entity.HasOne(d => d.Country).WithMany(p => p.Placements)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Placement_Country");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.ToTable("Resource");

            entity.Property(e => e.AltLocation).HasMaxLength(200);
            entity.Property(e => e.MainLocation).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.ResourceType).WithMany(p => p.Resources)
                .HasForeignKey(d => d.ResourceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resource_ResourceType");

            entity.HasOne(d => d.ResponsiblePerson).WithMany(p => p.Resources)
                .HasForeignKey(d => d.ResponsiblePersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resource_Person");
        });

        modelBuilder.Entity<ResourceRole>(entity =>
        {
            entity.ToTable("ResourceRole");

            entity.Property(e => e.RoleName).HasMaxLength(100);

            entity.HasOne(d => d.Resource).WithMany(p => p.ResourceRoles)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResourceRole_Resource");
        });

        modelBuilder.Entity<ResourceType>(entity =>
        {
            entity.ToTable("ResourceType");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<SystemUser>(entity =>
        {
            entity.ToTable("SystemUser");

            entity.HasIndex(e => e.Login, "UQ__SystemUs__5E55825BAEE17D73").IsUnique();

            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.LastPasswordChangeDate).HasColumnType("datetime");
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.SystemUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SystemUser_Role");
        });

        modelBuilder.Entity<Vacation>(entity =>
        {
            entity.ToTable("Vacation");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.Vacations)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vacation_Department");

            entity.HasOne(d => d.Division).WithMany(p => p.Vacations)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vacation_Division");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
