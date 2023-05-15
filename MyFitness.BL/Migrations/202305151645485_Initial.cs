﻿namespace MyFitness.BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CaloriesPerMinute = c.Double(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Exercises",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        Finish = c.DateTime(nullable: false),
                        Activity_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.Activity_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Activity_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateOfBirth = c.DateTime(nullable: false),
                        Weight = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        Age = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Gender_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Genders", t => t.Gender_Id)
                .Index(t => t.Gender_Id);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FoodIntakes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Moment = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Meals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Proteins = c.Double(nullable: false),
                        Fats = c.Double(nullable: false),
                        Carbohydrates = c.Double(nullable: false),
                        Calories = c.Double(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FoodIntakes", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Exercises", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Gender_Id", "dbo.Genders");
            DropForeignKey("dbo.Exercises", "Activity_Id", "dbo.Activities");
            DropIndex("dbo.FoodIntakes", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "Gender_Id" });
            DropIndex("dbo.Exercises", new[] { "User_Id" });
            DropIndex("dbo.Exercises", new[] { "Activity_Id" });
            DropTable("dbo.Meals");
            DropTable("dbo.FoodIntakes");
            DropTable("dbo.Genders");
            DropTable("dbo.Users");
            DropTable("dbo.Exercises");
            DropTable("dbo.Activities");
        }
    }
}
