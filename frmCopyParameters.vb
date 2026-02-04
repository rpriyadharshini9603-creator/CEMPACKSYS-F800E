Public Class frmCopyParameters
    Dim SpoutList As New List(Of CheckBox)
    Private ParameterTextBox As New List(Of TextBox)
    Private ParameterCheckBox As New List(Of CheckBox)
    Dim _UniPulse As New Unipulse
    Dim _Packer As New Packer
    Dim _spout As New SpoutController
    Dim FunctionsToCall() As String
    Private Sub frmCopyParameters_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CurrentFormOpened = Me.Name
        ProgressBar1.Visible = False
        AddHandler PackerCommunication.UpdateSystemCopyParameterProgressBar, AddressOf HandleProgressBar
        AddHandler PackerCommunication.CopyCompleteEvent, AddressOf HandleCopyComplete

        SpoutList.AddRange(New CheckBox() {Spout1, Spout2, Spout3, Spout4,
                                            Spout5, Spout6, Spout7, Spout8})
        HandlePackerType(1)
        Dim I As Integer
        For I = 1 To packerCount
            CboPacker.Items.Add(I)
        Next I
        CboPacker.SelectedIndex = 0
        For I = 0 To 9
            CboCode.Items.Add(I)
        Next I
        CboCode.SelectedIndex = 0
        For I = 1 To 8
            CboSpout.Items.Add(I)
        Next I
        CboSpout.SelectedIndex = 0
    End Sub

    Private Sub HandlePackerType(packerno As String)
        If packerno >= 1 Then
            Dim _Packer As New Packer
            _Packer = Packers.FirstOrDefault(Function(e) e.DeviceId = packerno)
            If _Packer.ControllerModel = "F800E" Then

                FunctionsToCall = {"SP2AdjustmentWeight", "SP2", "Final", "Over",
            "Under", "CPS", "AutoFreeFallCompensation", "FinalAdjustWeight", "SP3TargetTime",
            "SP3AllowableTime"}
                ParameterTextBox = New List(Of TextBox) From {
                                                        SP2ADJUST_TEXT, SP2_TEXT, FINAL_TEXT,
                                                        OVER_TEXT, UNDER_TEXT, CPS_TEXT,
                                                        AFFC_TEXT, FINALADJUSTWT_TEXT,
                                                        SP3TARGETTIME_TEXT, SP3ALLOWTIME_TEXT}

                ParameterCheckBox = New List(Of CheckBox) From {
                                                        SP2ADJUST_CHECKBOX, SP2SETPOINT_CHECKBOX,
                                                        FINAL_CHECKBOX,
                                                        OVER_CHECKBOX, UNDER_CHECKBOX, CPS_CHECKBOX,
                                                        AFFC_CHECKBOX, FINALADJUSTWEIGHT_CHECKBOX,
                                                        SP3TARGET_CHECKBOX, SP3ALLOW_CHECKBOX}

                Panel3.Visible = True
                Panel4.Visible = False
                Panel5.Visible = False
            ElseIf _Packer.ControllerModel = "F800" Then
                FunctionsToCall = {"SP1", "SP2", "Final", "Over",
            "Under", "CPS", "AutoFreeFallCompensation", "CompensationFeedingTime"}
                ParameterTextBox = New List(Of TextBox) From {TxtSetPoint2,
                                                        txtsp2, txtTrgetweight, txtoverweight,
                                                        txtUnderweight, txtcps, txtaffc,
                                                        txtcompfdtime}

                ParameterCheckBox = New List(Of CheckBox) From {chksp2SetPoint,
                                                        chksp2, chkTargetwt,
                                                        chkOverwt,
                                                        chkUnderwt, chkcps, chkaffc,
                                                        chkcompfdtime}
                Panel4.Visible = True
                Panel3.Visible = False
                Panel5.Visible = False
            ElseIf _Packer.ControllerModel = "F701S" Then
                FunctionsToCall = {"SP2", "Final", "Over",
            "Under", "CPS", "AutoFreeFallCompensation", "CompensationFeedingTime", "SP1", "TotalComparisonSelection", "TotallimitHigh", "TotallimitUnder", "CountLimit"}
                ParameterTextBox = New List(Of TextBox) From {
                                                        TextBoxsp2, TextBoxTargetwt, TextBoxOver, TextBoxUnder,
                                                        TextBoxCps, TextBoxAffc, TextBoxCompFdTime, TextBoxSp1,
                                                        TextBoxTotCompSelect, TextBoxTotLimitH, TextBoxTotLimitL, TextBoxCountLimit}

                ParameterCheckBox = New List(Of CheckBox) From {
                                                        CheckBoxsp2, CheckBoxTargetwt,
                                                        CheckBoxOver, CheckBoxUnder, CheckBoxCps,
                                                        CheckBoxAffc, CheckBoxCompFdTime,
                                                        CheckBoxSP1, CheckBoxCompSelection, CheckBoxTotLimH, CheckBoxTotLimL, CheckBoxCountLim}
                Panel5.Visible = True
                Panel3.Visible = False
                Panel4.Visible = False
            End If
        End If

    End Sub
    Private Sub HandleCopyComplete(sender As Object, e As String)
        Try
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              ListBox1.Items.Add(e)
                          End Sub)
            Else
                ListBox1.Items.Add(e)
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub
    Private Sub HandleProgressBar(sender As Object, e As ProgressbarEventAgrs)
        Try
            'If e.ProgessValue > 17 Then
            '    Exit Sub
            'End If
            'If Me.InvokeRequired Then
            '    Me.Invoke(Sub()
            '                  ProgressBar1.Minimum = e.MinValue
            '                  ProgressBar1.Maximum = e.MaxValue
            '                  ProgressBar1.Value = e.ProgessValue
            '              End Sub)
            'Else
            '    ProgressBar1.Minimum = e.MinValue
            '    ProgressBar1.Maximum = e.MaxValue
            '    ProgressBar1.Value = e.ProgessValue
            'End If
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              ProgressBar1.Minimum = e.MinValue
                              ProgressBar1.Maximum = e.MaxValue
                              Dim val = e.ProgessValue
                              If val < ProgressBar1.Minimum Then val = ProgressBar1.Minimum
                              If val > ProgressBar1.Maximum Then val = ProgressBar1.Maximum
                              ProgressBar1.Value = val
                          End Sub)
            Else
                ProgressBar1.Minimum = e.MinValue
                ProgressBar1.Maximum = e.MaxValue
                Dim val = e.ProgessValue
                If val < ProgressBar1.Minimum Then val = ProgressBar1.Minimum
                If val > ProgressBar1.Maximum Then val = ProgressBar1.Maximum
                ProgressBar1.Value = val
            End If

        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub
    Private Sub RefrSptLst()
        Dim I As Integer
        Dim J As Integer
        If CboPacker.Text = "" Then Exit Sub
        J = CboPacker.SelectedIndex + 1
        CboSpout.Items.Clear()

        For I = 1 To spoutCount - 1
            CboSpout.Items.Add(I)
        Next I
        If CboSpout.SelectedIndex >= 1 Then CboSpout.SelectedIndex = 0
    End Sub


    Private Sub CboPacker_LostFocus(sender As Object, e As EventArgs)
        RefrSptLst()
    End Sub

    Private Sub CboSpout_Click(sender As Object, e As EventArgs)
        Dim I As Integer
        Dim J As Integer
        If CboPacker.SelectedIndex >= 0 Then
            I = CboPacker.SelectedIndex + 1
            If CboSpout.SelectedIndex >= 0 Then
                J = CboSpout.SelectedIndex + 1
                CboCode.SelectedIndex = _spout.CodeNo
            End If
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Cleartextbox()
        For Each TextBox1 As TextBox In ParameterTextBox
            TextBox1.Text = ""
        Next
        ProgressBar1.Value = 0
        ListBox1.Items.Clear()
    End Sub
    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Dim PackerId As Integer
        Dim SpoutId As Integer
        Dim CurrentCode As Integer
        CurrentFormOpened = Me.Name
        Cleartextbox()
        If String.IsNullOrWhiteSpace(CboPacker.Text) OrElse
           String.IsNullOrWhiteSpace(CboSpout.Text) OrElse
           String.IsNullOrWhiteSpace(CboCode.Text) Then Exit Sub

        SpoutId = CInt(CboSpout.Text)
        PackerId = CboPacker.SelectedIndex + 1
        CurrentCode = CboCode.Text
        RdSPF800(PackerId, SpoutId, CurrentCode)

    End Sub
    Private Sub RdSPF800(PackerId As Integer, SpoutId As Integer, CurrentCode As Integer)
        Dim I As Integer
        Dim Inbuf As String = ""
        Dim _Packer As New Packer
        Dim _spout As New SpoutController
        ProgressBar1.Value = 0
        ProgressBar1.Visible = True
        _Packer = Packers.FirstOrDefault(Function(e) e.DeviceId = PackerId)
        _spout = _Packer.SpoutList.FirstOrDefault(Function(e) e.SpoutId = SpoutId)

        Dim Query As QueryInfo = _UniPulse.ChangeCode(_spout.ControllerModel, SpoutId, CurrentCode.ToString())
        PackerCommunication.EnqueueDeviceCommand(PackerId, SpoutId, 16, "Write Code", Query.Query)

        Dim allMethods = GetType(Unipulse).GetMethods()
        Dim validMethods = allMethods.Where(Function(m) FunctionsToCall.Contains(m.Name)).ToList()
        I = 0
        For Each m In validMethods
            Dim txtbox = ParameterTextBox(I)
            Dim ReadParameterQuery As QueryInfo = m.Invoke(_UniPulse, New Object() {_spout.ControllerModel, SpoutId, ""})
            PackerCommunication.EnqueueDeviceCommand(PackerId, SpoutId, ReadParameterQuery.ResponseLength, m.Name, ReadParameterQuery.Query, txtbox, ReadParameterQuery.DecimalFormat)
            I += 1
        Next
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim IsSpoutSelected As Boolean
        Dim IsParameterSelected As Boolean
        Dim USER_REPESPONE As DialogResult
        CurrentFormOpened = Me.Name
        ListBox1.Items.Clear()
        ProgressBar1.Value = 0
        IsCopy = True
        If String.IsNullOrWhiteSpace(CboPacker.Text) OrElse
           String.IsNullOrWhiteSpace(CboSpout.Text) OrElse
           String.IsNullOrWhiteSpace(CboCode.Text) Then Exit Sub

        Dim PackerId As Integer = CboPacker.SelectedIndex + 1
        Dim SpoutId As Integer = CInt(CboSpout.Text)
        Dim CurrentCode As Integer = CInt(CboCode.Text)

        IsSpoutSelected = SpoutList.Any(Function(spout) spout.Checked)
        IsParameterSelected = ParameterCheckBox.Any(Function(Parameter) Parameter.Checked)

        Dim _Packer = Packers.FirstOrDefault(Function(f) f.DeviceId = PackerId)
        Dim _spout = _Packer.SpoutList.FirstOrDefault(Function(f) f.SpoutId = SpoutId)

        If Not IsParameterSelected Then
            MessageBox.Show("No Parameter is Selected for Upload", "Parameter Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not IsSpoutSelected Then
            MessageBox.Show("NONE OF THE SELECTED SPOUT IS COMMUNICATION HEALTHY. CANNOT COPY THE DATA", "SPOUT COMM ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        USER_REPESPONE = MessageBox.Show("This will Upload the Selected Parameters in the Selected Spouts!!." & vbCrLf &
                              "Do you want to Continue?", "Parameters Updation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If USER_REPESPONE = DialogResult.Yes Then
            Dim ListOfParameters As New List(Of String)
            For Each Parameter As CheckBox In ParameterCheckBox
                If Parameter.Checked Then
                    Dim ParameterIndex = Parameter.Tag

                    Dim EnteredValue As TextBox = ParameterTextBox(ParameterIndex)
                    For Each Spout As CheckBox In SpoutList
                        If Spout.Checked Then
                            WriteSPData(PackerId, Spout.Tag, EnteredValue.Text, ParameterIndex)
                        End If
                    Next
                    _Packer.IsParameterRequested = True
                End If
            Next
        End If
    End Sub
    Private Sub WriteSPData(PackerId As Integer, SpoutId As Integer, EnteredValue As String, ParameterIndex As Integer)
        Try
            _Packer = Packers.FirstOrDefault(Function(e) e.DeviceId = PackerId)
            _spout = _Packer.SpoutList.FirstOrDefault(Function(e) e.SpoutId = SpoutId)

            Dim Query As QueryInfo = _UniPulse.ChangeCode(_spout.ControllerModel, SpoutId)
            PackerCommunication.EnqueueDeviceCommand(PackerId, SpoutId, 16, "Write Code", Query.Query,,, "Write")
            Dim allMethods = GetType(Unipulse).GetMethods()
            Dim validMethods = allMethods.Where(Function(m) m.Name = FunctionsToCall(ParameterIndex)).ToList()

            For Each m In validMethods
                Dim ReadParameterQuery As QueryInfo = m.Invoke(_UniPulse, New Object() {_spout.ControllerModel, SpoutId, ""})
                Dim value = _UniPulse.FormatValue(EnteredValue, 5, ReadParameterQuery.DecimalFormat)
                Dim WriteParameterQuery As QueryInfo = m.Invoke(_UniPulse, New Object() {_spout.ControllerModel, SpoutId, value})
                PackerCommunication.EnqueueDeviceCommand(PackerId, SpoutId, WriteParameterQuery.ResponseLength, m.Name, WriteParameterQuery.Query,, WriteParameterQuery.DecimalFormat, "Write")
            Next
        Catch ex As Exception
            Console.WriteLine($"{ex.Message}")
        End Try
    End Sub

    Private Sub AllSpout_CheckedChanged(sender As Object, e As EventArgs) Handles AllSpout.CheckedChanged
        For Each Spout As CheckBox In SpoutList
            If AllSpout.Checked Then
                Spout.Checked = True
            Else
                Spout.Checked = False
            End If
        Next
    End Sub

    Private Sub CboPacker_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboPacker.SelectedIndexChanged
        HandlePackerType(CboPacker.Text)
    End Sub

End Class
