using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace consoleLabb4Db
{
    public class GameContext : DbContext
    {

        public DbSet<Question> questions { get; set; }
        public DbSet<Answer> answers { get; set; }

        private string connectionURI = "https://localhost:8081";
        private string connectionKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private string targetDatabase = "Labb4DbConsoleApp";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseCosmos(connectionURI, connectionKey, targetDatabase);
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasMany(a => a.Answers)
                .WithOne(q => q.Question)
                .HasForeignKey(q => q.QuestionId);

            modelBuilder.Entity<Question>()
                .ToContainer("Questions");
            modelBuilder.Entity<Answer>()
                .ToContainer("Answers");
        }

    }
}
