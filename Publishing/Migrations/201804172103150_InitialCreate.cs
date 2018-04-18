namespace Publishing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tb_Publications",
                c => new
                    {
                        PublicationId = c.Int(nullable: false, identity: true),
                        PublicationName = c.String(nullable: false, maxLength: 100),
                        ISSN = c.String(nullable: false, maxLength: 20, unicode: false),
                        Genre = c.String(nullable: false, maxLength: 20),
                        Language = c.String(nullable: false, maxLength: 15),
                        NumberOfCopies = c.Int(nullable: false),
                        NumberOfPages = c.Int(nullable: false),
                        Format = c.String(nullable: false, maxLength: 15),
                        DownloadLink = c.String(nullable: false, unicode: false),
                        PublisherRefId = c.Int(),
                        Cover = c.Binary(),
                        PublicationDate = c.String(nullable: false, maxLength: 10, unicode: false),
                        AddingInDBDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PublicationId)
                .ForeignKey("dbo.Tb_Publisher", t => t.PublisherRefId)
                .Index(t => t.PublisherRefId);
            
            CreateTable(
                "dbo.Tb_Publisher",
                c => new
                    {
                        PublisherId = c.Int(nullable: false, identity: true),
                        PublisherName = c.String(nullable: false, maxLength: 50),
                        Addres = c.String(nullable: false, maxLength: 300),
                        Email = c.String(nullable: false, maxLength: 30, unicode: false),
                        AddingInDBDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PublisherId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tb_Publications", "PublisherRefId", "dbo.Tb_Publisher");
            DropIndex("dbo.Tb_Publications", new[] { "PublisherRefId" });
            DropTable("dbo.Tb_Publisher");
            DropTable("dbo.Tb_Publications");
        }
    }
}
