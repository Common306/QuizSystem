using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuizSystemApi.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Quiz> Quizzes { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<TakeAnswer> TakeAnswers { get; set; } = null!;
        public virtual DbSet<TakeQuiz> TakeQuizzes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var conf = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                optionsBuilder.UseSqlServer(conf.GetConnectionString("DbConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.Content).HasMaxLength(500);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_Answer_Question");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Content).HasMaxLength(500);

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("FK_Question_Quiz");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.ToTable("Quiz");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.EndAt).HasColumnType("datetime");

                entity.Property(e => e.QuizCode).HasMaxLength(50);

                entity.Property(e => e.StartAt).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK_Quiz_User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<TakeAnswer>(entity =>
            {
                entity.HasKey(e => new { e.TakeQuizId, e.AnswerId });

                entity.ToTable("TakeAnswer");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.TakeAnswers)
                    .HasForeignKey(d => d.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TakeAnswer_Answer");

                entity.HasOne(d => d.TakeQuiz)
                    .WithMany(p => p.TakeAnswers)
                    .HasForeignKey(d => d.TakeQuizId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TakeAnswer_TakeQuiz");
            });

            modelBuilder.Entity<TakeQuiz>(entity =>
            {
                entity.ToTable("TakeQuiz");

                entity.Property(e => e.EndAt).HasColumnType("datetime");

                entity.Property(e => e.StartAt).HasColumnType("datetime");

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.TakeQuizzes)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("FK_TakeQuiz_Quiz");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TakeQuizzes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_TakeQuiz_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.Property(e => e.Password).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
