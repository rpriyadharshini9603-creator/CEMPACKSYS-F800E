Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports Excel = Microsoft.Office.Interop.Excel

Public Class Report
    Dim X
    Dim Label3Text As String

    Public Sub New(dt As DataTable, title2 As String, FTdate As String, Optional excelFL As Boolean = False, Optional pdfFL As Boolean = False)
        X = RestoreSettings(TITLE1, "Properties", "CompanyName")
        If X <> "" Then
            Label3Text = X
        Else
            Label3Text = "Company name"
        End If


        If excelFL Then
            ExportExcelInBackground(dt, title2, Label3Text, FTdate)
        ElseIf pdfFL Then
            ExportPdfInBackground(dt, title2, Label3Text, FTdate)
        End If
    End Sub
    Public Sub ExportPdfInBackground(dt As DataTable, title As String, title1 As String, FTdate As String)
        Task.Run(Sub()
                     pdf(dt, title, title1, FTdate)
                 End Sub)
    End Sub

    Public Sub ExportExcelInBackground(dt As DataTable, title As String, title1 As String, FTdate As String)
        Task.Run(Sub()
                     Excelsheet(dt, title, title1, FTdate)
                 End Sub)
    End Sub

    Private Sub Excelsheet(dt As DataTable, title As String, title1 As String, FTdate As String)
        Try
            Dim excelApp As New Excel.Application()
            Dim excelWorkbook = excelApp.Workbooks.Add()
            Dim excelSheet As Excel.Worksheet = CType(excelWorkbook.Sheets(1), Excel.Worksheet)

            excelSheet.Cells(1, 1) = title1
            excelSheet.Cells(2, 1) = title
            excelSheet.Cells(3, 2) = FTdate

            For col = 0 To dt.Columns.Count - 1
                excelSheet.Cells(5, col + 1) = dt.Columns(col).ColumnName
            Next

            For row = 0 To dt.Rows.Count - 1
                For col = 0 To dt.Columns.Count - 1
                    excelSheet.Cells(row + 6, col + 1) = dt.Rows(row)(col).ToString()
                Next
            Next

            Dim lastColLetter As String = GetExcelColumnName(dt.Columns.Count)
            With excelSheet.Range($"A1:{lastColLetter}1")
                .Font.Name = "Arial"
                .Font.Bold = True
                .Font.Size = 14
                .MergeCells = True
                .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            End With
            excelSheet.Columns.AutoFit()
            excelApp.Visible = True
        Catch ex As Exception
            MessageBox.Show("Excel export failed: " & ex.Message)
        End Try
    End Sub





    Private Function GetExcelColumnName(columnNumber As Integer) As String
        Dim dividend As Integer = columnNumber
        Dim columnName As String = String.Empty
        Dim modulo As Integer

        While dividend > 0
            modulo = (dividend - 1) Mod 26
            columnName = Chr(65 + modulo) & columnName
            dividend = (dividend - modulo) \ 26
        End While

        Return columnName
    End Function
    Public Function pdf(dt As DataTable, title As String, title1 As String, FTdate As String)
        Dim pdfDoc As New Document(PageSize.A4, 10, 10, 10, 10)

        Dim downloadsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        Dim downloadsFolder As String = Path.Combine(downloadsPath, "Downloads")
        If Not Directory.Exists(downloadsFolder) Then
            Directory.CreateDirectory(downloadsFolder)
        End If

        Dim savePath As String = Path.Combine(downloadsFolder, title.Replace(" ", "") & Date.Now.ToString("ddMMyyyyHHmmss") & ".pdf")

        Dim headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)
        Dim cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 8)

        Using fs As New FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None)
            PdfWriter.GetInstance(pdfDoc, fs)
            pdfDoc.Open()

            '  Add title
            Dim titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14)
            pdfDoc.Add(New Paragraph(title1, titleFont))
            pdfDoc.Add(New Paragraph(title, titleFont))
            pdfDoc.Add(New Paragraph(FTdate))
            pdfDoc.Add(New Chunk(Environment.NewLine))

            '   === Create table ===
            Dim pdfTable As New PdfPTable(dt.Columns.Count)
            pdfTable.WidthPercentage = 100



            '=== Add Header Cells ===
            '  For Each colName As String In header
            For Each col As DataColumn In dt.Columns
                Dim cell As New PdfPCell(New Phrase(col.ColumnName, headerFont))
                cell.BackgroundColor = BaseColor.LIGHT_GRAY
                cell.HorizontalAlignment = Element.ALIGN_CENTER
                cell.Padding = 4
                pdfTable.AddCell(cell)
            Next

            '   === Add Data Rows ===
            For Each row As DataRow In dt.Rows
                For Each col As DataColumn In dt.Columns
                    Dim value = row(col).ToString()
                    Dim cell As New PdfPCell(New Phrase(value, cellFont))
                    cell.HorizontalAlignment = Element.ALIGN_CENTER
                    cell.Padding = 3
                    pdfTable.AddCell(cell)
                Next
            Next

            pdfDoc.Add(pdfTable)
            pdfDoc.Close()
        End Using

        '    === Prompt to Open ===
        Dim result As DialogResult = MessageBox.Show("Do you want to download (open and save) the report?", "Download Report", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Process.Start(savePath)
        Else
            Dim tempPath As String = Path.Combine(Path.GetTempPath(), title & ".pdf")
            File.Copy(savePath, tempPath, True)
            Process.Start(tempPath)
            File.Delete(savePath)
        End If
        Return True
    End Function


End Class
