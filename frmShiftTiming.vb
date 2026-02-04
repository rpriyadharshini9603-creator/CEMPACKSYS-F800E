Public Class frmShiftTiming
    Private Sub frmShiftTiming_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DateTimePicker1.CustomFormat = "HH:mm:ss"
        DateTimePicker1.Format = DateTimePickerFormat.Custom
        DateTimePicker1.CustomFormat = "HH:mm:ss"
        DateTimePicker1.ShowUpDown = True

        DateTimePicker2.CustomFormat = "HH:mm:ss"
        DateTimePicker2.Format = DateTimePickerFormat.Custom
        DateTimePicker2.CustomFormat = "HH:mm:ss"
        DateTimePicker2.ShowUpDown = True


        DateTimePicker3.CustomFormat = "HH:mm:ss"
        DateTimePicker3.Format = DateTimePickerFormat.Custom
        DateTimePicker3.CustomFormat = "HH:mm:ss"
        DateTimePicker3.ShowUpDown = True

        DateTimePicker4.CustomFormat = "HH:mm:ss"
        DateTimePicker4.Format = DateTimePickerFormat.Custom
        DateTimePicker4.CustomFormat = "HH:mm:ss"
        DateTimePicker4.ShowUpDown = True

        DateTimePicker5.CustomFormat = "HH:mm:ss"
        DateTimePicker5.Format = DateTimePickerFormat.Custom
        DateTimePicker5.CustomFormat = "HH:mm:ss"
        DateTimePicker5.ShowUpDown = True

        DateTimePicker6.CustomFormat = "HH:mm:ss"
        DateTimePicker6.Format = DateTimePickerFormat.Custom
        DateTimePicker6.CustomFormat = "HH:mm:ss"
        DateTimePicker6.ShowUpDown = True


        Dim X
        Dim keys As String() = {"Shft1St", "Shft1End", "Shft2St", "Shft2End", "Shft3St", "Shft3End"}
        Dim pickers As DateTimePicker() = {DateTimePicker1, DateTimePicker2, DateTimePicker3, DateTimePicker4, DateTimePicker5, DateTimePicker6}
        For i As Integer = 0 To keys.Length - 1
            X = RestoreSettings(TITLE1, "Properties", keys(i))

            If TimeSpan.TryParse(X, Nothing) Then
                Dim time As TimeSpan = TimeSpan.Parse(X)
                pickers(i).Value = DateTime.Today.Add(time)
            Else
                ' If invalid or empty, set default fallback time (e.g., 06:00:00)
                pickers(i).Value = DateTime.Today.AddHours(6)
            End If
        Next

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim X
        X = SaveProfile_Setting(TITLE1, "Properties", "Shft1St", DateTimePicker1.Value.ToString("HH:mm:ss"))
        X = SaveProfile_Setting(TITLE1, "Properties", "Shft1End", DateTimePicker2.Value.ToString("HH:mm:ss"))
        X = SaveProfile_Setting(TITLE1, "Properties", "Shft2St", DateTimePicker3.Value.ToString("HH:mm:ss"))
        X = SaveProfile_Setting(TITLE1, "Properties", "Shft2End", DateTimePicker4.Value.ToString("HH:mm:ss"))
        X = SaveProfile_Setting(TITLE1, "Properties", "Shft3St", DateTimePicker5.Value.ToString("HH:mm:ss"))
        X = SaveProfile_Setting(TITLE1, "Properties", "Shft3End", DateTimePicker6.Value.ToString("HH:mm:ss"))
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class

