using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnbordingPlatform.Entities;

public partial class VlasovaAaКурсовая1Context : DbContext
{
    public VlasovaAaКурсовая1Context()
    {
    }

    public VlasovaAaКурсовая1Context(DbContextOptions<VlasovaAaКурсовая1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountStatus> AccountStatuses { get; set; }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseStatus> CourseStatuses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<EnrollmentStatus> EnrollmentStatuses { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=dbsrv\\AG2025;Initial Catalog='VlasovaAA курсовая1';Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__46A52ED5A30AFC2F");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Username, "UQ__Account__F3DBC572F25F6CBA").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("account_ID");
            entity.Property(e => e.AccountDescription)
                .HasMaxLength(250)
                .HasColumnName("account_description");
            entity.Property(e => e.AccountStatusFk).HasColumnName("account_status_FK");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.RoleIdFk).HasColumnName("role_ID_FK");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.AccountStatusFkNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.AccountStatusFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__account__46E78A0C");

            entity.HasOne(d => d.RoleIdFkNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__role_ID__45F365D3");
        });

        modelBuilder.Entity<AccountStatus>(entity =>
        {
            entity.HasKey(e => e.AccountStatusId).HasName("PK__Account___569F74EDCB82CDF9");

            entity.ToTable("Account_status");

            entity.HasIndex(e => e.StatusName, "UQ__Account___501B3753BBE172FE").IsUnique();

            entity.Property(e => e.AccountStatusId).HasColumnName("account_status_ID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__Answer__337147000F3B743F");

            entity.ToTable("Answer");

            entity.Property(e => e.AnswerId).HasColumnName("answer_ID");
            entity.Property(e => e.AnswerDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("answer_date");
            entity.Property(e => e.AnswerDocument)
                .HasMaxLength(1000)
                .HasColumnName("answer_document");
            entity.Property(e => e.AnswerText).HasColumnName("answer_text");
            entity.Property(e => e.EmployeeIdFk).HasColumnName("employee_ID_FK");
            entity.Property(e => e.FeedbackIdFk).HasColumnName("feedback_ID_FK");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.ReviewIdFk).HasColumnName("review_ID_FK");
            entity.Property(e => e.TaskIdFk).HasColumnName("task_ID_FK");

            entity.HasOne(d => d.EmployeeIdFkNavigation).WithMany(p => p.Answers)
                .HasForeignKey(d => d.EmployeeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Answer__employee__6B24EA82");

            entity.HasOne(d => d.FeedbackIdFkNavigation).WithMany(p => p.Answers)
                .HasForeignKey(d => d.FeedbackIdFk)
                .HasConstraintName("FK__Answer__feedback__6C190EBB");

            entity.HasOne(d => d.ReviewIdFkNavigation).WithMany(p => p.Answers)
                .HasForeignKey(d => d.ReviewIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Answer__review_I__6D0D32F4");

            entity.HasOne(d => d.TaskIdFkNavigation).WithMany(p => p.Answers)
                .HasForeignKey(d => d.TaskIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Answer__task_ID___6A30C649");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__8F1FF3A68FB2EC39");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("course_ID");
            entity.Property(e => e.CourseDescription).HasColumnName("course_description");
            entity.Property(e => e.CourseStatusIdFk).HasColumnName("course_status_ID_FK");
            entity.Property(e => e.CourseTitle)
                .HasMaxLength(200)
                .HasColumnName("course_title");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("creation_date");
            entity.Property(e => e.MentorIdFk).HasColumnName("mentor_ID_FK");

            entity.HasOne(d => d.CourseStatusIdFkNavigation).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CourseStatusIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Course__course_s__5165187F");

            entity.HasOne(d => d.MentorIdFkNavigation).WithMany(p => p.Courses)
                .HasForeignKey(d => d.MentorIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Course__mentor_I__5070F446");
        });

        modelBuilder.Entity<CourseStatus>(entity =>
        {
            entity.HasKey(e => e.CourseStatusId).HasName("PK__Course_s__6ED53E6E87E6B2E0");

            entity.ToTable("Course_status");

            entity.Property(e => e.CourseStatusId).HasColumnName("course_status_ID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__6D29A6D256B61E2F");

            entity.ToTable("Enrollment");

            entity.Property(e => e.EnrollmentId).HasColumnName("enrollment_ID");
            entity.Property(e => e.AccountIdFk).HasColumnName("account_ID_FK");
            entity.Property(e => e.CourseIdFk).HasColumnName("course_ID_FK");
            entity.Property(e => e.EnrollmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("enrollment_date");
            entity.Property(e => e.EnrollmentStatusIdFk).HasColumnName("enrollment_status_ID_FK");

            entity.HasOne(d => d.AccountIdFkNavigation).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.AccountIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Enrollmen__accou__5535A963");

            entity.HasOne(d => d.CourseIdFkNavigation).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Enrollmen__cours__5629CD9C");

            entity.HasOne(d => d.EnrollmentStatusIdFkNavigation).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.EnrollmentStatusIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Enrollmen__enrol__571DF1D5");
        });

        modelBuilder.Entity<EnrollmentStatus>(entity =>
        {
            entity.HasKey(e => e.EnrollmentStatusId).HasName("PK__Enrollme__958D9C5BC7F719C0");

            entity.ToTable("Enrollment_status");

            entity.Property(e => e.EnrollmentStatusId).HasColumnName("enrollment_status_ID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__7A6A2F945239985B");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("feedback_ID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_date");
            entity.Property(e => e.FeedbackText)
                .HasMaxLength(500)
                .HasDefaultValue("-")
                .HasColumnName("feedback_text");
            entity.Property(e => e.MentorIdFk).HasColumnName("mentor_ID_FK");

            entity.HasOne(d => d.MentorIdFkNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.MentorIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__mentor__6477ECF3");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__E530676221DA7CB3");

            entity.ToTable("Permission");

            entity.Property(e => e.PermissionId).HasColumnName("permission_ID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(100)
                .HasColumnName("permission_name");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__608B39D8489C47C0");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("review_ID");
            entity.Property(e => e.ReviewName)
                .HasMaxLength(50)
                .HasColumnName("review_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760F9984354AED3F");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("role_ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.RolePermissionId).HasName("PK__Role_per__B1E95ED875BDD064");

            entity.ToTable("Role_permission");

            entity.Property(e => e.RolePermissionId).HasColumnName("role_permission_ID");
            entity.Property(e => e.PermissionIdFk).HasColumnName("permission_ID_FK");
            entity.Property(e => e.RoleIdFk).HasColumnName("role_ID_FK");

            entity.HasOne(d => d.PermissionIdFkNavigation).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Role_perm__permi__3C69FB99");

            entity.HasOne(d => d.RoleIdFkNavigation).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Role_perm__role___3B75D760");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Task__049318B510E9E2C3");

            entity.ToTable("Task");

            entity.Property(e => e.TaskId).HasColumnName("task_ID");
            entity.Property(e => e.CourseIdFk).HasColumnName("course_ID_FK");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_date");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.TaskDescription).HasColumnName("task_description");
            entity.Property(e => e.TaskDocument)
                .HasMaxLength(1000)
                .HasColumnName("task_document");
            entity.Property(e => e.TaskTitle)
                .HasMaxLength(100)
                .HasColumnName("task_title");

            entity.HasOne(d => d.CourseIdFkNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CourseIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Task__course_ID___5DCAEF64");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
