Public Class frmConfigBatProp
    Dim TSPCFG(15, MAXSPOUT) As String
    Dim CdOKFl As Boolean
    Dim ULFL1 As Boolean
    Private text2 As List(Of TextBox)
    Private Command2 As List(Of Button)
    Dim qname As String
    Dim _UniPulse As New Unipulse
    Dim RaiseFL As Boolean
    Dim FunctionsToCall() As String
    Private Sub frmConfigBatProp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ProgressBar1.Value = 0
        ProgressBar1.Visible = False
        HandlePackerType(1)
        CurrentFormOpened = Me.Name
        AddHandler PackerCommunication.UpdateBatchParameterProgressBar, AddressOf HandleProgressBar
    End Sub
    Private Sub HandlePackerType(packerno As Integer)
        ComboBox4.Items.Clear()
        If packerno >= 1 Then
            Dim _Packer As New Packer
            _Packer = Packers.FirstOrDefault(Function(e) e.DeviceId = packerno)
            If _Packer.ControllerModel = "F800E" Then
                text2 = New List(Of TextBox) From {
                                              Txtsp2adjustpoint, txtsp2, txtfinal, txtOver1, txtUnder1,
                                              txtcps, txtaffc, txtfinaladjustmentwt, txtsp3TargetWt, txtSp3AllowTime
                                          }
                Dim var() As String = {"SP2 Adjust Weight", "SP2", "FINAL", "OVER", "UNDER", "CPS", "AFFC", "Final Adjust Weight", "SP3 TARGET TIME", "SP3 ALLOW TIME"}
                For I = 0 To var.Length - 1
                    ComboBox4.Items.Add(var(I))
                Next
                FunctionsToCall = {"SP2AdjustmentWeight", "SP2", "Final", "Over", "Under", "CPS", "AutoFreeFallCompensation", "FinalAdjustWeight", "SP3TargetTime", "SP3AllowableTime"}
                Panel1.Visible = True
                Panel2.Visible = False
                Panel3.Visible = False
            ElseIf _Packer.ControllerModel = "F800" Then
                text2 = New List(Of TextBox) From {
                                               txtsp2Setpoint, txtTargetWeight, txtOver2, txtUnder2, txtCps2,
                                              txtAffc2, txtsp1, TxtCompFeedTime
                                          }
                Dim var() As String = {"SP2", "FINAL", "OVER", "UNDER", "CPS", "AFFC", "SP1", "COMPENSATION FEED TIME"}
                For I = 0 To var.Length - 1
                    ComboBox4.Items.Add(var(I))
                Next
                FunctionsToCall = {"SP2", "Final", "Over", "Under", "CPS", "AutoFreeFallCompensation", "SP1", "CompensationFeedingTime"}
                Panel2.Visible = True
                Panel1.Visible = False
                Panel3.Visible = False
            ElseIf _Packer.ControllerModel = "F701S" Then
                text2 = New List(Of TextBox) From {
                                               txtsetpoint2, txtfinalweight, txtover3, txtunder3, TxtTotalCompSelection, TxtCompensation,
                                              txtTotalLimitHigh4, txtaffc3, txtSetPoint1, txtTotalLimitUnder5, txtFeedDuration, TxtCountTime
                                          }
                Dim var() As String = {"SP2", "FINAL", "OVER", "UNDER", "TOTAL COMPARISON SELECTION", "COMPENSATION", "TOTAL LIMIT(HIGH4)", "AFFC", "SP1", "TOTAL LIMIT (UNDER5)", "COMPENSATION FEED TIME", "COUNT LIMIT"}
                For I = 0 To var.Length - 1
                    ComboBox4.Items.Add(var(I))
                Next
                FunctionsToCall = {"SP2", "Final", "Over", "Under", "TotalComparisonSelection", "CPS", "TotallimitHigh", "AutoFreeFallCompensation", "SP1", "TotallimitUnder", "CompensationFeedingTime", "CountLimit"}
                Panel3.Visible = True
                Panel1.Visible = False
                Panel2.Visible = False
            End If
        End If
    End Sub
    Private Sub HandleProgressBar(sender As Object, e As ProgressbarEventAgrs)
        Try
            If e.ProgessValue > 17 Then
                e.ProgessValue = 0
            End If
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
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim PackerId As Integer
        Dim SpoutId As Integer
        Dim CurrentCode As Integer
        CurrentFormOpened = Me.Name
        cleartextbox()
        If ComboBox1.Text <> "" And ComboBox2.Text <> "" Then
            SpoutId = CInt(ComboBox2.Text)
            PackerId = ComboBox1.SelectedIndex + 1
            CurrentCode = ComboBox3.Text
            TxtCodeno.Text = ComboBox3.Text
            codeno.Text = ComboBox3.Text
            txtCodeno3.Text = ComboBox3.Text
            RdSPF800(PackerId, SpoutId, CurrentCode)
        End If
    End Sub
    Private Sub RdSPF800(PackerId As Integer, SpoutId As Integer, CurrentCode As Integer)
        Dim I As Integer
        Dim Inbuf As String = ""
        Dim _Packer As New Packer
        Dim _spout As New SpoutController
        If ComboBox1.Text = "" Or ComboBox2.Text = "" Then Exit Sub
        ProgressBar1.Value = 0
        ProgressBar1.Visible = True
