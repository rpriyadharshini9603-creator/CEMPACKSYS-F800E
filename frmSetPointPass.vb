Imports System.Drawing
Imports System.Data.SqlClient
Public Class frmSetPointPass

    Private Sub frmSetPointPass_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        KeyPreview = True
        TextBox1.Text = USERTYPESTR
        TextBox2.Text = USERNAMESTR
        TextBox3.Text = ""
        lblWarning.Visible = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim helper As New SQLHelper(DBHostName, True, DBName, DBUserId, DBPassword)
        Dim dt As DataTable
        Dim username As String = TextBox3.Text.Trim()
        Dim inputPassword As String = TextBox3.Text

        Dim query As String = "SELECT Password FROM UserNameDetails WHERE UserName = @UserName"
        Dim parameters As SqlParameter() = {
        New SqlParameter("@UserName", username)
    }

        Try
            dt = helper.ExecuteReader(query, parameters)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim dbPassword As String = dt.Rows(0)("Password").ToString()

                If dbPassword = inputPassword Then
                    TextBox3.Clear()
                    CFGPASFL = True
                    If Me.InvokeRequired Then
                        Me.Invoke(New MethodInvoker(AddressOf Me.Close))
                    Else
                        Me.Close()
                    End If
                Else
                    lblWarning.Text = "Invalid Password"
                    TextBox3.Clear()
                    Me.Close()
                End If
            Else
                lblWarning.Text = "Username not found."
                TextBox3.Clear()
                Me.Close()
            End If
        Catch ex As Exception
            CentralErrhandler(Me.Name, ex.Message)
        End Try
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        lblWarning.Text = ""
    End Sub
End Class


