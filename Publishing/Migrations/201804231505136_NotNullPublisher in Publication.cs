namespace Publishing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotNullPublisherinPublication : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tb_Publications", "PublisherRefId", "dbo.Tb_Publisher");
            DropIndex("dbo.Tb_Publications", new[] { "PublisherRefId" });
            AlterColumn("dbo.Tb_Publications", "PublisherRefId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tb_Publications", "PublisherRefId");
            AddForeignKey("dbo.Tb_Publications", "PublisherRefId", "dbo.Tb_Publisher", "PublisherId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tb_Publications", "PublisherRefId", "dbo.Tb_Publisher");
            DropIndex("dbo.Tb_Publications", new[] { "PublisherRefId" });
            AlterColumn("dbo.Tb_Publications", "PublisherRefId", c => c.Int());
            CreateIndex("dbo.Tb_Publications", "PublisherRefId");
            AddForeignKey("dbo.Tb_Publications", "PublisherRefId", "dbo.Tb_Publisher", "PublisherId");
        }
    }
}
