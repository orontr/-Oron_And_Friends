using System.Data.Entity;
using AppointmentScheduling.Models;

namespace AppointmentScheduling.DAL
{
    public class SecurityQuestionDAL : DbContext
    {
        public DbSet<SecurityQuestion> Questions { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SecurityQuestion>().ToTable("Question");
        }

    }
}