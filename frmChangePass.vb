Imports System.Data.SqlClient
Public Class frmChangePass
    Private ChAppPW As String = ""
    Private sqlHelper As SQLHelper

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        closingpassword = 123
        Try

            If TextBox1.Text.Trim() <> closingpassword Then
                MsgBox("Old Password is Not correct. Please Enter the correct Password", vbOKOnly, "Password Wrong")
                ClearAndUnload()
                Return
            End If

            If TextBox2.Text <> TextBox3.Text Then
                MsgBox("New Password does not match the Confirmation." & vbCrLf & "Enter the Correct Password", vbOKOnly, "Password Mismatch")
                ClearAndUnload()
                Return
            End If

            SaveProfile_Setting(TITLE1, "Changepwd", "ClosingPassword", TextBox3.Text.Trim())

            'If TextBox1.Text.Trim() <> ChAppPW Then
            '    MsgBox("Old Password is Not correct. Please Enter the correct Password", vbOKOnly, "Password Wrong")
            '    ClearAndUnload()
            '    Return
            'End If

            'If TextBox2.Text <> TextBox3.Text Then
            '    MsgBox("New Password does not match the Confirmation." & vbCrLf & "Enter the Correct Password", vbOKOnly, "Password Mismatch")
            '    ClearAndUnload()
            '    Return
            'End If

            'Dim colName = If(USERPASSFL, "Password", "ClPassword")
            'Dim success = UpdatePassword(colName, TextBox2.Text.Trim())

            'If success Then
            '    MsgBox("Successfully changed the Password", vbOKOnly, "Change Password")
            'Else
            '    MsgBox("No matching user found to update password", vbOKOnly, "User Not Found")
            'End If

            'USERPASSFL = False
            'Me.Close()

        Catch ex As Exception
            CentralErrhandler("btnChange_Click", Me.Name)
        End Try
    End Sub




    Private Sub frmChangePass_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            sqlHelper = New SQLHelper(DBHostName, True, DBName, DBUserId, DBPassword)
            ChAppPW = GetExistingPassword(USERPASSFL)
        Catch ex As Exception
            CentralErrhandler("frmChangePass_Load", Me.Name)
        End Try

        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
    End Sub


    Private Function GetExistingPassword(isAppPass As Boolean) As String
        Dim colName = If(isAppPass, "Password", "ClPassword")
        Dim query = $"SELECT {colName} FROM UserNameDetails WHERE UserType = @UserType AND UserName = @UserName"

        Dim parameters As SqlParameter() = {
        New SqlParameter("@UserType", USERTYPESTR),
        New SqlParameter("@UserName", USERNAMESTR)
    }

        Dim result = sqlHelper.ExecuteScalar(query, parameters)

        If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
            Return result.ToString()
        Else
            Return "ACCSYS"
        End If
    End Function

    Private Function UpdatePassword(colName As String, newPass As String) As Boolean
        Dim query = $"UPDATE UserNameDetails SET {colName} = @NewPass WHERE UserType = @UserType AND UserName = @UserName"

        Dim parameters As SqlParameter() = {
        New SqlParameter("@NewPass", newPass),
        New SqlParameter("@UserType", USERTYPESTR),
        New SqlParameter("@UserName", USERNAMESTR)
    }

        Dim rows = sqlHelper.ExecuteNonQuery(query, parameters)
        Return rows > 0
    End Function

    Private Sub ClearAndUnload()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        USERPASSFL = False
        Me.Close()
    End Sub
End Class