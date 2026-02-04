Imports System.ComponentModel

Public Class frmgridscale

    Private Sub frmgridscale_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Y
        Y = RestoreSettings(TITLE1, "GridProperties", "GRIDWIDTH")
        If Y <> "" Then
            TextBox1.Text = Y
        Else
            TextBox1.Text = 1200
        End If
        Y = RestoreSettings(TITLE1, "GridProperties", "GridHeight")
        If Y <> "" Then
            TextBox2.Text = Y
        Else
            TextBox2.Text = 1200
        End If
        Y = RestoreSettings(TITLE1, "GridProperties", "Greentext")
        If Y <> "" Then
            TextBox5.Text = Y
        Else
            TextBox5.Text = 50.5
        End If
        Y = RestoreSettings(TITLE1, "GridProperties", "Redtext")
        If Y <> "" Then
            TextBox3.Text = Y
        Else
            TextBox3.Text = 48.8
        End If
        Y = RestoreSettings(TITLE1, "GridProperties", "Bluetext")
        If Y <> "" Then
            TextBox4.Text = Y
        Else
            TextBox4.Text = 49.5
        End If


        Y = RestoreSettings(TITLE1, "GridProperties", "GridFlag")
        If Y <> "" Then
            If Y = "True" Then
                CheckBox1.Checked = True
                GRIDSCALEFL = True
            Else
                CheckBox1.Checked = False
                GRIDSCALEFL = False
            End If
        Else
            CheckBox1.Checked = False
            GRIDSCALEFL = False
        End If

    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim X
        X = SaveProfile_Setting(TITLE1, "GridProperties", "GridWidth", TextBox1.Text)
        X = SaveProfile_Setting(TITLE1, "GridProperties", "GridHeight", TextBox2.Text)
        X = SaveProfile_Setting(TITLE1, "GridProperties", "Greentext", TextBox5.Text)
        X = SaveProfile_Setting(TITLE1, "GridProperties", "Redtext", TextBox3.Text)
        X = SaveProfile_Setting(TITLE1, "GridProperties", "Bluetext", TextBox4.Text)

        If CheckBox1.Checked = 1 Then
            GRIDSCALEFL = True
            X = SaveProfile_Setting(TITLE1, "GridProperties", "GridFlag", "True")
        Else
            GRIDSCALEFL = False
            X = SaveProfile_Setting(TITLE1, "GridProperties", "GridFlag", "Flase")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub frmgridscale_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim X
        X = SaveProfile_Setting(TITLE1, "GridProperties", "GridWidth", TextBox1.Text)
        X = SaveProfile_Setting(TITLE1, "GridProperties", "GridHeight", TextBox2.Text)
        X = SaveProfile_Setting(TITLE1, "GridProperties", "Greentext", TextBox5.Text)
        X = SaveProfile_Setting(TITLE1, "GridProperties", "Redtext", TextBox3.Text)
        X = SaveProfile_Setting(TITLE1, "GridProperties", "Bluetext", TextBox4.Text)

        If CheckBox1.Checked = 1 Then
            GRIDSCALEFL = True
            X = SaveProfile_Setting(TITLE1, "GridProperties", "GridFlag", "True")
        Else
            GRIDSCALEFL = False
            X = SaveProfile_Setting(TITLE1, "GridProperties", "GridFlag", "Flase")
        End If
    End Sub
End Class
