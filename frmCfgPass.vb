Public Class frmCfgPass
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "SPECIALPASS" Or TextBox1.Text = "123" Then
            CFGPASFL = True
            TextBox1.Text = ""
            Me.Hide()
        Else
            MsgBox("PASSWORD INCORRECT")
            TextBox1.Text = ""
        End If
    End Sub



    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If Asc(e.KeyChar) = 13 Then ' Enter key
            If TextBox1.Text <> "" Then
                Call Button1_Click(Nothing, EventArgs.Empty)
            End If
        End If
    End Sub


End Class