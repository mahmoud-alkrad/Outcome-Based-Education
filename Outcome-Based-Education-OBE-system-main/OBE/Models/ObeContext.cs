using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OBE.Models;

public partial class ObeContext : DbContext
{
    public ObeContext()
    {
    }

    public ObeContext(DbContextOptions<ObeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CollegeGoal> CollegeGoals { get; set; }

    public virtual DbSet<CollegeGoalProgramGoal> CollegeGoalProgramGoals { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseGoal> CourseGoals { get; set; }

    public virtual DbSet<CourseInstructor> CourseInstructors { get; set; }

    public virtual DbSet<Program> Programs { get; set; }

    public virtual DbSet<ProgramGoal> ProgramGoals { get; set; }

    public virtual DbSet<ProgramGoalCourseGoal> ProgramGoalCourseGoals { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-BGRK7SD\\SQLEXPRESS;Database=OBE;User Id=sa;Password=12345;TrustServerCertificate=True;Persist Security Info=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CollegeGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__college___3213E83F492B08D5");

            entity.ToTable("college_goals");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.DescriptionAr).HasColumnName("description_ar");
            entity.Property(e => e.DescriptionEn).HasColumnName("description_en");
            entity.Property(e => e.TitleAr)
                .HasMaxLength(255)
                .HasColumnName("title_ar");
            entity.Property(e => e.TitleEn)
                .HasMaxLength(255)
                .HasColumnName("title_en");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<CollegeGoalProgramGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__college___3213E83F94357F8C");

            entity.ToTable("college_goal_program_goal");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CollegeGoalId).HasColumnName("college_goal_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.ProgramGoalId).HasColumnName("program_goal_id");

            entity.HasOne(d => d.CollegeGoal).WithMany(p => p.CollegeGoalProgramGoals)
                .HasForeignKey(d => d.CollegeGoalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__college_g__colle__5812160E");

            entity.HasOne(d => d.ProgramGoal).WithMany(p => p.CollegeGoalProgramGoals)
                .HasForeignKey(d => d.ProgramGoalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__college_g__progr__59063A47");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__courses__3213E83FD13C0853");

            entity.ToTable("courses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Mark)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("mark");
            entity.Property(e => e.NameAr)
                .HasMaxLength(255)
                .HasColumnName("name_ar");
            entity.Property(e => e.NameEn)
                .HasMaxLength(255)
                .HasColumnName("name_en");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Program).WithMany(p => p.Courses)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__courses__program__4F7CD00D");
        });

        modelBuilder.Entity<CourseGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__course_g__3213E83FDB9C7584");

            entity.ToTable("course_goals");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.DescriptionAr).HasColumnName("description_ar");
            entity.Property(e => e.DescriptionEn).HasColumnName("description_en");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseGoals)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__course_go__cours__5441852A");
        });

        modelBuilder.Entity<CourseInstructor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__course_i__3213E83FA3D86B8C");

            entity.ToTable("course_instructors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            entity.Property(e => e.Semester)
                .HasMaxLength(50)
                .HasColumnName("semester");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseInstructors)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__course_in__cours__628FA481");

            entity.HasOne(d => d.Instructor).WithMany(p => p.CourseInstructors)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__course_in__instr__6383C8BA");
        });

        modelBuilder.Entity<Program>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__programs__3213E83F7C32F630");

            entity.ToTable("programs");

            entity.HasIndex(e => e.Code, "UQ__programs__357D4CF9C9C8BE4E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.NameAr)
                .HasMaxLength(255)
                .HasColumnName("name_ar");
            entity.Property(e => e.NameEn)
                .HasMaxLength(255)
                .HasColumnName("name_en");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<ProgramGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__program___3213E83F20BAA779");

            entity.ToTable("program_goals");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.DescriptionAr).HasColumnName("description_ar");
            entity.Property(e => e.DescriptionEn).HasColumnName("description_en");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");
            entity.Property(e => e.TitleAr)
                .HasMaxLength(255)
                .HasColumnName("title_ar");
            entity.Property(e => e.TitleEn)
                .HasMaxLength(255)
                .HasColumnName("title_en");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Program).WithMany(p => p.ProgramGoals)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__program_g__progr__4AB81AF0");
        });

        modelBuilder.Entity<ProgramGoalCourseGoal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__program___3213E83F734E1A3D");

            entity.ToTable("program_goal_course_goal");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CorrelationLevel)
                .HasMaxLength(10)
                .HasColumnName("correlation_level");
            entity.Property(e => e.CourseGoalId).HasColumnName("course_goal_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.ProgramGoalId).HasColumnName("program_goal_id");

            entity.HasOne(d => d.CourseGoal).WithMany(p => p.ProgramGoalCourseGoals)
                .HasForeignKey(d => d.CourseGoalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__program_g__cours__5EBF139D");

            entity.HasOne(d => d.ProgramGoal).WithMany(p => p.ProgramGoalCourseGoals)
                .HasForeignKey(d => d.ProgramGoalId)
                .HasConstraintName("FK__program_g__progr__5DCAEF64");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F3BABA6E7");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E61649D689533").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.NameAr)
                .HasMaxLength(255)
                .HasColumnName("name_ar");
            entity.Property(e => e.NameEn)
                .HasMaxLength(255)
                .HasColumnName("name_en");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("Instructor")
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Program).WithMany(p => p.Users)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__users__program_i__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