STARTAGAIN1:
        _Packer = Packers.FirstOrDefault(Function(e) e.DeviceId = PackerId)
        _spout = _Packer.SpoutList.FirstOrDefault(Function(e) e.SpoutId = SpoutId)
        Dim Query As QueryInfo = _UniPulse.ChangeCode(_spout.ControllerModel, SpoutId, CurrentCode.ToString())
        PackerCommunication.EnqueueDeviceCommand(PackerId, SpoutId, 16, "Write Code", Query.Query)
        Dim allMethods = GetType(Unipulse).GetMethods()
        Dim validMethods = FunctionsToCall.SelectMany(Function(fn) allMethods.Where(Function(m) m.Name = fn)).ToList()
        I = 0
        For Each m In validMethods
            Dim txtbox = text2(I)
            Dim ReadParameterQuery As QueryInfo = m.Invoke(_UniPulse, New Object() {_spout.ControllerModel, SpoutId, ""})
            PackerCommunication.EnqueueDeviceCommand(PackerId, SpoutId, ReadParameterQuery.ResponseLength, m.Name, ReadParameterQuery.Query, txtbox, ReadParameterQuery.DecimalFormat)
            I += 1
        Next
        Label2.Text = "Last Download of " & "Packer-" & PackerId & " and Spout " & SpoutId & " at " & Format(Now, "dd-MM-yyyy") & " - " & Format(Now, "HH:mm:ss")
    End Sub
    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        Me.Close()
    End Sub
    Private Sub ComboBox1_GotFocus(sender As Object, e As EventArgs) Handles ComboBox1.GotFocus
        Dim I As Integer
        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        ComboBox3.Items.Clear()
        For I = 1 To packerCount
            ComboBox1.Items.Add(I)
        Next
        For j = 1 To spoutCount - 1
            ComboBox2.Items.Add(j)
        Next
        For z = 0 To 9
            ComboBox3.Items.Add(z)
        Next
    End Sub
    Private Sub cleartextbox()
        For Each tb As TextBox In text2
            tb.Text = ""
        Next
        Label2.Text = ""
        Label13.Text = ""
        ComboBox4.Text = ""
        TextBox26.Text = ""
        ProgressBar1.Value = 0
    End Sub
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        'Dim btn As Button = DirectCast(sender, Button)
        Dim index As Integer = ComboBox4.SelectedIndex + 1
        Dim _Packer As New Packer
        Dim _spout As New SpoutController
        Dim EnteredValue = TextBox26.Text.Trim()
        ProgressBar1.Value = 0
        ProgressBar1.Visible = True
        CurrentFormOpened = Me.Name
        If EnteredValue <> "" Then
            If ComboBox1.Text = "" Or ComboBox2.Text = "" Then Exit Sub
            Dim PackerId = Integer.Parse(ComboBox1.Text)
            Dim SpoutId = Integer.Parse(ComboBox2.Text)
            Dim CurrentCode = Integer.Parse(ComboBox3.Text)
            _Packer = Packers.FirstOrDefault(Function(Pkr) Pkr.DeviceId = PackerId)
            _spout = _Packer.SpoutList.FirstOrDefault(Function(Pkr) Pkr.SpoutId = SpoutId)
            Dim Query As QueryInfo = _UniPulse.ChangeCode(_spout.ControllerModel, SpoutId, CurrentCode.ToString())
            PackerCommunication.EnqueueDeviceCommand(PackerId, SpoutId, 16, "Write Code", Query.Query,,, "Write")
            Dim allMethods = GetType(Unipulse).GetMethods()
            Dim validMethods = allMethods.Where(Function(m) m.Name = FunctionsToCall(index - 1)).ToList()
            For Each m In validMethods
                Dim txtbox = text2(index - 1)
                txtbox.Text = ""
                Dim ReadParameterQuery As QueryInfo = m.Invoke(_UniPulse, New Object() {_spout.ControllerModel, SpoutId, ""})
                Dim value = _UniPulse.FormatValue(EnteredValue, 5, ReadParameterQuery.DecimalFormat)
                Dim WriteParameterQuery As QueryInfo = m.Invoke(_UniPulse, New Object() {_spout.ControllerModel, SpoutId, value})
                PackerCommunication.EnqueueDeviceCommand(PackerId, SpoutId, WriteParameterQuery.ResponseLength, m.Name, WriteParameterQuery.Query, txtbox, WriteParameterQuery.DecimalFormat, "Write")
                PackerCommunication.EnqueueDeviceCommand(PackerId, SpoutId, ReadParameterQuery.ResponseLength, m.Name, ReadParameterQuery.Query, txtbox, ReadParameterQuery.DecimalFormat, "Read")
            Next
        Else
            MsgBox("Please enter Value and Proceed")
        End If
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        HandlePackerType(ComboBox1.Text)
        cleartextbox()
    End Sub
    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        Dim i = ComboBox4.SelectedIndex
        Label13.Text = text2(i).Text
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        cleartextbox()
    End Sub
    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        cleartextbox()
    End Sub
End Class