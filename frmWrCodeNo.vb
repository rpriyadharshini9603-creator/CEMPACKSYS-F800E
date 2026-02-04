Public Class frmWrCodeNo
    Dim _Packer As New Packer
    Dim _spout As New SpoutController
    Dim _UniPulse As New Unipulse

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim J As Integer = ComboBox1.SelectedIndex + 1
        Dim S As Integer = ComboBox2.SelectedIndex + 1
        Dim RPLY
        _Packer = Packers.FirstOrDefault(Function(f) f.DeviceId = J)
        _spout = _Packer.SpoutList.FirstOrDefault(Function(f) f.SpoutId = S)
        If ComboBox1.Text <> "" And ComboBox2.Text <> "" And TextBox1.Text <> "" Then
            S = CInt(ComboBox2.Text)
            J = ComboBox1.SelectedIndex + 1
            If _spout.CodeNo <> TextBox1.Text Then
                RPLY = MsgBox("CHANGING THE CODE NO. WILL CHANGE THE CURRENT LOADING PARAMETERS..." & vbCrLf &
                        "DO YOU WANT TO CONTINUE?", vbYesNo + vbCritical, "CAUTION...CODENO CHANGE")
                If RPLY = 6 Then
                    WriteSPData()
                End If
            End If
        Else
            MessageBox.Show("Kindly fill all the fields", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub WriteSPData()
        Dim I As Integer
        Dim J As Integer

        J = ComboBox1.SelectedIndex + 1
        I = ComboBox2.SelectedIndex + 1
        _Packer = Packers.FirstOrDefault(Function(e) e.DeviceId = J)
        _spout = _Packer.SpoutList.FirstOrDefault(Function(e) e.SpoutId = I)
        Dim SpoutId As Integer = _spout.SpoutId
        Dim PackerId As Integer = _spout.PackerName
        Dim Query As QueryInfo = _UniPulse.ChangeCode(_spout.ControllerModel, SpoutId)
        PackerCommunication.EnqueueDeviceCommand(PackerId, SpoutId, 16, "Write Code", Query.Query)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
    Private Sub ComboBox1_GotFocus(sender As Object, e As EventArgs) Handles ComboBox1.GotFocus
        Dim I As Integer
        ComboBox1.Items.Clear()

        For I = 1 To packerCount
            ComboBox1.Items.Add(I)
        Next I
        ComboBox1.SelectedIndex = 0
    End Sub
    Private Sub ComboBox1_LostFocus(sender As Object, e As EventArgs) Handles ComboBox1.LostFocus
        Dim I As Integer
        Dim J As Integer
        J = ComboBox1.SelectedIndex + 1
        If ComboBox1.Text <> "" Then
            For J = 1 To packerCount
                For I = 1 To spoutCount - 1
                    ComboBox2.Items.Add(I)

                Next I
            Next
        End If
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs)
        ' Allow only digits and "." (ASCII 46)
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = "."c Then
            Dim currentText As String = TextBox1.Text
            Dim newText As String = currentText.Insert(TextBox1.SelectionStart, e.KeyChar.ToString())
            If Double.TryParse(newText, Nothing) AndAlso Val(newText) >= 10 Then
                e.Handled = True
            End If
        ElseIf e.KeyChar = ControlChars.Back Then
            ' Allow Backspace
            e.Handled = False
        Else
            ' Block all other keys
            e.Handled = True
        End If
    End Sub
End Class
