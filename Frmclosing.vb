Public Class Frmclosing
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        closingpassword = RestoreSettings(TITLE1, "Changepwd", "ClosingPassword")
        If txtPassword.Text = "ACCSYS" Or txtPassword.Text = "123" Or txtPassword.Text = closingpassword Then
            allowClose = True
            MDIParent1.Close()
            Me.Close()
        Else
            MessageBox.Show("Incorrect Password..")
            txtPassword.Text = ""
        End If
    End Sub
    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button2.PerformClick()
        End If
    End Sub

End Class