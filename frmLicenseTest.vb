

Public Class frmLicenseTest

    Private Sub frmLicenseTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Not LicenseManager.CheckLicense() Then
            MessageBox.Show("Please activate your license first.", "Activation Needed")
        End If
    End Sub

    Private Sub btnActivate_Click(sender As Object, e As EventArgs) Handles btnActivate.Click
        Dim key As String = txtKey.Text.Trim()
        LicenseManager.ActivateLicense(key)
    End Sub

    Private Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click
        If LicenseManager.CheckLicense() Then
            MessageBox.Show("License is valid!", "Check Result")
        Else
            MessageBox.Show("License not valid or expired!", "Check Result")
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim plain = "2CFDA174BB93|ACCSYS202530D"
        Dim enc = LicenseManager.EncryptString(plain, "Accsys!Secret@Key#2025")
        Clipboard.SetText(enc)
        MessageBox.Show("Encrypted key: " & enc)

    End Sub
End Class
