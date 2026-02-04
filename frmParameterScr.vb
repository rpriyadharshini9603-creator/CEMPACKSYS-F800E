Imports System.Threading
Public Class frmParameterScr
    Dim ULFL1 As Boolean
    Public text2 As List(Of TextBox)
    Private Command2 As New List(Of Button)
    Dim qname As String
    Dim _UniPulse As New Unipulse
    Dim FunctionsToCall() As String
    Private Sub frmParameterScr_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim I As Integer
        Dim J As Integer
        ProgressBar1.Value = 0
        ProgressBar1.Visible = False
        CurrentFormOpened = Me.Name


        For I = 1 To packerCount
            ComboBox1.Items.Add(I)
        Next
        For J = 1 To spoutCount - 1
            ComboBox2.Items.Add(J)
        Next

        HandlePackerType(1)
        Button11.Enabled = True
        Button11.Visible = True
        AddHandler PackerCommunication.UpdateSystemParameterProgressBar, AddressOf HandleProgressBar
    End Sub
    Private Sub HandlePackerType(packerno As String)
        ComboBox4.Items.Clear()

        If packerno >= 1 Then
            Dim _Packer As New Packer
            _Packer = Packers.FirstOrDefault(Function(e) e.DeviceId = packerno)
            If _Packer.ControllerModel = "F800E" Then
                text2 = New List(Of TextBox)
                text2.AddRange(New TextBox() {txtDischagretime, txtUpperlimit, txtMotionDetection, txtmotiondetection2, txtLowerlimit, txtAverageWt, txtCompinhibittime, txtGenrealstddev,
       txtFunctionselection, txtSamplestddev, txtMaxwt, txtNearzero, txtweighingfunct1, txtSequencemode, txtweighingfunct2, txtTImer, txttimer2, txtweighingfunct3,
       txtGrossover, txtPresentTarewt, txtNetover, txtGeavAcceleraton, txtMinscalediv, txtAutoZerocount, txtcCapacity, txtFunctkeyinhibit, txtBalencewt, txtFillter,
       txtJudgingcount, txtZerotracking, txtzerotracking2
    })
                Dim var() As String = {"Discharge Time", "Upper Limit", "Motion Detection", "Motion Detection2", "Lower Limit", "Average Weight", "Comparision Inhibit Time",
            "General Std Deviation", "Function Selection", "Sample Std Deviation", "Maximum Weight", "Near Zero", "Weighing Function 1", "Sequence mode",
            "Weighing Function 2", "Timer", "Timer2", "Weighing Function 3", "Gross Over", "Present Tare Weight", "Net Over", "Gravitational Acceleration Compensation",
            "Min Scale Division", "Auto ZeroCount", "Capacity", "Function Key Inhibit", "Balance Weight", "Filter", "Judging Count", "Zero Tracking", "Zero Tracking2"}

                For I = 1 To var.Length - 1
                    ComboBox4.Items.Add(var(I - 1))
                Next
                FunctionsToCall = {"DischargeingTime", "UpperLimit", "MotionDetection", "MotionDetection2", "LowerLimit", "AverageWeight", "ComperisionInhibitTime", "GeneralStandardDeviation",
             "FunctionSelection", "SampleStandardDeviation", "MaximumWeight", "NearZero", "WeighingFunction1", "SequenceMode", "WeighingFunction2", "Timer", "Timer2", "WeighingFunction3",
             "GrossOver", "PresetTareWeight", "NetOver", "GravitationalAccelerationCompensation", "MinimumScaleDivision",
             "AutoZeroCount", "Capacity", "FunctionKeyInhibited", "BalanceWeightValue", "Filter", "JudgingCount", "ZeroTracking", "ZeroTracking2"}

                Panel1.Visible = True
                Panel2.Visible = False
                Panel3.Visible = False

            ElseIf _Packer.ControllerModel = "F800" Then
                text2 = New List(Of TextBox)
                text2.AddRange(New TextBox() {txtdischargetime1, txtUpper1, txtmotiondetection1, txtmotiondetection3, txtLower2, txtcompinhibitTime1, txtsequencemode1,
       txtfunctionselection1, txtgrossover1, txtweightingFunction1, txtnetover1, txtweightingFunction2, txtminscalediv1, txtweightingFunction3, txtcapacity1,
       txtgravAcceleration, txtbalweight1, txtAZ, txtjudgingcount1, txtFunctKeyInhibit1, txtZeroTracking1, txtZeroTracking3, txtfilter1,
       txtcompFeedingtime1, TextBoxTimer1, TextBoxTimer2, TextBoxNearZero
    })
                Dim var() As String = {"Discharge Time", "Upper Limit", "Motion Detection", "Motion Detection2", "Lower Limit", "Comparision Inhibit Time",
            "Sequence mode", "Function Selection", "Gross Over", "Weighing Function 1", "Net Over",
            "Weighing Function 2", "Min Scale Division", "Weighing Function 3", "Capacity", "Gravitational Acceleration Compensation", "Balance Weight",
             "Auto ZeroCount", "Judging Count", "Function Key Inhibit", "Filter", "Zero Tracking", "Zero Tracking2", "Compensation Feeding Time", "Timer", "Timer2", "NearZero"}

                For I = 1 To var.Length - 1
                    ComboBox4.Items.Add(var(I - 1))
                Next

                FunctionsToCall = {"DischargeingTime", "UpperLimit", "MotionDetection", "MotionDetection2", "LowerLimit", "ComperisionInhibitTime",
             "SequenceMode", "FunctionSelection", "GrossOver", "WeighingFunction1", "NetOver", "WeighingFunction2", "MinimumScaleDivision", "WeighingFunction3",
             "Capacity", "GravitationalAccelerationCompensation", "BalanceWeightValue",
             "AutoZeroCount", "JudgingCount", "FunctionKeyInhibited", "Filter", "ZeroTracking", "ZeroTracking2", "CompensationFeedingTime", "Timer", "Timer2", "NearZero"}

                Panel2.Visible = True
                Panel1.Visible = False
                Panel3.Visible = False
            ElseIf _Packer.ControllerModel = "F701S" Then
                text2 = New List(Of TextBox)
                text2.AddRange(New TextBox() {TextBoxupper, TextBoxlower, TextBoxCompInhibittime, TextBoxWtFunct1,
                                                TextBoxWtFunct2, TextBoxWtFunct3, TextBoxMotionDet, TextBoxMotionDet1,
                                                TextBoxDischargeTime, TextBoxAZ, TextBoxJudgingCount, TextBoxMinScaleDiv, TextBoxnearZero1,
                                                TextBoxFunctSelection1, TextBoxFunctSelection2,
                                                TextBoxJudgingTIme, TextBoxCompOPTime, TextBoxFilter, TextBoxAvgFilter,
                                                TextBoxKeyInvalidLock, TextBoxWtSatrtTime, TextBoxBagOPTime, TextBoxseqMode1,
                                                TextBoxseqMode2, TextBoxFillingWt, TextBoxDZ, TextBoxNetZero,
                                                TextBoxTare, TextBoxIPselection1, TextBoxIPselection2, TextBoxOPselection1,
                                                TextBoxOPselection2, TextBoxErrOP, TextBoxReserveOP, TextBoxBalWt, TextBoxCapacity,
                                                TextBoxDisplaySelection, TextBoxGraAcceleration, TextBoxNetOver, TextBoxGrossOver,
                                                TextBoxDisplaySelection2, TextBoxZeroTrack1, TextBoxZeroTrack2
                                                        })
                Dim var() As String = {"Upper Limit", "Lower Limit", "Comparision Inhibit Time", "Weighing Function 1",
                     "Weighing Function 2", "Weighing Function 3", "Motion Detection", "Motion Detection2", "Discharge Time",
                       "Auto ZeroCount", "Judging Count", "Min Scale Division", "NearZero", "Extended Function Selection1",
                  "Extended Function Selection2", "Judging Time", "Complete Output Time", "Digital Low Pass Filter", "Moving Avg Filter",
                  "Key Invalid Lock", "Bag Clamp Output Time", "Weighing Start Time", "Sequence mode1", "Sequence mode2",
                    "Filling Promotion Wt", "Dz Regulation Val", "Net Zero", "Tare Setting", "Input Selection 1", "Input Selection 2", "Output Selection 1",
                    "Output Selection 2", "Error Output Selection", "Reserve Output Selection", "Balance Weight Value", "Capacity", "Display Selection 1", "Gravitational Acceleration",
                    "Net Over", "Gross Over", "Display Selection 2", "ZeroTracking", "ZeroTracking2"}

                For I = 1 To var.Length - 1
                    ComboBox4.Items.Add(var(I - 1))
                Next

                FunctionsToCall = {"UpperLimit", "LowerLimit", "ComperisionInhibitTime", "WeighingFunction1", "WeighingFunction2",
                     "WeighingFunction3", "MotionDetection", "MotionDetection2", "DischargeingTime", "AutoZeroCount", "JudgingCount",
                    "MinimumScaleDivision", "NearZero", "ExtendedFunctionSelection1", "ExtendedFunctionSelection2", "JudgingTime", "CompleteOutputTime", "DigitalLowPassFilter",
                    "MovingAvgFilter", "KeyInvalidLock", "BagClampOutputTime", "WeighingStartTime", "Sequencemode1", "Sequencemode2", "FillingPromotionWeight", "DzRegulationVal", "NetZero",
                    "TareSetting", "InputSelection1", "InputSelection2", "OutputSelection1",
                    "OutputSelection2", "ErrorOutputSelection", "ReserveOutputSelection", "BalanceWeightValue", "Capacity", "DisplaySelection1", "GravitationalAccelerationCompensation",
                    "NetOver", "GrossOver", "DisplaySelection2", "ZeroTracking", "ZeroTracking2"}
                Panel3.Visible = True
                Panel2.Visible = False
                Panel1.Visible = False
            End If
        End If

    End Sub

    Private Sub HandleProgressBar(sender As Object, e As ProgressbarEventAgrs)
        Try
            If e.ProgessValue > 17 Then
                Exit Sub
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
    Private Sub RdSPF800(SPDI As Integer, Pkrc As Integer)
        Dim I As Integer
        Dim J As Integer
        Dim S As Integer
        Dim Inbuf As String = ""
        If ComboBox1.Text = "" Or ComboBox2.Text = "" Then Exit Sub
        J = Pkrc
        S = SPDI
        Dim _Packer As New Packer
        Dim _spout As New SpoutController
        ProgressBar1.Value = 0
        ProgressBar1.Visible = True
