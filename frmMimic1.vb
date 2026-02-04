Imports System.Data.SqlClient
Imports System.IO
Imports Newtonsoft.Json
Public Class frmMimic1
    Inherits Form
    Public lbl1 As New Label
    Public lbl2 As New Label
    Public lbl3 As New Label
    Public title_1 As New Label
    Public title2 As New Label
    Public titleWrap As New Panel
    Public botLabel1 As New Label
    Public botLabel2 As New Label
    Public leftBottomStack As New Panel
    Public botLabel3 As New Label
    Dim pic As New PictureBox
    Dim txtInput As TextBox
    Dim btn As New Button
    Public leftTopStack As New FlowLayoutPanel
    Public topPanel As New TableLayoutPanel
    Public mainLayout As New TableLayoutPanel
    Dim tab As TabPage
    Public tabControl As New TabControl
    Public tabPage As New TabPage
    Public tablePanel As New TableLayoutPanel
    Public bottomPanel As New TableLayoutPanel
    Public buttonPanel As New FlowLayoutPanel
    Private LastActivity As New Dictionary(Of String, DateTime)
    Dim TPHlabel As New Label
    Dim lblDate As New Label
    Dim lblTime As New Label
    Dim lblShift As New Label
    Dim lblStatus As New Label
    Private outerbuttonList As New List(Of Button)
    Dim StatCntr As Integer
    Dim TotalTonnage As Object = 0
    Private baseFormWidth As Integer
    Private basePicSize As Size
    Private baseTitleFontSize As Single = 20
    Private baseSubtitleFontSize As Single = 18
    Dim countZeroFL As New Dictionary(Of (Integer, Integer), Boolean)
    Dim shift1Start As TimeSpan
    Dim shift1End As TimeSpan
    Dim shift2Start As TimeSpan
    Dim shift2End As TimeSpan
    Dim packerPanel As TableLayoutPanel
    Dim centerPanel As FlowLayoutPanel
    Dim shift3Start As TimeSpan
    Dim shift3End As TimeSpan
    Dim prevspout As Integer
    Dim Err As Integer
    Private ReadOnly _TphLabels As New Dictionary(Of String, Label)()
    Private ReadOnly _QueryLabels As New Dictionary(Of String, ToolStripStatusLabel)()
    ReadOnly _ResponseLabels As New Dictionary(Of String, ToolStripStatusLabel)()
    Private ReadOnly spoutLocks As Object() = Enumerable.Range(0, 20).Select(Function(i) New Object()).ToArray()
    Dim BatchStartTime As DateTime = Now()
    Private _UniPulse As New Unipulse
    Private CurrentShift As String
    Private PrvShift As String
    Private Sub frmMimic1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        baseFormWidth = Me.ClientSize.Width
        basePicSize = pic.Size
        GroupBox1.Visible = False
        design()
        CurrentFormOpened = Me.Name
        Me.DoubleBuffered = True
        '  Panel10.Location = New Point((Me.ClientSize.Width - Panel10.Width) \ 2, (Me.ClientSize.Height - Panel10.Height) \ 2)
        'Panel2.Location = New Point((Me.ClientSize.Width - Panel10.Width) \ 2, (Me.ClientSize.Height - Panel10.Height) \ 2)
        Dim x
        '  Panel10.Visible = True
        Panel2.Visible = False
        x = RestoreSettings(TITLE1, "Properties", "SubTitle")
        If x <> "" Then
            title2.Text = x
        Else
            title2.Text = "Cement Packing System"
        End If
        x = RestoreSettings(TITLE1, "Properties", "CompanyName")
        If x <> "" Then
            title_1.Text = x
        Else
            title_1.Text = "Company name"
        End If

        x = RestoreSettings("MainForm", "LogoPictureBox", "ImagePath")
        If Not String.IsNullOrEmpty(x) AndAlso File.Exists(x) Then
            pic.Image = Image.FromFile(x)
            LOGOSTR = x
        End If
        Dim keys As String() = {"Shft1St", "Shft1End", "Shft2St", "Shft2End", "Shft3St", "Shft3End"}

        For i As Integer = 0 To keys.Length - 1
            x = RestoreSettings(TITLE1, "Properties", keys(i))
            If x <> "" Then
                Select Case i
                    Case 0 : shift1Start = TimeSpan.Parse(x)
                    Case 1 : shift1End = TimeSpan.Parse(x)
                    Case 2 : shift2Start = TimeSpan.Parse(x)
                    Case 3 : shift2End = TimeSpan.Parse(x)
                    Case 4 : shift3Start = TimeSpan.Parse(x)
                    Case 5 : shift3End = TimeSpan.Parse(x)
                End Select
            Else
                Select Case i
                    Case 0 : shift1Start = TimeSpan.Parse("06:00:00")
                    Case 1 : shift1End = TimeSpan.Parse("14:00:00")
                    Case 2 : shift2Start = TimeSpan.Parse("14:00:00")
                    Case 3 : shift2End = TimeSpan.Parse("22:00:00")
                    Case 4 : shift3Start = TimeSpan.Parse("22:00:00")
                    Case 5 : shift3End = TimeSpan.Parse("06:00:00")
                End Select
            End If
        Next

        Application.DoEvents()
        Dim updateTimer As New Timer With {.Interval = 1000}
        AddHandler updateTimer.Tick, Sub()
                                         lblTime.Text = "Time: " & Date.Now.ToString("HH:mm:ss")
                                         lblDate.Text = "Date: " & Date.Now.ToString("dd-MMM-yyyy")
                                         CurrentShift = GetCurrentShiftName()
                                         If CurrentShift <> PrvShift Then
                                             ShitfReset()
                                         End If
                                         lblShift.Text = CurrentShift
                                         PrvShift = CurrentShift
                                         If Not TCPconnflg Then
                                             lblStatus.Text = "Status: Connected"
                                         Else
                                             lblStatus.Text = "Status: Not Connected"
                                             lblStatus.ForeColor = Color.Red
                                         End If
                                     End Sub
        updateTimer.Start()
        autocrtrestore()

        currenttime = DateTime.Now
        PackerCommunication = New CommunicationV1(Packers)
        _checkwire = New CheckWeigher(Packers)
        AddHandler _checkwire.DeviceDataReceived, AddressOf HandleDeviceDataReceived
        AddHandler PackerCommunication.DeviceDataReceived, AddressOf HandleDeviceDataReceived
        AddHandler PackerCommunication.DeviceParameterReceived, AddressOf HandleDeviceParameterReceived
        AddHandler PackerCommunication.ManualWeightReceived, AddressOf HandleManualWeightReceived
        AddHandler PackerCommunication.AutoCorrectDataReceived, AddressOf HandleAutoCrtDataReceived
        AddHandler PackerCommunication.ToolStripUpdate, AddressOf HandleToolStripUpdate
        AddHandler _checkwire.ToolStripUpdate, AddressOf HandleToolStripUpdate
        AddHandler PackerCommunication.UpdateMinicProgressBar, AddressOf HandleMinicProgressBar
        RestoreDaveDetails()

    End Sub
    'Private Sub RestoreDaveDetails()
    '    Dim DirectoryPath As String = $"c:\{Application.ProductName}"
    '    Dim FilePath As String = Path.Combine(DirectoryPath, "Spout.Json")
    '    Dim RestoreSpoutDetails As New List(Of SpoutController)()

    '    Try
    '        If File.Exists(FilePath) Then
    '            Dim JsonString As String = File.ReadAllText(FilePath)
    '            Dim SpoutJsonList As List(Of SpoutController) = JsonConvert.DeserializeObject(Of List(Of SpoutController))(JsonString)
    '            For Each _json As SpoutController In SpoutJsonList
    '                Dim Packer As Packer = Packers.FirstOrDefault(Function(pkr) pkr.DeviceId = _json.PackerId)
    '                For Each _Spout As SpoutController In Packer.SpoutList
    '                    _Spout.PackerId = _json.PackerId
    '                    _Spout.CodeNo = _json.CodeNo
    '                    _Spout.LastDischargedBagWeight = _json.LastDischargedBagWeight
    '                    _Spout.FinalWeight = _json.FinalWeight
    '                    _Spout.Under = _json.Under
    '                    _Spout.Over = _json.Over
    '                    _Spout.BagCount = _json.BagCount
    '                    _Spout.TotalWtTonnes = _json.TotalWtTonnes
    '                    _Spout.Deviation = _json.Deviation
    '                    _Spout.LastDIschargedBagDateTime = _json.LastDIschargedBagDateTime
    '                    _Spout.EmptyRound = _json.EmptyRound
    '                    _Spout.BatchStartTime = _json.BatchStartTime
    '                    UpdateUI(_Spout)
    '                Next
    '            Next
    '        End If
    '    Catch ex As Exception
    '        Console.WriteLine(ex.Message)
    '    End Try

    'End Sub
    Private Sub RestoreDaveDetails()
        Dim DirectoryPath As String = $"C:\{Application.ProductName}"
        Dim FilePath As String = Path.Combine(DirectoryPath, "Spout.Json")

        Try
            If File.Exists(FilePath) Then
                Dim JsonString As String = File.ReadAllText(FilePath)
                Dim SpoutJsonList As List(Of SpoutController) = JsonConvert.DeserializeObject(Of List(Of SpoutController))(JsonString)

                For Each _json As SpoutController In SpoutJsonList
                    Dim Packer As Packer = Packers.FirstOrDefault(Function(pkr) pkr.DeviceId = _json.PackerId)
                    If Packer Is Nothing Then Continue For


                    Dim _Spout As SpoutController = Packer.SpoutList.FirstOrDefault(Function(sp) sp.CodeNo = _json.CodeNo)
                    If _Spout Is Nothing Then Continue For


                    _Spout.PackerId = _json.PackerId
                    _Spout.CodeNo = _json.CodeNo
                    _Spout.LastDischargedBagWeight = _json.LastDischargedBagWeight
                    _Spout.FinalWeight = _json.FinalWeight
                    _Spout.Under = _json.Under
                    _Spout.Over = _json.Over
                    _Spout.BagCount = _json.BagCount
                    _Spout.TotalWtTonnes = _json.TotalWtTonnes
                    _Spout.Deviation = _json.Deviation
                    _Spout.LastDIschargedBagDateTime = _json.LastDIschargedBagDateTime
                    _Spout.EmptyRound = _json.EmptyRound
                    _Spout.BatchStartTime = _json.BatchStartTime

                    UpdateUI(_Spout)
                Next
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub HandleMinicProgressBar(sender As Object, e As ProgressbarEventAgrs)
        Dim ProgressBars As ProgressBar = Nothing
        If ProgressBar.ContainsKey("ProgressBar" & e.PackerId) Then
            ProgressBars = ProgressBar("ProgressBar" & e.PackerId)
        End If
        Try
            If Me.InvokeRequired Then
                Me.Invoke(Sub()
                              If e.ProgessValue < e.MaxValue Then
                                  ProgressBars.Minimum = e.MinValue
                                  ProgressBars.Maximum = e.MaxValue
                                  ProgressBars.Value = e.ProgessValue
                              End If
                          End Sub)
            Else
                ProgressBars.Minimum = e.MinValue
                ProgressBars.Maximum = e.MaxValue
                ProgressBars.Value = e.ProgessValue
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub
    Private Sub HandleAutoCrtDataReceived(sender As Object, e As SpoutController)
        Task.Run(Async Function()
                     Await InsertAutocorrection(e)
                 End Function)
        If ListBox2.InvokeRequired Then
            Me.Invoke(Sub()
                          ListBox2.Items.Add($"Packer-{e.PackerId}")
                          ListBox2.Items.Add($"SpoutNo-{e.SpoutId}")
                          ListBox2.Items.Add($"CurrentCode-{e.CurrentCode}")
                          ListBox2.Items.Add($"OLD_FINALADJUSTMENTWT-{e.FinalAdjustmentWeight}")
                          ListBox2.Items.Add($"NEW_FINALADJUSTMENTWT-{e.RealCorrection:F2}")
                          ListBox2.Items.Add($"Remarks-{e.AutoCorrectionRemarks}")
                          If ListBox2.Items.Count > 10 Then
                              ListBox2.Items.Clear()
                          End If
                      End Sub)
        Else
            ListBox2.Items.Add($"Packer-{e.PackerId}")
            ListBox2.Items.Add($"SpoutNo-{e.SpoutId}")
            ListBox2.Items.Add($"CurrentCode-{e.CurrentCode}")
            ListBox2.Items.Add($"OLD_FINALADJUSTMENTWT-{e.FinalAdjustmentWeight}")
            ListBox2.Items.Add($"NEW_FINALADJUSTMENTWT-{e.RealCorrection:F2}")
            ListBox2.Items.Add($"Remarks-{e.AutoCorrectionRemarks}")
            If ListBox2.Items.Count > 10 Then
                ListBox2.Items.Clear()
            End If
        End If

    End Sub
    Private Sub autocrtrestore()
        Dim x
        For i = 0 To packerCount - 1
            x = RestoreSettings(TITLE1, "FrmAutocrt", "Samples" & i)
            If x <> "" Then
                Integer.TryParse(x, sample(i))
            Else
                sample(i) = 4
            End If
            x = RestoreSettings(TITLE1, "FrmAutocrt", "Coefficient" & i)
            If x <> "" Then
                Double.TryParse(x, coefficient(i))
            Else
                coefficient(i) = 1
            End If
            x = RestoreSettings(TITLE1, "FrmAutocrt", "ValidWeightFrom" & i)
            If x <> "" Then
                Double.TryParse(x, ValidWeightFrom(i))
            Else
                ValidWeightFrom(i) = 49.9
            End If
            x = RestoreSettings(TITLE1, "FrmAutocrt", "ValidWeightTo" & i)
            If x <> "" Then
                Double.TryParse(x, ValidWeightTo(i))
            Else
                ValidWeightTo(i) = 50.1
            End If
            x = RestoreSettings(TITLE1, "FrmAutocrt", "CorrectionRangeFrom" & i)
            If x <> "" Then
                Double.TryParse(x, CorrectionRangeFrom(i))
            Else
                CorrectionRangeFrom(i) = 1.01
            End If
            x = RestoreSettings(TITLE1, "FrmAutocrt", "CorrectionRangeTo" & i)
            If x <> "" Then
                Double.TryParse(x, CorrectionRangeTo(i))
            Else
                CorrectionRangeTo(i) = 1.02
            End If

            x = RestoreSettings(TITLE1, "FrmAutocrt", "CorrectionLimitFrom" & i)
            If x <> "" Then
                Double.TryParse(x, CorrectionLimitFrom(i))
            Else
                CorrectionLimitFrom(i) = 1.01
            End If
            x = RestoreSettings(TITLE1, "FrmAutocrt", "CorrectionLimitTo" & i)
            If x <> "" Then
                Double.TryParse(x, CorrectionLimitTo(i))
            Else
                CorrectionLimitTo(i) = 1.02
            End If

            x = RestoreSettings(TITLE1, "FrmAutocrt", "IsAutoCorrectionEnabled" & i)
            If x <> "" Then
                IsAutoCorrectionEnabled(i) = x
            Else
                IsAutoCorrectionEnabled(i) = True
            End If
        Next
    End Sub
    Private LastTphRun As New Dictionary(Of Integer, DateTime)
    Public Async Function AccDataStoreAsync(_Spout As SpoutController) As Task
        Dim Deviation As Double
        Dim _Packer = Packers.FirstOrDefault(Function(Pkr) Pkr.DeviceId = _Spout.PackerId)
        If _Spout.NewDataFlag = False Then Exit Function
        Try
            Dim weightStr = _Spout.LastDischargedBagWeight
            If weightStr = "" Then Exit Function
            If Not String.IsNullOrEmpty(weightStr) Then
                Dim weightVal = Val(weightStr)

                Deviation = CDbl(_Spout.LastDischargedBagWeight) - _Spout.FinalWeight
                _Spout.Deviation = Deviation
                _Spout.AverageDeviation = (_Spout.AverageDeviation + _Spout.Deviation) / 2
                Dim sql = $"INSERT INTO SpoutData ( CodeNumber, PackerNo, PackerName, SpoutName, SpoutNo, AccWtStr, ActualWt, TargetWt, Deviation, OverWt, UnderWt, Field01, Mode, Current_FinalWt, TimeDtFrmt)
                            VALUES ( @CodeNumber, @PackerNo, @PackerName, @SpoutName, @SpoutNo, @AccWtStr, @ActualWt, @TargetWt, @Deviation, @OverWt, @UnderWt, @Field01, @Mode, @Current_FinalWt, @TimeDtFrmt)"
                Dim parameters As SqlParameter() = {
                    New SqlParameter("@CodeNumber", _Spout.CodeNo),
                    New SqlParameter("@PackerNo", _Spout.PackerId),
                    New SqlParameter("@PackerName", _Spout.PackerName),
                    New SqlParameter("@SpoutName", _Spout.SpoutName),
                    New SqlParameter("@SpoutNo", _Spout.SpoutId),
                    New SqlParameter("@AccWtStr", weightStr),
                    New SqlParameter("@ActualWt", weightStr),
                    New SqlParameter("@TargetWt", _Spout.FinalWeight.ToString("N2")),
                    New SqlParameter("@Deviation", Deviation.ToString("N2")),
                    New SqlParameter("@OverWt", _Spout.Over),
                    New SqlParameter("@UnderWt", _Spout.Under),
                    New SqlParameter("@Field01", _Spout.CPS),
                    New SqlParameter("@Mode", _Spout.DeviceModeType),
                    New SqlParameter("@Current_FinalWt", (_Spout.FinalWeight + _Spout.FinalAdjustmentWeight).ToString("N2")),
                    New SqlParameter("@TimeDtFrmt", DateTime.Now)
                }
                Await helper.ExecuteNonQueryAsync(sql, parameters)

                ' IDLE TIME CALCULATION
                Dim key As String = $"{_Spout.PackerId}_{_Spout.SpoutId}"
                Dim nowTime As DateTime = DateTime.Now
                If LastActivity.ContainsKey(key) Then
                    Dim startTime As DateTime = LastActivity(key)
                    Dim timeDiff As TimeSpan = nowTime - startTime
                    If timeDiff.TotalMinutes > 1 Then
                        Dim idleSql = "INSERT INTO IdleTime (PackerNo, SpoutNo, StartTime, EndTime, TimeDiff)
                                   VALUES (@PackerNo, @SpoutNo, @StartTime, @EndTime, @TimeDiff)"
                        Dim idleParams As SqlParameter() = {
                        New SqlParameter("@PackerNo", _Spout.PackerId),
                        New SqlParameter("@SpoutNo", _Spout.SpoutId),
                        New SqlParameter("@StartTime", startTime),
                        New SqlParameter("@EndTime", nowTime),
                        New SqlParameter("@TimeDiff", CInt(timeDiff.TotalSeconds))
                    }
                        Await helper.ExecuteNonQueryAsync(idleSql, idleParams)

                        'Dim query = "SELECT ROUND((t.TotalBags * 50.00) / 1000, 2) AS TotalTonnage FROM (
                        '    SELECT COUNT(*) AS TotalBags FROM SpoutData WHERE PackerNo=@PackerNo AND TimeDtFrmt BETWEEN DATEADD(HOUR, -1, GETDATE()) AND GETDATE()) t;"
                        'Dim TotalTonnageParams As SqlParameter() = {
                        '    New SqlParameter("@PackerNo", _Spout.PackerId)
                        '}
                        '_Packer.PackerTph = Await helper.ExecuteScalarAsync(query, TotalTonnageParams)

                    End If
                End If


                If Not LastTphRun.ContainsKey(_Spout.PackerId) OrElse
   (DateTime.Now - LastTphRun(_Spout.PackerId)).TotalSeconds >= 60 Then

                    LastTphRun(_Spout.PackerId) = DateTime.Now

                    Dim query = "SELECT ROUND((t.TotalBags * 50.00) / 1000, 2) AS TotalTonnage FROM (
                     SELECT COUNT(*) AS TotalBags FROM SpoutData 
                     WHERE PackerNo=@PackerNo AND TimeDtFrmt BETWEEN DATEADD(HOUR, -1, GETDATE()) AND GETDATE()) t;"
                    Dim TotalTonnageParams As SqlParameter() = {
        New SqlParameter("@PackerNo", _Spout.PackerId)
    }
                    _Packer.PackerTph = Await helper.ExecuteScalarAsync(query, TotalTonnageParams)
                End If


                LastActivity(key) = nowTime
            End If
        Catch ex As Exception
            'Dim _Packer = Packers.FirstOrDefault(Function(Pkr) Pkr.DeviceId = _Spout.PackerId)
            If _Packer IsNot Nothing Then
                If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
                _Packer.ErrorLog.Add($"Error in AccDataStoreAsync for {_Packer.DeviceId} - {_Spout.SpoutId} : {ex.Message}")
            End If
            Console.WriteLine($"SpoutData: {ex.Message}")
        End Try
    End Function

    Private Function GetCurrentShiftName() As String
        Dim nowTime As TimeSpan = DateTime.Now.TimeOfDay

        If nowTime >= shift1Start AndAlso nowTime < shift1End Then
            Return "SHIFT: A"
        ElseIf nowTime >= shift2Start AndAlso nowTime < shift2End Then
            Return "SHIFT: B"
        Else
            If nowTime >= shift3Start OrElse nowTime < shift3End Then
                Return "SHIFT: C"
            End If
        End If

        Return "UNKNOWN"
    End Function


    Public Sub UpdateLabels(packerIndex As Integer, col As Integer, Optional newText As String = "", Optional row As Integer = -1, Optional type As String = "")
        If packerIndex >= 0 AndAlso packerIndex < packerPanels.Count Then
            Dim tablePanel As TableLayoutPanel = packerPanels(packerIndex)
            tablePanel.SuspendLayout()
            For Each ctrl As Control In tablePanel.Controls
                Dim pos As TableLayoutPanelCellPosition = tablePanel.GetPositionFromControl(ctrl)

                ' Case 1: single cell update
                If row >= 0 AndAlso col >= 0 Then
                    If pos.Row = row AndAlso pos.Column = col Then
                        UpdateLabelCell(ctrl, newText, type, tablePanel)
                        Exit For
                    End If

                    ' Case 2: whole column update
                ElseIf row = -1 AndAlso col >= 0 Then
                    If pos.Column = col AndAlso pos.Row > 0 Then
                        UpdateLabelCell(ctrl, newText, type, tablePanel)
                    End If

                    ' Case 3: whole row update
                ElseIf col = -1 AndAlso row >= 0 Then
                    If pos.Row = row AndAlso pos.Column >= 0 Then
                        UpdateLabelCell(ctrl, newText, type, tablePanel)
                    End If
                End If
            Next
            tablePanel.ResumeLayout()
        Else
            Console.WriteLine("Packer panel at index " & packerIndex & " does not exist.")
        End If
    End Sub



    Private Sub HandleDeviceDataReceived(sender As Object, e As SpoutController)
        Task.Run(Async Function()
                     If e.IsConnected = True Then Await AccDataStoreAsync(e)
                 End Function)
        If Me.InvokeRequired Then
            Me.Invoke(Sub()
                          UpdateUI(e)
                      End Sub)
        Else
            UpdateUI(e)
        End If
    End Sub

    Private Sub UpdateLabelCell(ctrl As Control, newText As String, type As String, tablePanel As TableLayoutPanel)
        If ctrl Is Nothing Then Exit Sub
        If ctrl.InvokeRequired Then
            ctrl.Invoke(Sub() UpdateLabelCell(ctrl, newText, type, tablePanel))
            Exit Sub
        End If

        Dim lbl As Label = TryCast(ctrl, Label)
        If lbl IsNot Nothing Then
            Select Case type
                Case "Red"
                    lbl.BackColor = Color.FromArgb(229, 57, 53)
                Case "Green"
                    lbl.BackColor = Color.Green
                Case "White"
                    lbl.BackColor = Color.FromArgb(221, 221, 221)
            End Select

            If newText <> "" Then lbl.Text = newText

            lbl.TextAlign = ContentAlignment.MiddleCenter
            lbl.ImageAlign = ContentAlignment.MiddleCenter
            lbl.AutoSize = False
            lbl.Dock = DockStyle.Fill
        End If
    End Sub


    Private Async Sub HandleManualWeightReceived(sender As Object, e As ManualWeightEventAgrs)
        Dim Message As String = $"Packer No {e.PackerId} SpoutId {e.SpoutId} and weight {e.Weight}"
        If Me.InvokeRequired Then
            Me.Invoke(Sub()
                          If ListBox1.Items.Count > 20 Then ListBox1.Items.Clear()
                          ListBox1.Items.Add(Message)
                      End Sub)
        Else
            If ListBox1.Items.Count > 20 Then ListBox1.Items.Clear()
            ListBox1.Items.Add(Message)
        End If
        ' DATABASE INSERT
        Try
            Dim packerNo As Integer = e.PackerId
            Dim spoutId As Integer = e.SpoutId
            Dim weight As Double = e.Weight
            Dim nowTime As DateTime = e.DateTime

            Dim sql = "INSERT INTO ManualWt (PackerNo, SpoutNo, DateTime, Weight)
                   VALUES (@PackerNo, @SpoutId, @CurrentTime, @Weight)"
            Dim parameters As SqlParameter() = {
            New SqlParameter("@PackerNo", packerNo),
            New SqlParameter("@SpoutId", spoutId),
            New SqlParameter("@CurrentTime", nowTime),
            New SqlParameter("@Weight", weight.ToString("N2"))
        }
            Await helper.ExecuteNonQueryAsync(sql, parameters)

        Catch ex As Exception
            Dim _Packer = Packers.FirstOrDefault(Function(Pkr) Pkr.DeviceId = e.PackerId)
            If _Packer IsNot Nothing Then
                If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
                _Packer.ErrorLog.Add($"Error in HandleManualWeightReceived {_Packer.DeviceId} : {ex.Message}")
            End If
            Console.WriteLine($"Error in HandleManualWeightReceived - {ex.Message}")
        End Try
    End Sub

    Private Sub HandleDeviceParameterReceived(sender As Object, e As ParameterReceivedEventAgrs)
        If Me.InvokeRequired Then
            Me.Invoke(Sub()
                          e.TextBoxRef.Text = e.ParameterValue
                      End Sub)
        Else
            e.TextBoxRef.Text = e.ParameterValue
        End If
    End Sub

    Private Async Function InsertAutocorrection(_spout As SpoutController) As Task
        Try
            Dim query = $"INSERT INTO AUTO_CORRECTION_LOGS (PackerNo, SpoutNo,SampleWeights,FINALWT,OLD_FINALADJUSTMENTWT,NEW_FINALADJUSTMENTWT,TimeDtFrmt,AutoCorrectionRemarks)
                                        VALUES (@PackerNo, @SpoutNo,@SampleWeights,@FINALWT,@OLD_FINALADJUSTMENTWT,@NEW_FINALADJUSTMENTWT, @TimeDtFrmt,@AutoCorrectionRemarks)"
            Dim Qparameters As SqlParameter() = {
                                                    New SqlParameter("@PackerNo", _spout.PackerId),
                                                    New SqlParameter("@SpoutNo", _spout.SpoutId),
                                                    New SqlParameter("@FINALWT", _spout.FinalWeight),
                                                    New SqlParameter("@OLD_FINALADJUSTMENTWT", _spout.FinalAdjustmentWeight),
                                                    New SqlParameter("@NEW_FINALADJUSTMENTWT", _spout.RealCorrection.ToString("F2")),
                                                    New SqlParameter("@SampleWeights", _spout.SampleWeightStr),
                                                    New SqlParameter("@TimeDtFrmt", DateTime.Now),
                                                    New SqlParameter("@AutoCorrectionRemarks", _spout.AutoCorrectionRemarks)
                                                }
            Await helper.ExecuteNonQueryAsync(query, Qparameters)
            _spout.FinalAdjustmentWeight = _spout.RealCorrection
            _spout.SampleWeights.Clear()
            _spout.SampleWeightStr = ""
        Catch ex As Exception
            Dim _Packer = Packers.FirstOrDefault(Function(Pkr) Pkr.DeviceId = _spout.PackerId)
            If _Packer IsNot Nothing Then
                If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
                _Packer.ErrorLog.Add($"Error in InsertAutocorrection {_Packer.DeviceId} : {ex.Message}")
            End If
            Console.WriteLine("Error" & ex.Message)
        End Try
    End Function
    Public Async Sub UpdateUI(_Spout As SpoutController)
        Dim spoutIndex As Integer = CInt(_Spout.SpoutId)
        Dim J As Integer = _Spout.PackerId
        Dim _Packer = Packers.FirstOrDefault(Function(Pkr) Pkr.DeviceId = _Spout.PackerId)
        Dim Tphlabel As Label = Nothing
        Try
            If _TphLabels.TryGetValue($"TPHLabel{J}", Tphlabel) Then
                Tphlabel.Text = $"TPH: {_Packer.PackerTph} MT"
            End If

            Dim DummyPanel As Panel = Nothing
            If ProgressBarPanel.TryGetValue("StatusPanel" & _Packer.DeviceId, DummyPanel) Then
                If DummyPanel IsNot Nothing Then
                    If _Packer.IsInitalParameterDownloaded = True Then
                        If Me.InvokeRequired Then
                            Me.Invoke(Sub()
                                          DummyPanel.Visible = False
                                      End Sub)
                        Else
                            DummyPanel.Visible = False
                        End If
                    End If
                End If
            End If

            For i = 1 To 8
                UpdateLabels(J - 1, -1, type:="White", row:=i)
            Next

            If spoutIndex = 0 Then
            Else
                UpdateLabels(J - 1, 0, $"S{_Spout.SpoutId}", row:=spoutIndex)
            End If
            Dim totalWeightTonn As Double = (_Spout.BagCount * 50.0) / 1000
            _Spout.TotalWtTonnes = totalWeightTonn
            _Spout.BatchStartTime = BatchStartTime
            If _Spout.IsConnected Then
                If _Spout.Under = 0 Then
                    Console.WriteLine()
                End If
                'If _Spout.NewDataFlag Then
                _Spout.NewDataFlag = False
                UpdateLabels(J - 1, -1, type:="White", row:=prevspout)
                UpdateLabels(J - 1, 0, $"S{_Spout.SpoutId}", row:=spoutIndex)
                UpdateLabels(J - 1, 1, _Spout.CurrentCode, row:=spoutIndex)
                If Not String.IsNullOrEmpty(_Spout.LastDischargedBagWeight) Then
                    UpdateLabels(J - 1, 2, _Spout.LastDischargedBagWeight, row:=spoutIndex)
                End If
                UpdateLabels(J - 1, 3, _Spout.FinalWeight, row:=spoutIndex)
                UpdateLabels(J - 1, 4, _Spout.Under, row:=spoutIndex)
                UpdateLabels(J - 1, 5, _Spout.Over, row:=spoutIndex)
                UpdateLabels(J - 1, 6, _Spout.BagCount, row:=spoutIndex)
                UpdateLabels(J - 1, -1, type:="Green", row:=spoutIndex)
                UpdateLabels(J - 1, 7, _Spout.TotalWtTonnes, row:=spoutIndex)
                UpdateLabels(J - 1, 8, _Spout.Deviation.ToString("N2"), row:=spoutIndex)
                UpdateLabels(J - 1, 9, _Spout.datetime, row:=spoutIndex)
                UpdateLabels(J - 1, 10, _Spout.EmptyRound, row:=spoutIndex)
                UpdateLabels(J - 1, 11, BatchStartTime, row:=spoutIndex)
                UpdateLabels(J - 1, 12, _Spout.AccumulationCount, row:=spoutIndex)
                'End If
                If _Spout.BagCount >= 500000 Then _Spout.BagCount = 0
            Else
                If spoutIndex = 0 Then

                Else
                    UpdateLabels(J - 1, 0, $"S{_Spout.SpoutId}", row:=spoutIndex, type:="Red")

                    If prevspout >= 0 AndAlso prevspout <> spoutIndex Then
                        UpdateLabels(J - 1, -1, type:="White", row:=prevspout)
                    End If
                End If
                UpdateLabels(J - 1, 1, "", row:=spoutIndex)
                UpdateLabels(J - 1, 2, "", row:=spoutIndex)
                UpdateLabels(J - 1, 3, "", row:=spoutIndex)
                UpdateLabels(J - 1, 4, "", row:=spoutIndex)
                UpdateLabels(J - 1, 5, "", row:=spoutIndex)
                UpdateLabels(J - 1, 6, "", row:=spoutIndex)
                UpdateLabels(J - 1, 7, "", row:=spoutIndex)
                UpdateLabels(J - 1, 8, "", row:=spoutIndex)
                UpdateLabels(J - 1, 9, "", row:=spoutIndex)
                UpdateLabels(J - 1, 10, "", row:=spoutIndex)
                UpdateLabels(J - 1, 11, "", row:=spoutIndex)
                UpdateLabels(J - 1, 12, "", row:=spoutIndex)
            End If
            Dim Packer As Packer = Packers.FirstOrDefault(Function(Pkr) Pkr.DeviceId = _Spout.PackerId)
            Dim totalBags As Integer = Packer.BagCount
            Dim totalWeightKg As Double = (totalBags * 50.0) / 1000
            If TextBoxDict.ContainsKey($"{J}_textbox1") AndAlso TextBoxDict.ContainsKey($"{J}_textbox0") Then
                TextBoxDict($"{J}_textbox1").Text = totalWeightKg.ToString("#####.##")
                TextBoxDict($"{J}_textbox0").Text = totalBags.ToString()
            End If
            Await Task.Delay(1)
            prevspout = spoutIndex
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

    End Sub
    Private Sub DynamicPictureBox_Click(sender As Object, e As EventArgs)
        Dim picBox As PictureBox = CType(sender, PictureBox)
        Dim dialog As New OpenFileDialog With {
        .Title = "Select Logo Image",
        .Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
    }

        If dialog.ShowDialog() = DialogResult.OK Then
            Try
                Dim logoPath = dialog.FileName
                picBox.Image = Image.FromFile(logoPath)
                LOGOSTR = logoPath
                SaveProfile_Setting("MainForm", "LogoPictureBox", "ImagePath", logoPath)
            Catch ex As Exception
                MessageBox.Show("The selected file is not a valid image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
    Private Sub title2_Click(sender As Object, e As EventArgs)
        Dim X
        Dim REP
        REP = InputBox("Enter the Name of the System", "Sub Title Name", title2.Text)
        If REP <> "" Then
            title2.Text = REP
            X = SaveProfile_Setting(TITLE1, "Properties", "SubTitle", title2.Text)
        End If
    End Sub
    Private Sub title_1_Click(sender As Object, e As EventArgs)
        Dim X
        Dim REP
        REP = InputBox("Enter the Name of the Company", "Company Name", title_1.Text)
        If REP <> "" Then
            title_1.Text = REP
            X = SaveProfile_Setting(TITLE1, "Properties", "CompanyName", title_1.Text)
        End If
    End Sub
    Private Sub ResetPackerCount(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim PackerId As Integer = btn.Tag
        If MsgBox("DO YOU WANT TO RESET THE PACKER?", vbYesNo, "CONFIRMATION") = vbNo Then
            Exit Sub
        Else
            Dim Packer = Packers.FirstOrDefault(Function(Pkr) Pkr.DeviceId = PackerId)
            For Each _Spout As SpoutController In Packer.SpoutList
                Dim ClearQuery As QueryInfo = _UniPulse.Clear(_Spout.ControllerModel, _Spout.SpoutId)
                PackerCommunication.EnqueueDeviceCommand(_Spout.PackerId, _Spout.SpoutId, 0, "Clear", ClearQuery.Query,,, "Write")
                _Spout.BagCount = 0
            Next
            Packer.BagCount = 0
            BatchStartTime = Now
        End If
    End Sub

    Private Sub ShitfReset()
        For Each Packer As Packer In Packers
            For Each _Spout As SpoutController In Packer.SpoutList
                Dim ClearQuery As QueryInfo = _UniPulse.Clear(_Spout.ControllerModel, _Spout.SpoutId)
                PackerCommunication.EnqueueDeviceCommand(_Spout.PackerId, _Spout.SpoutId, 0, "Clear", ClearQuery.Query,,, "Write")
                _Spout.BagCount = 0
            Next
            Packer.BagCount = 0
        Next
        BatchStartTime = Now
    End Sub
    Private Sub ReadAccumulatedCount(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim PackerId As Integer = btn.Tag
        Dim Packer = Packers.FirstOrDefault(Function(Pkr) Pkr.DeviceId = PackerId)
        For Each _Spout As SpoutController In Packer.SpoutList
            Dim Query As QueryInfo = _UniPulse.ChangeCode(_Spout.ControllerModel, _Spout.SpoutId, _Spout.CurrentCode.ToString())
            PackerCommunication.EnqueueDeviceCommand(PackerId, _Spout.SpoutId, 16, "Write Code", Query.Query,,, "Write")
            Dim ReadQuery As QueryInfo = _UniPulse.ReadCount(_Spout.ControllerModel, _Spout.SpoutId)
            PackerCommunication.EnqueueDeviceCommand(PackerId, _Spout.SpoutId, 16, "ReadCount", ReadQuery.Query,, ReadQuery.DecimalFormat, "Read")
        Next
    End Sub

    Private Sub DynamicButton_Click(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim txtInput As TextBox = TryCast(btn.Tag, TextBox)
        Select Case btn.Text
            Case "Status Screen"
                frmStatus.ShowDialog()
            Case "Report"
                FrmReports.ShowDialog()
            Case "Logs"
                LogForm.ShowDialog()
            Case "Show Manual weight"
                GroupBox1.Location = New Point((Me.ClientSize.Width - GroupBox1.Width) \ 2, (Me.ClientSize.Height - GroupBox1.Height) \ 2)
                GroupBox1.Visible = True
            Case "AutoCorrection"
                Panel2.Visible = True
        End Select
    End Sub
    Private Const BaseWidth As Integer = 1920
    Private Const BaseHeight As Integer = 1080
    Private baseFontSize As Single = 12
    Private resizeTargets As New List(Of Control)
    Private Function GetScaledFontSize() As Single
        Dim currentWidth As Integer = Me.ClientSize.Width
        Dim currentHeight As Integer = Screen.PrimaryScreen.Bounds.Height
        Dim scaleX As Single = currentWidth / BaseWidth
        Dim scaleY As Single = currentHeight / BaseHeight
        Dim scale As Single = (scaleX + scaleY) / 2
        Return baseFontSize * scale
    End Function


    Public Sub design()
        Me.Text = "frmMimic1"
        Me.WindowState = FormWindowState.Maximized
        Me.Font = New Font("Microsoft Sans Serif", 10, FontStyle.Bold)
        Me.BackColor = Color.LightGray
        Me.SuspendLayout()
        ' === MAIN LAYOUT ===
        mainLayout = New TableLayoutPanel With {
                                                .Dock = DockStyle.Fill,
                                                .RowCount = 3
                                            }
        mainLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 15)) ' Top
        mainLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 80)) ' Center
        mainLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 20)) ' Bottom
        Me.Controls.Add(mainLayout)

        ' === TOP ===
        Dim topWrapper As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 3
        }
        topWrapper.BackColor = Color.DarkGray ' Top Panel
        topWrapper.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25))  ' Left spacer
        topWrapper.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))     ' Center logo+title
        topWrapper.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30))  ' Right spacer
        mainLayout.Controls.Add(topWrapper, 0, 0)
        ' === PictureBox ===
        pic = New PictureBox With {
                                    .SizeMode = PictureBoxSizeMode.Zoom,
                                    .Size = New Size(180, 80),
                                    .Margin = New Padding(10)
                                }
        AddHandler pic.Click, AddressOf DynamicPictureBox_Click
        ' === Text Panel ===
        Dim textPanel As New TableLayoutPanel With {
                                                .RowCount = 2,
                                                .ColumnCount = 1,
                                                .AutoSize = True,
                                                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                                .Dock = DockStyle.Fill
                                            }
        textPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 30))
        textPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 30))
        textPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))

        title_1 = New Label With {
                            .Text = "ULTRATECH",
                            .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() + 7, FontStyle.Bold),
                            .AutoSize = True,
                            .TextAlign = ContentAlignment.MiddleCenter,
                            .Margin = New Padding(5),
                            .Dock = DockStyle.Fill
                        }

        title2 = New Label With {
                            .Text = "CEMENT PACKING SYSTEM",
                            .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() + 3, FontStyle.Regular),
                            .AutoSize = True,
                            .TextAlign = ContentAlignment.MiddleCenter,
                           .Margin = New Padding(5, 0, 0, 8),
                            .Dock = DockStyle.Fill
                        }

        title_1.ForeColor = Color.Black
        textPanel.Controls.Add(title_1, 0, 0)
        textPanel.Controls.Add(title2, 0, 1)

        topWrapper.Controls.Add(pic, 0, 0)
        topWrapper.Controls.Add(textPanel, 1, 0)
        mainLayout.Controls.Add(topWrapper, 0, 0)

        AddHandler title_1.Click, AddressOf title_1_Click
        AddHandler title2.Click, AddressOf title2_Click
        ' === Right Info Panel ===
        Dim infoPanel As New TableLayoutPanel With {
                                                    .RowCount = 5,
                                                    .AutoSize = True,
                                                    .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                                    .Height = 20,
                                                    .Width = 40,
                                                    .Margin = New Padding(4, 0, 1, 4),
                                                    .Anchor = AnchorStyles.Right}

        infoPanel.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        infoPanel.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        infoPanel.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        infoPanel.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        infoPanel.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        lblShift = New Label With {.Text = "Shift: A", .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() + 2, FontStyle.Bold), .AutoSize = True}
        lblDate = New Label With {.Text = "Date: " & Date.Now.ToString("dd-MMM-yyyy"), .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() + 2, FontStyle.Bold), .AutoSize = True, .TextAlign = ContentAlignment.BottomRight}
        lblTime = New Label With {.Text = "Time: " & Date.Now.ToString("HH:mm:ss"), .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() + 2, FontStyle.Bold), .AutoSize = True, .TextAlign = ContentAlignment.BottomRight}
        lblStatus = New Label With {.Text = "Status:", .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() + 2, FontStyle.Bold), .ForeColor = Color.Green, .AutoSize = True, .TextAlign = ContentAlignment.BottomRight}
        infoPanel.Controls.Add(lblShift)
        infoPanel.Controls.Add(lblDate)
        infoPanel.Controls.Add(lblTime)
        infoPanel.Controls.Add(lblStatus)

        Dim rightWrapper As New FlowLayoutPanel With {
                                                    .FlowDirection = FlowDirection.RightToLeft,
                                                    .AutoSize = True,
                                                    .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                                    .Dock = DockStyle.Fill,
                                                    .Anchor = AnchorStyles.Top Or AnchorStyles.Right
                                                  }

        rightWrapper.Controls.Add(infoPanel)
        topWrapper.Controls.Add(rightWrapper, 2, 0)

        ' === CENTER  ===
        tabControl = New TabControl With {
        .Left = 10,
        .Top = 10,
        .Width = ClientSize.Width * 0.99,
        .Height = ClientSize.Height * 0.68
        }

        For j = 1 To CInt(Packers.Count)
            Dim PackerId = j
            Dim _Packer As Packer = Packers.FirstOrDefault(Function(e) e.DeviceId = PackerId)
            Dim _TotalSpout = _Packer.SpoutList.Count

            packerPanel = New TableLayoutPanel With {
                .BackColor = Color.DarkGray,
                .RowCount = 4,
                .ColumnCount = 1,
                .Margin = New Padding(5),
                .Dock = DockStyle.Fill
            }
            packerPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 10))  ' Label
            packerPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 100)) ' Table
            packerPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 8))   ' TextBoxes
            tab = New TabPage("Packer " & j)

            Dim headerPanel As New TableLayoutPanel With {
                .ColumnCount = 2,
                .Dock = DockStyle.Fill,
                .AutoSize = True
             }
            headerPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
            headerPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40))
            Dim packerLabel As New Label With {
                    .Name = "PackerLabel" & j,
                    .Text = "Packer-" & j,
                    .AutoSize = True,
                    .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() + 2, FontStyle.Bold),
                    .TextAlign = ContentAlignment.MiddleRight,
                    .Dock = DockStyle.Fill
}

            TPHlabel = New Label With {
                                        .Name = "TPHLabel" & j,
                                        .Text = "TPH-" & TotalTonnage.ToString(),
                                        .AutoSize = True,
                                        .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() + 5, FontStyle.Bold),
                                        .TextAlign = ContentAlignment.MiddleRight,
                                        .Dock = DockStyle.Fill
                                    }

            _TphLabels.Add(TPHlabel.Name, TPHlabel)
            headerPanel.Controls.Add(packerLabel, 0, 0)
            headerPanel.Controls.Add(TPHlabel, 1, 0)
            packerPanel.Controls.Add(headerPanel, 0, 0)
            ' === Table Layout ===
            Dim tablePanel As New DoubleBufferedTableLayoutPanel With {
                                                                        .Dock = DockStyle.Fill,
                                                                        .RowCount = spoutCount,
                                                                        .ColumnCount = 13,
                                                                        .CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial,
                                                                        .BackColor = Color.FromArgb(221, 221, 221)
                                                                    }
            packerPanels.Add(tablePanel)
            For r = 0 To _TotalSpout + 1
                tablePanel.RowStyles.Add(New RowStyle(SizeType.Percent, 100 / (spoutCount + 1)))
            Next
            For c = 0 To 12
                tablePanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20))
            Next

            Dim headings() As String = {"Spout No", "Sort No", "Actual Wt", "Target Wt", "Under Wt", "Over Wt", "No Of Bags", "Total Wt Tonnes", "Deviation", "Time", "Empty Rotation", "Batch StartTime", "Accumulation Count"}
            For c = 0 To headings.Length - 1
                Dim lbl As New Label With {
                                        .Text = headings(c),
                                        .Font = New Font("Microsoft Sans Serif", GetScaledFontSize, FontStyle.Bold),
                                        .TextAlign = ContentAlignment.MiddleCenter,
                                        .Dock = DockStyle.Fill
                                    }
                tablePanel.Controls.Add(lbl, c, 0)
            Next

            For r = 1 To _TotalSpout
                For c = 0 To 12

                    Dim bgColor As Color = Color.FromArgb(241, 239, 236) 'If(r Mod 2 = 0, Color.FromArgb(221, 221, 221), Color.FromArgb(241, 239, 236)) 'FromArgb(255, 224, 178)
                    Dim lbl As New Label With {
                            .Name = $"Row{r}Col{c + 1}",
                            .Text = "",
                            .AutoSize = True,
                            .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() - 1, FontStyle.Bold),
                            .TextAlign = ContentAlignment.MiddleCenter,
                            .Height = 5,
                            .Width = 5,
                            .Margin = New Padding(2),
                            .BackColor = bgColor
                        }
                    resizeTargets.Add(lbl)
                    tablePanel.Controls.Add(lbl, c, r)
                Next
            Next
            packerPanel.Controls.Add(tablePanel, 0, 1)

            ' === TextBoxes ===
            Dim localtxtPanel As New FlowLayoutPanel With {
                                                        .Dock = DockStyle.Fill,
                                                        .FlowDirection = FlowDirection.LeftToRight,
                                                        .WrapContents = False,
                                                        .AutoSize = True
                                                    }
            Dim txtval() As String = {"", ""}

            For z = 0 To txtval.Length - 1
                Dim key As String = $"{j}_textbox{z}"
                Dim txtInput As New TextBox With {
                        .Text = txtval(z),
                        .Width = 120,
                        .Height = 30,
                        .Font = New Font("Microsoft Sans Serif", GetScaledFontSize, FontStyle.Bold),
                        .TextAlign = HorizontalAlignment.Center,
                        .ForeColor = Color.Black,
                        .Name = $"{j}_textbox{z}"
                    }
                TextBoxDict.Add(key, txtInput)
                localtxtPanel.Controls.Add(txtInput)
            Next
            Dim ResetButton As New Button With {
                    .Text = "Reset",
                    .AutoSize = True,
                    .Tag = j,
                    .Margin = New Padding(0, 0, 0, 8),
                    .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() - 2, FontStyle.Bold),
                    .BackColor = Color.LightGray,
                    .ForeColor = Color.Black,
                    .FlatStyle = FlatStyle.Flat
            } '
            AddHandler ResetButton.Click, AddressOf ResetPackerCount
            localtxtPanel.Controls.Add(ResetButton)
            packerPanel.Controls.Add(localtxtPanel, 0, 2)

            Dim ReadCountButton As New Button With {
                    .Text = "Read Accumulation",
                    .Tag = j,
                    .Margin = New Padding(2, 0, 0, 8),
                    .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() - 2, FontStyle.Bold),
                    .BackColor = Color.LightGray,
                    .ForeColor = Color.Black,
                    .FlatStyle = FlatStyle.Flat,
                    .AutoSize = True
            } '
            AddHandler ReadCountButton.Click, AddressOf ReadAccumulatedCount
            localtxtPanel.Controls.Add(ReadCountButton)
            packerPanel.Controls.Add(localtxtPanel, 0, 3)

            ' === Buttons ===
            Dim localButtonPanel As New FlowLayoutPanel With {
                .Dock = DockStyle.Fill,
                .FlowDirection = FlowDirection.LeftToRight,
                .WrapContents = False,
                .AutoSize = True
            }

            Dim panels As New Panel With {
                .Height = 90,
                .Name = "StatusPanel" & j,
                .Width = 500,
                .Visible = True,
                .BackColor = Color.Transparent,
                .Padding = New Padding(80, 0, 90, 0)
            }
            panels.Location = New Point((tabControl.Width - panels.Width) \ 2,
                                        (tabControl.Height - panels.Height) \ 2)



            Dim progressBars As New ProgressBar With {
                .Name = "ProgressBar" & j,
                .Height = 30,
                .Width = panels.Width,
                .Dock = DockStyle.Bottom
            }
            Dim panellabel As New Label With {
                .Text = "Loading Initial Parameters For Each Packer," & vbCrLf &
                        "Please Wait to complete the action!!",
                .TextAlign = ContentAlignment.MiddleCenter,
                .AutoSize = True,
                .Padding = New Padding(80, 0, 90, 0)
            }
            panels.Controls.Add(panellabel)
            panels.Controls.Add(progressBars)
            ProgressBar.Add(progressBars.Name, progressBars)
            ProgressBarPanel.Add(panels.Name, panels)
            tab.Controls.Add(panels)
            tab.Controls.Add(packerPanel)
            tabControl.TabPages.Add(tab)
        Next
        mainLayout.Controls.Add(tabControl, 0, 1)

        ' === BOTTOM ===
        bottomPanel = New TableLayoutPanel With {
                                                    .Dock = DockStyle.Fill,
                                                    .ColumnCount = 3,
                                                    .RowCount = 2
                                                }
        bottomPanel.RowStyles.Add(New RowStyle(SizeType.AutoSize)) ' Row 0 for buttons
        bottomPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 100)) ' Row 1 for text/image
        bottomPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33))
        bottomPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33))
        bottomPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33))
        mainLayout.Controls.Add(bottomPanel, 0, 2)

        Dim btn2 As New Button With {
                .Text = "Status Screen",
                .Width = 150,
                .Height = 30,
                 .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() - 2, FontStyle.Bold),
                .BackColor = Color.LightGray,
                .ForeColor = Color.Black,
                .FlatStyle = FlatStyle.Flat
            }

        Dim btn3 As New Button With {
                .Text = "Report",
                .Width = 150,
                .Height = 30,
                 .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() - 2, FontStyle.Bold),
                .BackColor = Color.LightGray,
                .ForeColor = Color.Black,
                .FlatStyle = FlatStyle.Flat
            }

        Dim btn5 As New Button With {
                    .Text = "Show Manual weight",
                    .Width = 150,
                    .Height = 30,
                     .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() - 2, FontStyle.Bold),
                    .BackColor = Color.LightGray,
                    .ForeColor = Color.Black,
                    .FlatStyle = FlatStyle.Flat
                }

        Dim Logbutton As New Button With {
                    .Text = "Logs",
                    .Width = 150,
                    .Height = 30,
                     .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() - 2, FontStyle.Bold),
                    .BackColor = Color.LightGray,
                    .ForeColor = Color.Black,
                    .FlatStyle = FlatStyle.Flat
                }
        Dim Autocrtbtn As New Button With {
                .Text = "AutoCorrection",
                .Width = 150,
                .Height = 30,
                 .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() - 2, FontStyle.Bold),
                .BackColor = Color.LightGray,
                .ForeColor = Color.Black,
                .FlatStyle = FlatStyle.Flat
            }
        AddHandler btn2.Click, AddressOf DynamicButton_Click
        AddHandler btn3.Click, AddressOf DynamicButton_Click
        AddHandler btn5.Click, AddressOf DynamicButton_Click
        AddHandler Autocrtbtn.Click, AddressOf DynamicButton_Click
        AddHandler Logbutton.Click, AddressOf DynamicButton_Click

        outerbuttonList.Add(btn2)
        outerbuttonList.Add(btn3)
        outerbuttonList.Add(btn5)
        outerbuttonList.Add(Logbutton)
        outerbuttonList.Add(Autocrtbtn)
        ' === Panel to hold buttons ===
        Dim buttonPanel As New FlowLayoutPanel With {
                .FlowDirection = FlowDirection.LeftToRight,
                .Dock = DockStyle.Fill,
                .AutoSize = True
            }
        buttonPanel.Controls.AddRange(New Control() {btn2, btn3, btn5, Logbutton, Autocrtbtn})

        ' === Add buttonPanel to bottomPanel ===
        bottomPanel.Controls.Add(buttonPanel, 0, 0)
        bottomPanel.SetColumnSpan(buttonPanel, 3)

        botLabel2 = New Label With {.Text = "Designed and Developed by " & "Accsys Electronics,India-Chennai", .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() - 2, FontStyle.Bold), .Dock = DockStyle.Bottom, .Margin = New Padding(40, 0, 0, 0)}
        leftBottomStack = New Panel With {.Dock = DockStyle.Fill, .Margin = New Padding(0, 0, 0, 40)}
        ' leftBottomStack.Controls.Add(botLabel1)
        leftBottomStack.Controls.Add(botLabel2)
        bottomPanel.Controls.Add(leftBottomStack, 0, 1)

        bottomPanel.BackColor = Color.DarkGray
        botLabel3 = New Label With {.Text = LastModifiedStr, .Font = New Font("Microsoft Sans Serif", GetScaledFontSize() - 2, FontStyle.Bold), .TextAlign = ContentAlignment.BottomRight, .Dock = DockStyle.Bottom, .Margin = New Padding(0, 0, 0, 50)}
        bottomPanel.Controls.Add(botLabel3, 1, 1)
        ' === Image & Settings ===
        Dim LogoPath As String
        LogoPath = Application.StartupPath & "\logo.jpg"
        If LogoPath <> "" Then
            If Dir$(LogoPath) <> "" Then
                pic.Image = Image.FromFile(LogoPath)
                LOGOSTR = LogoPath
            End If
        End If
        LogoPath = Application.StartupPath & "\logo1.jpg"
        If LogoPath <> "" Then
            If Dir$(LogoPath) <> "" Then
                pic.Image = Image.FromFile(LogoPath)
                LOGOSTR = LogoPath
            End If
        End If

        Dim QueryW As Integer = (ClientSize.Width / CInt(Packers.Count)) * 0.2
        Dim ResponseW As Integer = (ClientSize.Width / CInt(Packers.Count)) * 0.7
        For i = 1 To CInt(Packers.Count)
            Dim ReceivedDataLabel As New ToolStripStatusLabel() With {
           .Name = $"PackerReceivedQuery{i}",
           .Text = $"Query{i}",
           .Spring = False,
           .AutoSize = False,
           .Width = QueryW
           }
            StatusStrip1.Items.Add(ReceivedDataLabel)

            _QueryLabels.Add(ReceivedDataLabel.Name, ReceivedDataLabel)
            Dim separator As New ToolStripSeparator()
            StatusStrip1.Items.Add(separator)

            Dim ResponseSentLabel As New ToolStripStatusLabel() With {
            .Name = $"ResponseSent{i}",
            .Text = $"Response{i}",
            .Spring = False,
            .AutoSize = False,
            .Width = ResponseW
            }
            StatusStrip1.Items.Add(ResponseSentLabel)
            _ResponseLabels.Add(ResponseSentLabel.Name, ResponseSentLabel)
            Dim separatorend As New ToolStripSeparator()
            StatusStrip1.Items.Add(separatorend)
        Next
        Me.ResumeLayout()
    End Sub

    Private Sub HandleToolStripUpdate(sender As Object, e As ToolStripEventArgs)
        If Me.InvokeRequired Then
            Me.Invoke(Sub()
                          UpdateToolStrip(e)
                      End Sub)
        Else
            UpdateToolStrip(e)
        End If
    End Sub
    Private Sub UpdateToolStrip(e As ToolStripEventArgs)
        Dim deviceIdStr As String = $"{e.PackerId}" ' Use the string DeviceId from EventArgs
        Try
            Me.SuspendLayout()

            If e.MessageType = "Query" Then
                Dim lbl As ToolStripStatusLabel = Nothing
                If _QueryLabels.TryGetValue($"PackerReceivedQuery{deviceIdStr}", lbl) Then
                    lbl.Text = e.Message
                End If
            Else
                Dim lbl1 As ToolStripStatusLabel = Nothing
                If _ResponseLabels.TryGetValue($"ResponseSent{deviceIdStr}", lbl1) Then
                    lbl1.Text = e.Message
                End If
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Me.ResumeLayout(True)
        End Try
    End Sub
    Private Sub frmMimic1_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        PackerCommunication.StopCommunication()
        _checkwire.StopCommunication()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        GroupBox1.Visible = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListBox1.Items.Clear()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Panel2.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Dim weights As New List(Of Double) From {50.1, 50.2, 50.3, 50.4}
        Dim Spout As New SpoutController With {
            .PackerId = 2,
            .SpoutId = 1,
            .SampleWeightStr = "50.1, 50.2, 50.3, 50.4",
            .RealCorrection = 0,
            .AutoCorrectionRemarks = "Not Corrected..........................",
            .FinalAdjustmentWeight = 1
            }
        InsertAutocorrection(Spout)
    End Sub
End Class
Public Class DoubleBufferedTableLayoutPanel
    Inherits TableLayoutPanel
    Public Sub New()
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        Me.UpdateStyles()
    End Sub
End Class
