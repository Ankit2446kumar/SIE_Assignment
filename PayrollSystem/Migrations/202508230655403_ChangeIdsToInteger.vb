Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class ChangeIdsToInteger
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Employees",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .Employee_Id = c.String(),
                        .Employee_Name = c.String(),
                        .Employee_Designation = c.String(),
                        .Basic_Salary = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .Leaves_Taken = c.Int(nullable := False),
                        .Working_Days_In_Month = c.Int(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
            CreateTable(
                "dbo.PayrollReports",
                Function(c) New With
                    {
                        .Id = c.Int(nullable := False, identity := True),
                        .EmpId = c.String(),
                        .Allowance = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .Gross_Salary = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .Deduction = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .NetPay = c.Decimal(nullable := False, precision := 18, scale := 2)
                    }) _
                .PrimaryKey(Function(t) t.Id)
            
        End Sub
        
        Public Overrides Sub Down()
            DropTable("dbo.PayrollReports")
            DropTable("dbo.Employees")
        End Sub
    End Class
End Namespace
