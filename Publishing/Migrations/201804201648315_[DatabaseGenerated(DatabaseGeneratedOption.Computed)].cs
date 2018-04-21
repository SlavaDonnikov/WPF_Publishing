namespace Publishing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseGeneratedDatabaseGeneratedOptionComputed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tb_Publications", "AddedOrModifiedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Tb_Publisher", "AddedOrModifiedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tb_Publisher", "AddedOrModifiedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Tb_Publications", "AddedOrModifiedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
