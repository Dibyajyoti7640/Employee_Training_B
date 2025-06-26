using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Employee_Training_B.Models;

public partial class EmpTdsContext : DbContext
{
    public EmpTdsContext()
    {
    }

    public EmpTdsContext(DbContextOptions<EmpTdsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AssessmentResponse> AssessmentResponses { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Calendar> Calendars { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<StudyMaterial> StudyMaterials { get; set; }

    public virtual DbSet<TrainingAssessment> TrainingAssessments { get; set; }

    public virtual DbSet<TrainingCertificate> TrainingCertificates { get; set; }

    public virtual DbSet<TrainingFeedback> TrainingFeedbacks { get; set; }

    public virtual DbSet<TrainingProgram> TrainingPrograms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= IN-JMDLV64;Database=Emp_TDS;User Id=sa; Password=sa;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<AssessmentResponse>(entity =>
        {
            entity.HasKey(e => e.ResponseId).HasName("PK__Assessme__1AAA646C47B9A261");

            entity.Property(e => e.ResponseText).HasColumnType("text");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Assessment).WithMany(p => p.AssessmentResponses)
                .HasForeignKey(d => d.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Assessmen__Asses__571DF1D5");

            entity.HasOne(d => d.User).WithMany(p => p.AssessmentResponses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Assessmen__UserI__5812160E");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendan__3213E83F99751CEA");

            entity.ToTable("Attendance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Email_id");
            entity.Property(e => e.EmpId).HasColumnName("Emp_Id");
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("fullName");
            entity.Property(e => e.MeetingDuration)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Meeting_Duration");
        });

        modelBuilder.Entity<Calendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Calendar__3213E83F7B493F08");

            entity.ToTable("Calendar");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("End_time");
            entity.Property(e => e.EndingDate).HasColumnName("ending_date");
            entity.Property(e => e.MeetingName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Meeting_Name");
            entity.Property(e => e.Organiser)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.StartingDate).HasColumnName("starting_date");
            entity.Property(e => e.TeamsLink).IsUnicode(false);
            entity.Property(e => e.Time).HasColumnType("datetime");
            entity.Property(e => e.Trainer)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Venue)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("venue");
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3214EC07AC230F71");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.Remarks).HasMaxLength(250);
            entity.Property(e => e.ReviewedOn).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.SubmittedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E12C290B0A9");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Notificat__UserI__5BE2A6F2");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF5881087798976");

            entity.Property(e => e.RegisteredAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Registered");

            entity.HasOne(d => d.Program).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Registrat__Progr__4316F928");

            entity.HasOne(d => d.User).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Registrat__UserI__4222D4EF");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48052D733480");

            entity.Property(e => e.ReportDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReportType).HasMaxLength(100);
            entity.Property(e => e.ReportUrl)
                .HasMaxLength(255)
                .HasColumnName("ReportURL");

            entity.HasOne(d => d.GeneratedByNavigation).WithMany(p => p.Reports)
                .HasForeignKey(d => d.GeneratedBy)
                .HasConstraintName("FK__Reports__Generat__60A75C0F");
        });

        modelBuilder.Entity<StudyMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StudyMat__3213E83F1A1EA80E");

            entity.ToTable("StudyMaterial");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.DocumentName)
                .IsUnicode(false)
                .HasColumnName("Document_Name");
        });

        modelBuilder.Entity<TrainingAssessment>(entity =>
        {
            entity.HasKey(e => e.AssessmentId).HasName("PK__Training__3D2BF81EF101923C");

            entity.Property(e => e.AnswerType).HasMaxLength(50);
            entity.Property(e => e.QuestionText).HasColumnType("text");

            entity.HasOne(d => d.Program).WithMany(p => p.TrainingAssessments)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TrainingA__Progr__534D60F1");
        });

        modelBuilder.Entity<TrainingCertificate>(entity =>
        {
            entity.HasKey(e => e.CertificateId).HasName("PK__Training__BBF8A7C10FCAF69D");

            entity.Property(e => e.CertificateUrl)
                .HasMaxLength(255)
                .HasColumnName("CertificateURL");
            entity.Property(e => e.IssuedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Program).WithMany(p => p.TrainingCertificates)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TrainingC__Progr__49C3F6B7");

            entity.HasOne(d => d.User).WithMany(p => p.TrainingCertificates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TrainingC__UserI__48CFD27E");
        });

        modelBuilder.Entity<TrainingFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Training__6A4BEDD6859365D0");

            entity.ToTable("TrainingFeedback");

            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Program).WithMany(p => p.TrainingFeedbacks)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TrainingF__Progr__4E88ABD4");

            entity.HasOne(d => d.User).WithMany(p => p.TrainingFeedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TrainingF__UserI__4D94879B");
        });

        modelBuilder.Entity<TrainingProgram>(entity =>
        {
            entity.HasKey(e => e.ProgramId).HasName("PK__Training__75256058AA08864C");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Mode).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Trainer).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TrainingProgramCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__TrainingP__Creat__3D5E1FD2");

            entity.HasOne(d => d.ManagedByNavigation).WithMany(p => p.TrainingProgramManagedByNavigations)
                .HasForeignKey(d => d.ManagedBy)
                .HasConstraintName("FK__TrainingP__Manag__3F466844");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CB8E6F9F3");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053470EA5E0F").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmpId).HasColumnName("Emp_ID");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