RepeatTry:

        _Packer = Packers.FirstOrDefault(Function(e) e.DeviceId = J)
        _spout = _Packer.SpoutList.FirstOrDefault(Function(e) e.SpoutId = S)
        Dim Query As QueryInfo = _UniPulse.ChangeCode(_spout.ControllerModel, S)
        PackerCommunication.EnqueueDeviceCommand(Pkrc, S, 16, "Write Code", Query.Query, txtCodeno)
        Dim allMethods = GetType(Unipulse).GetMethods()
        '   Dim validMethods = FunctionsToCall.Select(Function(fn) allMethods.FirstOrDefault(Function(m) m.Name = fn)).Where(Function(m) m IsNot Nothing).ToList()
        Dim validMethods = FunctionsToCall.ToList().Select(Function(fn) allMethods.FirstOrDefault(Function(m) m.Name = fn)).Where(Function(m) m IsNot Nothing).ToList()
        I = 0
        For Each m In validMethods
            Dim txtbox = text2(I)
            Dim ReadParameterQuery As QueryInfo = m.Invoke(_UniPulse, New Object() {_spout.ControllerModel, S, ""})
            PackerCommunication.EnqueueDeviceCommand(Pkrc, S, ReadParameterQuery.ResponseLength, m.Name, ReadParameterQuery.Query, txtbox, ReadParameterQuery.DecimalFormat)
            I += 1
        Next

    End Sub


    Private Sub Button21_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Private Sub Button21_Click_(sender As Object, e As EventArgs) Handles Button21.Click
        Me.Close()
    End Sub

    Private Sub Button11_Click_(sender As Object, e As EventArgs) Handles Button11.Click
        cleartextbox()
        Dim J As Integer
        Dim S As Integer
        If ComboBox1.Text <> "" And ComboBox2.Text <> "" Then
            Button11.Enabled = False
            S = CInt(ComboBox2.Text)
            J = ComboBox1.SelectedIndex + 1
            RdSPF800(S, J)
        End If
        CurrentFormOpened = Me.Name
        Button11.Enabled = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim btn As Button = DirectCast(sender, Button)
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
            _Packer = Packers.FirstOrDefault(Function(Pkr) Pkr.DeviceId = PackerId)
            _spout = _Packer.SpoutList.FirstOrDefault(Function(Pkr) Pkr.SpoutId = SpoutId)

            Dim Query As QueryInfo = _UniPulse.ChangeCode(_spout.ControllerModel, SpoutId)
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

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        Dim i = ComboBox4.SelectedIndex
        Label3.Text = text2(i).Text
    End Sub

    Private Sub ComboBox1_GotFocus(sender As Object, e As EventArgs) Handles ComboBox1.GotFocus
        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()

        For I = 1 To packerCount
            ComboBox1.Items.Add(I)
        Next
        For J = 1 To spoutCount - 1
            ComboBox2.Items.Add(J)
        Next
    End Sub
    Private Sub cleartextbox()
        For i As Integer = 0 To text2.Count - 1
            text2(i).Text = ""
        Next
        ComboBox4.Text = ""
        TextBox26.Text = ""
        Label3.Text = ""
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        HandlePackerType(ComboBox1.Text)
        cleartextbox()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        cleartextbox()
    End Sub


End Class