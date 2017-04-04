namespace ScreeningAutomation.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ScreeningAutomationDbContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<ScreeningTest> ScreeningTest { get; set; }
        public DbSet<ScreeningTestPassedHistory> ScreeningTestPassedHistory { get; set; }
        public DbSet<ScreeningTestPassingActive> ScreeningTestPassingActive { get; set; }
        public DbSet<ScreeningTestPassingPlan> ScreeningTestPassingPlan { get; set; }

        public ScreeningAutomationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.FirstName).HasMaxLength(200);
                b.Property(x => x.LastName).HasMaxLength(200);
                b.Property(x => x.Alias).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<ScreeningTest>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).HasMaxLength(500).IsRequired();
                b.Property(x => x.ValidPeriod).IsRequired();
            });

            modelBuilder.Entity<ScreeningTestPassingActive>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.DatePass).IsRequired();
                b.HasOne(x => x.Employee)
                    .WithMany(x => x.ScreeningTestPassingActive)
                    .HasForeignKey(x => x.EmployeeId);
                b.HasOne(x => x.ScreeningTest)
                    .WithMany(x => x.ScreeningTestPassingActive)
                    .HasForeignKey(x => x.ScreeningTestId);
            });

            modelBuilder.Entity<ScreeningTestPassedHistory>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.DatePass).IsRequired();
                b.HasOne(x => x.Employee)
                    .WithMany(x => x.ScreeningTestPassedHistory)
                    .HasForeignKey(x => x.EmployeeId);
                b.HasOne(x => x.ScreeningTest)
                    .WithMany(x => x.ScreeningTestPassedHistory)
                    .HasForeignKey(x => x.ScreeningTestId);
            });

            modelBuilder.Entity<ScreeningTestPassingPlan>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Employee)
                .WithMany(x => x.ScreeningTestPassingPlan)
                .HasForeignKey(x => x.EmployeeId);
                b.HasOne(x => x.ScreeningTest)
                    .WithMany(x => x.ScreeningTestPassingPlan)
                    .HasForeignKey(x => x.ScreeningTestId);
            });
        }
    }
}
