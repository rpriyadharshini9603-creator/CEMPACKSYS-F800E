Public Class LicenseDetails
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LicenseManager.ActivateLicense(RichTextBox1.Text)
        Me.Close()
    End Sub

    Private Sub AddLicense_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label2.Text = "Mac-" & LicenseManager.GetMacAddress()
        Label3.Text = "Valid Upto-" & LicenseManager.CheckValidity & " Days Left"
    End Sub
End Class