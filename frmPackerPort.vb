Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO.Ports
Public Class frmPackerPort
    Private adapter As SqlDataAdapter
    Private dt As DataTable
    Private conn As SqlConnection
    Private builder As SqlCommandBuilder
    Dim ListofPorts() As String
    Private Sub FrmPacker_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AddHandler grid.DataError, AddressOf grid_DataError
        LoadGrid()
    End Sub
    Private Sub LoadGrid()
        Try
            Dim sql = "SELECT * FROM COMMPKRTB ORDER BY PKRNAME"
            conn = New SqlConnection(helper._connectionString)
            adapter = New SqlDataAdapter(sql, conn)
            builder = New SqlCommandBuilder(adapter)
            dt = New DataTable()
            adapter.Fill(dt)

            grid.DataSource = dt
            grid.ReadOnly = False
            grid.AllowUserToAddRows = True
            grid.AllowUserToDeleteRows = True
            grid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            ReplaceWithComboBox("Mode", {"SlipringMode", "ControllerMode"})
            ReplaceWithComboBox("CommunicationType", {"Ethernet", "SerialPort"})
            ReplaceWithComboBox("PortName", SerialPort.GetPortNames())
            ReplaceWithComboBox("Parity", {"None", "Even", "Mark", "Space", "Odd"})
            ReplaceWithComboBox("PKRTYPE", {"F800", "F800E", "F701S"})
            ReplaceWithComboBox("IsCheckWeigher", {"True", "False"})
            ReplaceWithComboBox("CWPortName", SerialPort.GetPortNames())
            ReplaceWithComboBox("CWParity", {"None", "Even", "Mark", "Space", "Odd"})
            AddHandler grid.CellValueChanged, AddressOf grid_CellValueChanged
            AddHandler grid.CurrentCellDirtyStateChanged, AddressOf grid_CurrentCellDirtyStateChanged

            For Each row As DataGridViewRow In grid.Rows
                ApplyCommunicationTypeRules(row)
            Next
        Catch ex As Exception
            MessageBox.Show("Unexpected error while loading grid: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub grid_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs)
        If grid.IsCurrentCellDirty Then
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub grid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 AndAlso grid.Columns(e.ColumnIndex).Name = "CommunicationType" Then
            Dim row = grid.Rows(e.RowIndex)
            ApplyCommunicationTypeRules(row)
        ElseIf e.RowIndex >= 0 AndAlso grid.Columns(e.ColumnIndex).Name = "IsCheckWeigher" Then
            Dim row = grid.Rows(e.RowIndex)
            ApplyCommunicationTypeRules(row)
        End If
    End Sub

    Private Sub ApplyCommunicationTypeRules(row As DataGridViewRow)
        Try
            If row.IsNewRow Then Exit Sub
            Dim commType As String = Convert.ToString(row.Cells("CommunicationType").Value)
            Dim ConType As String = Convert.ToString(row.Cells("IsCheckWeigher").Value)
            If String.Equals(commType, "Ethernet", StringComparison.OrdinalIgnoreCase) Then
                ' Disable SerialPort fields
                DisableAndSetDefault(row, {"PortName", "BaudRate", "Parity"}, 0)
                EnableCells(row, {"PORT", "IP"})
            ElseIf String.Equals(commType, "SerialPort", StringComparison.OrdinalIgnoreCase) Then
                ' Disable Ethernet fields
                DisableAndSetDefault(row, {"PORT", "IP"}, 0)
                EnableCells(row, {"PortName", "BaudRate", "Parity"})
                'Else
                '    ' If undefined, disable all
                '    DisableAndSetDefault(row, {"PortName", "BaudRate", "Parity", "PORT", "IP", "CWPortName", "CWBaudRate", "CWParity"}, 0)
            End If

            If String.Equals(ConType, "False", StringComparison.OrdinalIgnoreCase) Then
                ' Disable Ethernet fields
                DisableAndSetDefault(row, {"CWPortName", "CWBaudRate", "CWParity"}, 0)
            ElseIf String.Equals(ConType, "True", StringComparison.OrdinalIgnoreCase) Then
                EnableCells(row, {"CWPortName", "CWBaudRate", "CWParity"})
                'Else
                '    ' If undefined, disable all
                '    DisableAndSetDefault(row, {"PortName", "BaudRate", "Parity", "PORT", "IP", "CWPortName", "CWBaudRate", "CWParity"}, 0)
            End If
        Catch ex As Exception
            MessageBox.Show("Error applying rules: " & ex.Message)
        End Try
    End Sub

    Private Sub DisableAndSetDefault(row As DataGridViewRow, columnNames As IEnumerable(Of String), defaultValue As Object)
        For Each colName In columnNames
            If grid.Columns.Contains(colName) Then
                Dim cell = row.Cells(colName)
                cell.ReadOnly = True
                cell.Style.BackColor = Color.LightGray

                ' This sets numeric fields to 0, and string fields to "" 
                If TypeOf cell.Value Is String OrElse grid.Columns(colName).ValueType Is GetType(String) Then
                    cell.Value = ""
                Else
                    cell.Value = defaultValue
                End If
            End If
        Next
    End Sub

    Private Sub EnableCells(row As DataGridViewRow, columnNames As IEnumerable(Of String))
        For Each colName In columnNames
            If grid.Columns.Contains(colName) Then
                Dim cell = row.Cells(colName)
                cell.ReadOnly = False
                cell.Style.BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub ReplaceWithComboBox(columnName As String, items As IEnumerable(Of String))
        Try
            If grid.Columns.Contains(columnName) Then
                Dim comboCol As New DataGridViewComboBoxColumn() With {
                .HeaderText = columnName,
                .Name = columnName,
                .DataPropertyName = columnName
                }
                comboCol.Items.AddRange(items.ToArray())
                Dim colIndex As Integer = grid.Columns(columnName).Index
                grid.Columns.Remove(columnName)
                grid.Columns.Insert(colIndex, comboCol)
            End If
        Catch ex As Exception
            MessageBox.Show($"Error setting up column '{columnName}': {ex.Message}", "Grid Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub grid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        If TypeOf grid.Columns(e.ColumnIndex) Is DataGridViewComboBoxColumn Then
            Dim col As DataGridViewComboBoxColumn = CType(grid.Columns(e.ColumnIndex), DataGridViewComboBoxColumn)
            Dim cellValue = grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            If cellValue IsNot Nothing AndAlso Not col.Items.Contains(cellValue.ToString()) Then
                col.Items.Add(cellValue.ToString())
                e.ThrowException = False
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Dim changes As DataTable = dt.GetChanges()
            If changes IsNot Nothing Then
                adapter.Update(dt)
                dt.AcceptChanges()
                'MessageBox.Show("Changes Saved!.Application will restart now.")
                MessageBox.Show("Changes saved to the database 👍. Kindly restart the application!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("No changes to save 👎.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Error while saving: " & ex.Message)
        End Try
        LoadGrid()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
    Private Sub grid_KeyDown(sender As Object, e As KeyEventArgs) Handles grid.KeyDown
        If e.KeyCode = Keys.Delete Then
            If grid.CurrentRow Is Nothing OrElse grid.CurrentRow.IsNewRow Then
                Return
            End If
            Dim result = MessageBox.Show("Delete selected row?", "Confirm", MessageBoxButtons.YesNo)
            If result = DialogResult.Yes Then
                Try
                    grid.Rows.Remove(grid.CurrentRow)
                    If adapter IsNot Nothing AndAlso dt IsNot Nothing Then
                        adapter.Update(dt)
                        dt.AcceptChanges()
                        MessageBox.Show("Application will restart now.")
                        'MessageBox.Show("Row deleted from database 👍.")
                    Else
                        MessageBox.Show("Adapter or DataTable not initialized 👎. Did you call LoadGrid()?")
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error deleting row: " & ex.Message)
                End Try
            End If
            e.Handled = True
        End If
    End Sub
    Private Sub frmPackerPort_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        MDIParent1.Show()
    End Sub
End Class
