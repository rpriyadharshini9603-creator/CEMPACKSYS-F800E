Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports System.Linq
Imports Microsoft.Office.Interop.Excel

Public Class frmStatus
    Dim UpdScrFl As Boolean
    Private DataGrid As DataGridView
    Dim _Packer As New Packer
    Dim _spout As New SpoutController
    Private Sub frmStatus_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each Packer As Packer In Packers
            ComboBox1.Items.Add(Packer.DeviceId)
        Next
        ComboBox1.SelectedIndex = 0
        DataGridView1.Font = New System.Drawing.Font("Arial", 12, FontStyle.Regular)
        Dim colHeaders As String() = {"Spout", "0001", "0002", "0003", "0004", "0005", "0006", "0007", "0008"}
        Dim rowHeaders As String() = {"Com Status", "CodeNo", "Final", "Over", "Under",
         "Final Adjust Weight", "BagWt-1", "BagWt-2", "BagWt-3", "BagWt-4", "BagWt-5", "BagWt-6",
         "BagWt-7", "BagWt-8", "BagWt-9", "BagWt-10", "BagWt-11", "BagWt-12", "BagWt-13",
         "BagWt-14", "BagWt-15", "BagWt-16", "BagWt-17", "BagWt-18", "BagWt-19", "BagWt-20",
         "Maximum", "Minimum", "Average"} '"CPS", "SP2",
        ' Change the font of the column headers to Verdana, 10pt
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView1.Columns.Clear()
        For col As Integer = 0 To colHeaders.Length - 1
            DataGridView1.Columns.Add(colHeaders(col), colHeaders(col))
        Next
        For row As Integer = 0 To rowHeaders.Length - 1
            DataGridView1.Rows.Add(rowHeaders(row))
            DataGridView1.Rows(row).Cells(0).Style.BackColor = Color.LightGray
            DataGridView1.Rows(row).Cells(0).Style.ForeColor = Color.Black
            DataGridView1.Rows(row).Cells(0).Style.Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
        Next
        Timer1.Enabled = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Refresh()
        'DataGridView1.Refresh()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim PackerId As Integer = ComboBox1.SelectedIndex + 1
        Dim row(9) As Object
        Dim IsConnected As New Dictionary(Of Integer, Boolean)
        Timer1.Enabled = False
        Timer1.Interval = 500
        _Packer = Packers.FirstOrDefault(Function(f) f.DeviceId = PackerId)
        DataGridView1.Rows(1).Cells(1).Value = "IsConnected"
        For Each Spout As SpoutController In _Packer.SpoutList
            If Spout.IsConnected = True Then
                DataGridView1.Rows(0).Cells(Spout.SpoutId).Value = "Connected"
                DataGridView1.Rows(0).Cells(Spout.SpoutId).Style.BackColor = Color.Green
            Else
                DataGridView1.Rows(0).Cells(Spout.SpoutId).Value = "Disconnected"
                DataGridView1.Rows(0).Cells(Spout.SpoutId).Style.BackColor = Color.Red
            End If
            DataGridView1.Rows(1).Cells(Spout.SpoutId).Value = Spout.CodeNo.ToString
            DataGridView1.Rows(2).Cells(Spout.SpoutId).Value = Spout.FinalWeight.ToString
            DataGridView1.Rows(3).Cells(Spout.SpoutId).Value = Spout.Over.ToString
            DataGridView1.Rows(4).Cells(Spout.SpoutId).Value = Spout.Under.ToString
            'DataGridView1.Rows(5).Cells(Spout.SpoutId).Value = Spout.CPS.ToString
            'DataGridView1.Rows(6).Cells(Spout.SpoutId).Value = Spout.SP2.ToString
            DataGridView1.Rows(5).Cells(Spout.SpoutId).Value = Spout.FinalAdjustmentWeight.ToString
            Dim bagsArray As Measurement() = Spout.Last20Bags.ToArray()
            Dim I = 5
            If bagsArray.Length > 0 Then
                For Each bag As Measurement In bagsArray
                    I += 1
                    DataGridView1.Rows(I).Cells(Spout.SpoutId).Value = bag.Weight
                Next
                I = 25
                Dim averageWeight As Double = bagsArray.Average(Function(bag) bag.Weight)
                Dim minWeight As Double = bagsArray.Min(Function(bag) bag.Weight)
                Dim maxWeight As Double = bagsArray.Max(Function(bag) bag.Weight)
                I += 1
                DataGridView1.Rows(I).Cells(Spout.SpoutId).Value = maxWeight.ToString("N2")
                DataGridView1.Rows(I).DefaultCellStyle.Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
                I += 1
                DataGridView1.Rows(I).Cells(Spout.SpoutId).Value = minWeight.ToString("N2")
                DataGridView1.Rows(I).DefaultCellStyle.Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
                I += 1
                DataGridView1.Rows(I).Cells(Spout.SpoutId).Value = averageWeight.ToString("N2")
                DataGridView1.Rows(I).DefaultCellStyle.Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
            End If
        Next
        Timer1.Enabled = True
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Timer1.Enabled = True
    End Sub
End Class






