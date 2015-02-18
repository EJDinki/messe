namespace DiscoveryCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullFKs : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Questions", new[] { "SurveyID" });
            DropIndex("dbo.Submissions", new[] { "SurveyId" });
            AlterColumn("dbo.Questions", "SurveyID", c => c.Int());
            AlterColumn("dbo.Submissions", "SurveyId", c => c.Int());
            CreateIndex("dbo.Questions", "SurveyID");
            CreateIndex("dbo.Submissions", "SurveyId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Submissions", new[] { "SurveyId" });
            DropIndex("dbo.Questions", new[] { "SurveyID" });
            AlterColumn("dbo.Submissions", "SurveyId", c => c.Int(nullable: false));
            AlterColumn("dbo.Questions", "SurveyID", c => c.Int(nullable: false));
            CreateIndex("dbo.Submissions", "SurveyId");
            CreateIndex("dbo.Questions", "SurveyID");
        }
    }
}
