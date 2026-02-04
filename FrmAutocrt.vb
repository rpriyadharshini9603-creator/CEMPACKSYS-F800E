Public Class FrmAutocrt
    Private isFormLoaded As Boolean = False
    Private Sub FrmAutocrt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For I = 1 To 10
            ComboBox1.Items.Add(I)
        Next

        Dim val() As String = {"0.25", "0.50", "0.75", "1"}
        For I = 1 To val.Length - 1
            ComboBox2.Items.Add(val(I - 1))
        Next
        For i = 1 To packerCount
            ComboBox3.Items.Add(i)
        Next
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
        Dim packerno As String = ComboBox3.SelectedIndex
        ComboBox1.Text = sample(packerno)
        ComboBox2.Text = coefficient(packerno)
        TextBox1.Text = CorrectionRangeFrom(packerno)
        TextBox2.Text = CorrectionRangeTo(packerno)

        TextBox3.Text = CorrectionLimitFrom(packerno)
        TextBox4.Text = CorrectionLimitTo(packerno)

        TextBox5.Text = ValidWeightFrom(packerno)
        TextBox6.Text = ValidWeightTo(packerno)

        If IsAutoCorrectionEnabled(packerno) Then
            CheckBox1.Checked = True
        Else
            CheckBox1.Checked = False
        End If
        isFormLoaded = True
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim packerno As String = ComboBox3.SelectedIndex
        sample(packerno) = ComboBox1.Text.Trim()
        coefficient(packerno) = ComboBox2.Text.Trim()
        ValidWeightFrom(packerno) = TextBox5.Text.Trim()
        ValidWeightTo(packerno) = TextBox6.Text.Trim()
        CorrectionLimitFrom(packerno) = TextBox3.Text.Trim()
        CorrectionLimitTo(packerno) = TextBox4.Text.Trim()
        CorrectionRangeFrom(packerno) = TextBox1.Text.Trim()
        CorrectionRangeTo(packerno) = TextBox2.Text.Trim()
        SaveProfile_Setting(TITLE1, Me.Name, "Samples" & packerno, ComboBox1.Text.Trim())
        SaveProfile_Setting(TITLE1, Me.Name, "Coefficient" & packerno, ComboBox2.Text.Trim())
        SaveProfile_Setting(TITLE1, Me.Name, "ValidWeightFrom" & packerno, TextBox5.Text.Trim())
        SaveProfile_Setting(TITLE1, Me.Name, "ValidWeightTo" & packerno, TextBox6.Text.Trim())
        SaveProfile_Setting(TITLE1, Me.Name, "CorrectionLimitFrom" & packerno, TextBox3.Text.Trim())
        SaveProfile_Setting(TITLE1, Me.Name, "CorrectionLimitTo" & packerno, TextBox4.Text.Trim())
        SaveProfile_Setting(TITLE1, Me.Name, "CorrectionRangeFrom" & packerno, TextBox1.Text.Trim())
        SaveProfile_Setting(TITLE1, Me.Name, "CorrectionRangeTo" & packerno, TextBox2.Text.Trim())
        SaveProfile_Setting(TITLE1, Me.Name, "IsAutoCorrectionEnabled" & packerno, IsAutoCorrectionEnabled(packerno))
        Me.Close()
    End Sub
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Dim packerno = ComboBox3.SelectedIndex
        If CheckBox1.Checked = True Then
            IsAutoCorrectionEnabled(packerno) = True
        Else
            IsAutoCorrectionEnabled(packerno) = False
        End If
    End Sub
    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        Me.Close()
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If Not isFormLoaded Then Exit Sub
        Dim packerno As Integer = ComboBox3.SelectedIndex
            ComboBox1.Text = sample(packerno)
            ComboBox2.Text = coefficient(packerno)
            TextBox5.Text = ValidWeightFrom(packerno)
            TextBox6.Text = ValidWeightTo(packerno)
            TextBox3.Text = CorrectionLimitFrom(packerno)
            TextBox4.Text = CorrectionLimitTo(packerno)
            TextBox1.Text = CorrectionRangeFrom(packerno)
            TextBox2.Text = CorrectionRangeTo(packerno)
        CheckBox1.Checked = IsAutoCorrectionEnabled(packerno)
    End Sub
End Class