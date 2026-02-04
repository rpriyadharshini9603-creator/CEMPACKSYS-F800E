Imports System.Windows.Forms
Imports System.Drawing
Imports System.Data.SqlClient
Public Class frm_user_pass

    Private Sub frm_user_pass_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim userTypeQuery As String = "SELECT DISTINCT UserType FROM UserNameDetails"
            Dim userTypeTable As DataTable = helper.ExecuteReader(userTypeQuery)

            cbUserType.Items.Clear()
            For Each row As DataRow In userTypeTable.Rows
                cbUserType.Items.Add(row("UserType").ToString())
            Next

            Dim userNameQuery As String = "SELECT DISTINCT UserName FROM UserNameDetails"
            Dim userNameTable As DataTable = helper.ExecuteReader(userNameQuery)

            cbUserName.Items.Clear()
            For Each row As DataRow In userNameTable.Rows
                cbUserName.Items.Add(row("UserName").ToString())
            Next

        Catch ex As Exception
            Call CentralErrhandler("FormLoad", Me.Name)
        End Try

        cbUserType.SelectedIndex = 0
        cbUserName.SelectedIndex = 0
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Button3.Visible = False
        Button4.Visible = True
        txtPassword.PasswordChar = ""
    End Sub

    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button1_Click(Nothing, e)
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If String.IsNullOrWhiteSpace(cbUserType.Text) OrElse
       String.IsNullOrWhiteSpace(cbUserName.Text) OrElse
       String.IsNullOrWhiteSpace(txtPassword.Text) Then

            lblWarning.ForeColor = Color.Red
            lblWarning.Text = "Please fill in all fields!"
            Exit Sub
        End If

        Try
            Dim sql As String = "SELECT Password FROM UserNameDetails WHERE UserName = @username"
            Dim parameters As SqlParameter() = {
            New SqlParameter("@username", cbUserName.Text.Trim())
        }

            Dim dt As DataTable = helper.ExecuteReader(sql, parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim passwordFromDb As String = dt.Rows(0)("Password").ToString()

                If passwordFromDb = txtPassword.Text.Trim() Then
                    txtPassword.Clear()

                    USERTYPESTR = cbUserType.Text.Trim()
                    USERNAMESTR = cbUserName.Text.Trim()

                    If USERTYPESTR = "Supplier" AndAlso USERNAMESTR = "AUTHORIZED" Then
                        frmPackerPort.Show()
                    Else
                        Me.Hide()
                        MDIParent1.Show()
                    End If

                    Me.Close()
                Else
                    txtPassword.Clear()
                    lblWarning.Visible = True
                    lblWarning.Text = "Invalid username or password"
                End If
            Else
                MessageBox.Show("Un-Identified Error in Selecting the User Name!!", "Contact Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            Call CentralErrhandler("LoginCheck", Me.Name)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SQLCONN.Close()
        Me.Close()
        Application.Exit()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Button3.Visible = True
        Button4.Visible = False
        txtPassword.PasswordChar = "*"
    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged
        lblWarning.Visible = False
    End Sub


End Class
