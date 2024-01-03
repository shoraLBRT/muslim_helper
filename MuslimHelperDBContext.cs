using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace muslim_helper;

public partial class MuslimHelperDBContext : DbContext
{
    public MuslimHelperDBContext()
    {
    }

    public MuslimHelperDBContext(DbContextOptions<MuslimHelperDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TaskTrackingTable> TaskTrackingTables { get; set; }

    public virtual DbSet<TaskTypesTable> TaskTypesTables { get; set; }

    public virtual DbSet<UsersTable> UsersTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            ConfigurationManager.ConnectionStrings["muslimhelperDB"].ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskTrackingTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__task_tra__3214EC0745CFBDFA");

            entity.ToTable("task_tracking_table");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CompletionCheck).HasColumnName("completion_check");
            entity.Property(e => e.CompletionDate).HasColumnName("completion_date");
            entity.Property(e => e.ObligationBool).HasColumnName("obligation_bool");
            entity.Property(e => e.TaskTypesId).HasColumnName("task_types_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.TaskTypes).WithMany(p => p.TaskTrackingTables)
                .HasForeignKey(d => d.TaskTypesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__task_trac__task___4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.TaskTrackingTables)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__task_trac__user___4D94879B");
        });

        modelBuilder.Entity<TaskTypesTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__task_typ__3214EC07B7FC9E4F");

            entity.ToTable("task_types_table");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("name");
        });

        modelBuilder.Entity<UsersTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users_ta__3213E83FEDE429C5");

            entity.ToTable("users_table");

            entity.HasIndex(e => e.Chatid, "UQ__users_db__82628194DBE60E0E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Chatid).HasColumnName("chatid");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.NamazNotification).HasColumnName("namaz_notification");
            entity.Property(e => e.TaskTracking).HasColumnName("task_tracking");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
