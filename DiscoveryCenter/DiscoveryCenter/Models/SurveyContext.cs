namespace DiscoveryCenter.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using DiscoveryCenter.Migrations;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class SurveyContext : DbContext
    {
        // Your context has been configured to use a 'Survey' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'SurveyStuff.Models.Survey' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Survey' 
        // connection string in the application configuration file.
        public SurveyContext()
            : base("name=Survey")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Theme> Themes { get; set; }

        public virtual DbSet<Submission> Submissions { get; set; }

        public virtual DbSet<Answer> Answers { get; set; }

        public virtual DbSet<Question> Questions { get; set; }

        public virtual DbSet<Survey> Surveys { get; set; }

        public virtual DbSet<Exhibit> Exhibits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}