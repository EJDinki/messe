namespace DiscoveryCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        Value = c.String(),
                        Submission_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Submissions", t => t.Submission_Id)
                .Index(t => t.QuestionId)
                .Index(t => t.Submission_Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Type = c.Int(nullable: false),
                        SurveyID = c.Int(nullable: false),
                        Choices = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Surveys", t => t.SurveyID, cascadeDelete: true)
                .Index(t => t.SurveyID);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Submissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.SurveyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Submissions", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.Answers", "Submission_Id", "dbo.Submissions");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Questions", "SurveyID", "dbo.Surveys");
            DropIndex("dbo.Submissions", new[] { "SurveyId" });
            DropIndex("dbo.Questions", new[] { "SurveyID" });
            DropIndex("dbo.Answers", new[] { "Submission_Id" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropTable("dbo.Submissions");
            DropTable("dbo.Surveys");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
