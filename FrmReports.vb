Imports System.Data.SqlClient
Public Class FrmReports
    Dim isLoading As Boolean
    Dim tempTable As New DataTable
    Dim View As Boolean
    Dim excel As Boolean
    Dim pdf As Boolean
    Dim Rep As Report
    Dim header() As String
    Dim title As String = ""
    Dim ReportType As String = "DateWise"
    Dim CurrentSelection As String
    Dim ChkFl As Boolean
    Private Sub Reports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim FormWidth = ClientSize.Width
        Dim FormHeight = ClientSize.Height
        Dim XValue As Double
        SaveOriginalBounds(Me)
        ResizeLayout()
        Label1.Left = (FormWidth - Label1.Width) / 2
        CheckBox1.Left = Label1.Left - 10
        XValue = (FormWidth - (Panel1.Width + Panel3.Width)) / 2
        Panel1.Width = (FormWidth - 60) \ 2
        Panel3.Width = Panel1.Width - 30

        Panel1.Left = XValue
        Panel3.Left = Panel1.Left + (Panel1.Width) + 30

        DataGridView1.Left = Panel1.Left
        DataGridView1.Width = Panel3.Width * 1.8
        DataGridView1.Top = Panel1.Height + Panel1.Top + 50

        Button3.Top = DataGridView1.Bottom + 20
        Button3.Left = DataGridView1.Right - Button3.Width

        DateTimePicker1.Value = DateTime.Now
        DateTimePicker2.Value = DateTime.Now
        DateTimePicker3.Value = DateTime.Now.ToString("dd-MM-yyyy 00:00:00")
        DateTimePicker4.Value = DateTime.Now.ToString("dd-MM-yyyy 23:59:59")
        Label5.Left = Label1.Left
        Label5.Top = DataGridView1.Top - Label5.Height - 5
        CmboPack.Items.Clear()
        For Each Packer As Packer In Packers
            CmboPack.Items.Add(Packer.DeviceId)
        Next
        If Packers.Count > 0 Then CmboPack.SelectedIndex = 0
        DateWiseRadioBtn.Checked = True
        Me.BeginInvoke(Sub()
                           isLoading = True
                           RadioButton2.Checked = True
                           RadioButton3.Checked = False
                           RadioButton6.Checked = False
                           RadioButton5.Checked = False
                           RadioButton11.Checked = False
                           isLoading = False

                       End Sub)
        If CheckBox1.Checked Then
            ChkFl = True
        End If
        PictureBox1.Top = (FormHeight - PictureBox1.Height) / 2
        PictureBox1.Left = (FormWidth - PictureBox1.Width) / 2
        PictureBox1.Visible = False
        ResizeLayout()

    End Sub
    ' Base design resolution
    Private ReadOnly BaseFormWidth As Integer = 1800
    Private ReadOnly BaseFormHeight As Integer = 1080

    ' Store original bounds for all controls
    Private OriginalBounds As New Dictionary(Of Control, Rectangle)
    Private OriginalFonts As New Dictionary(Of Control, Single)



    Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        ResizeLayout()
    End Sub

    Private Sub SaveOriginalBounds(parent As Control)
        For Each ctrl As Control In parent.Controls
            If Not OriginalBounds.ContainsKey(ctrl) Then
                OriginalBounds(ctrl) = ctrl.Bounds
                OriginalFonts(ctrl) = ctrl.Font.Size
            End If
            If ctrl.HasChildren Then
                SaveOriginalBounds(ctrl)
            End If
        Next
    End Sub


    Private Sub ResizeControls(container As Control, scaleX As Double, scaleY As Double)
        If Me.WindowState = FormWindowState.Minimized Then Exit Sub
        For Each ctrl As Control In container.Controls
            If OriginalBounds.ContainsKey(ctrl) Then
                Dim orig = OriginalBounds(ctrl)
                ctrl.Left = CInt(orig.Left * scaleX)
                ctrl.Top = CInt(orig.Top * scaleY)
                ctrl.Width = CInt(orig.Width * scaleX)
                ctrl.Height = CInt(orig.Height * scaleY)
                Dim origFontSize = OriginalFonts(ctrl)
                ctrl.Font = New Font(ctrl.Font.FontFamily, CSng(origFontSize * scaleY), ctrl.Font.Style)
            End If
        Next
    End Sub

    Private Sub ResizeLayout()
        If Panel1 Is Nothing OrElse Panel3 Is Nothing Then Exit Sub

        ' Scale Panel1
        Panel1.Width = CInt(435 * (ClientSize.Width / BaseFormWidth))
        Panel1.Height = CInt(203 * (ClientSize.Height / BaseFormHeight))
        ResizeControls(Panel1, ClientSize.Width / BaseFormWidth, ClientSize.Height / BaseFormHeight)

        ' Scale Panel3 (or AutoSize if you prefer)
        Panel3.Width = CInt(1009 * (ClientSize.Width / BaseFormWidth))
        Panel3.Height = CInt(203 * (ClientSize.Height / BaseFormHeight))
        ResizeControls(Panel3, ClientSize.Width / BaseFormWidth, ClientSize.Height / BaseFormHeight)

        ' Fixed gap between them
        Dim gap As Integer = 50
        Dim totalWidth As Integer = Panel1.Width + gap + Panel3.Width

        ' --- Horizontal centering only ---
        Dim startX As Integer = (ClientSize.Width - totalWidth) \ 2
        Dim topY As Integer = 50 ' keep them near top (adjust as you like)

        ' Place them
        Panel1.Left = startX
        Panel1.Top = topY

        Panel3.Left = Panel1.Right + gap
        Panel3.Top = topY
    End Sub


    Private Sub RadioButton_CheckedChanged(sender As Object, e As EventArgs) _
    Handles RadioButton1.CheckedChanged, RadioButton2.CheckedChanged, RadioButton3.CheckedChanged, RadioButton5.CheckedChanged,
    RadioButton6.CheckedChanged, RadioButton11.CheckedChanged, DateWiseRadioBtn.CheckedChanged, ShiftWiseRadioBtn.CheckedChanged
        If isLoading Then Exit Sub
        DataGridView1.DataSource = Nothing
        Dim rb As RadioButton = CType(sender, RadioButton)
        If rb.Checked Then
            Select Case rb.Name
                Case "DateWiseRadioBtn"
                    ReportType = "DateWise"
                    Panel2.Visible = False
                    Label4.Visible = True
                    DateTimePicker1.Visible = True
                    DateTimePicker3.Visible = True
                    DateTimePicker2.Visible = True
                    DateTimePicker4.Visible = True
                Case "ShiftWiseRadioBtn"
                    ReportType = "ShiftWise"
                    Label4.Visible = False
                    DateTimePicker1.Visible = True
                    DateTimePicker3.Visible = False
                    DateTimePicker2.Visible = False
                    DateTimePicker4.Visible = False
                    Panel2.Visible = True
                Case "RadioButton1"
                    Label5.Text = "Auto Correction Report"
                    CmboPack.Visible = True
                    Label9.Visible = True
                    Cmbospout.Visible = True
                    CurrentSelection = "Auto Correction Report"
                Case "RadioButton2"
                    Label5.Text = "Individual Report"
                    CmboPack.Visible = True
                    Label9.Visible = True
                    Cmbospout.Visible = True
                    CurrentSelection = "Individual Report"
                Case "RadioButton3"
                    Label5.Text = "Percentage Report"
                    CmboPack.Visible = True
                    Label9.Visible = False
                    Cmbospout.Visible = False
                    CurrentSelection = "Percentage Report"
                Case "RadioButton5"
                    Label5.Text = "Spout Wise Report"
                    CmboPack.Visible = True
                    Label9.Visible = False
                    Cmbospout.Visible = False
                    CurrentSelection = "Spout Wise Report"
                Case "RadioButton6"
                    Label5.Text = "Weight Wise Report"
                    CmboPack.Visible = True
                    Label9.Visible = False
                    Cmbospout.Visible = False
                    CurrentSelection = "Weight Wise Report"
                Case "RadioButton11"
                    Label5.Text = "Manual Weight Report"
                    CmboPack.Visible = True
                    Label9.Visible = True
                    Cmbospout.Visible = True
                    CurrentSelection = "Manual Weight Report"
            End Select
        End If
    End Sub

    Private sub HandleSpinner(StatusValue  as Boolean)
        If Me.InvokeRequired Then
            Me.Invoke(Sub()
                          If StatusValue Then
                              PictureBox1.Visible = True
                              PictureBox1.BringToFront()
                          Else
                              PictureBox1.Visible = False
                          End If

                      End Sub)
        Else
            If StatusValue Then
                PictureBox1.Visible = True
                PictureBox1.BringToFront()
            Else
                PictureBox1.Visible = False
            End If
        End If
    End sub


    Private Async Sub PrepareQuery()
        Dim PackerId = CmboPack.SelectedIndex + 1
        Dim SpoutId = Cmbospout.SelectedIndex
        Dim dateFrom As Date = DateTimePicker1.Value.Date
        Dim dateTo As Date = DateTimePicker2.Value.Date
        Dim timeFrom As TimeSpan = DateTimePicker3.Value.TimeOfDay
        Dim timeTo As TimeSpan = DateTimePicker4.Value.TimeOfDay
        Dim fromDateTime As DateTime
        Dim toDateTime As DateTime
        Dim _Packer As New Packer
        Dim _spout As New SpoutController
        Dim query
        Dim parameters As New Dictionary(Of String, Object)

        If ReportType = "ShiftWise" Then
            If RadioButton10.Checked Then ' Shift A
                fromDateTime = dateFrom.Add(TimeSpan.Parse(GetShiftTimeSetting("Shft1St", "06:00:00")))
                toDateTime = dateFrom.Add(TimeSpan.Parse(GetShiftTimeSetting("Shft1End", "14:00:00")))
            ElseIf RadioButton9.Checked Then ' Shift B
                fromDateTime = dateFrom.Add(TimeSpan.Parse(GetShiftTimeSetting("Shft2St", "14:00:00")))
                toDateTime = dateFrom.Add(TimeSpan.Parse(GetShiftTimeSetting("Shft2End", "22:00:00")))
            ElseIf RadioButton8.Checked Then ' Shift C
                fromDateTime = dateFrom.Add(TimeSpan.Parse(GetShiftTimeSetting("Shft3St", "22:00:00")))
                toDateTime = dateFrom.AddDays(1).Add(TimeSpan.Parse(GetShiftTimeSetting("Shft3End", "06:00:00")))
            End If
        ElseIf ReportType = "DateWise" Then
            fromDateTime = dateFrom.Add(timeFrom)
            toDateTime = dateTo.Add(timeTo)
        Else
            MsgBox("Select Report Type and Proceed!!!")
            Exit Sub
        End If

        _Packer = Packers.FirstOrDefault(Function(e) e.DeviceId = PackerId)
        _spout = _Packer.SpoutList.FirstOrDefault(Function(e) e.SpoutId = SpoutId)

        If CurrentSelection = "Individual Report" Then
            header = {"Sl No", "Packer", "Spout", "Event Time", "Weight", "Cumm. Weight", "Final", "Deviation"}
            title = "Individual Report"
            If Cmbospout.Text = "All" Then
                If ChkFl Then
                    query = $"SELECT ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo, StoreTime) AS [S.No],
                             PackerName,SpoutName,TimeDtFrmt,CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 
                             ELSE ActualWt END AS ActualWt,TargetWt,ROUND(Deviation, 2) AS Deviation
                             FROM SpoutData WHERE PackerNo = @PackerNo AND TimeDtFrmt BETWEEN @curDt AND @curDt2
                             ORDER BY PackerName, SpoutNo, TimeDtFrmt;"

                Else
                    query = $"SELECT ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo, StoreTime) AS [S.No],
                         PackerName,SpoutName,TimeDtFrmt,ActualWt,TargetWt,ROUND(Deviation, 2) AS Deviation FROM SpoutData
                            WHERE PackerNo = @PackerNo
                              AND TimeDtFrmt BETWEEN @curDt AND @curDt2
                            ORDER BY PackerName, SpoutNo, TimeDtFrmt;"
                End If


                parameters.Add("@PackerNo", PackerId)
                parameters.Add("@curDt", fromDateTime)
                parameters.Add("@curDt2", toDateTime)
            Else
                If ChkFl Then
                    query = $"SELECT ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo, StoreTime) AS [S.No],
                PackerName,SpoutName,TimeDtFrmt,CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 
                  ELSE ActualWt END AS ActualWt,TargetWt,round(Deviation,2) AS Deviation
                 FROM SpoutData
                 WHERE PackerNo = @PackerNo AND SpoutNo = @SpoutNo 
                 AND TimeDtFrmt BETWEEN @curDt AND @curDt2 
                 ORDER BY PackerName, SpoutNo, TimeDtFrmt"
                Else
                    query = $"SELECT ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo, StoreTime) AS [S.No],
                PackerName,SpoutName,TimeDtFrmt,ActualWt,TargetWt,round(Deviation,2) AS Deviation
                 FROM SpoutData
                 WHERE PackerNo = @PackerNo AND SpoutNo = @SpoutNo 
                 AND TimeDtFrmt BETWEEN @curDt AND @curDt2 
                 ORDER BY PackerName, SpoutNo, TimeDtFrmt"
                End If
                parameters.Add("@PackerNo", PackerId)
                parameters.Add("@SpoutNo", _spout.SpoutId)
                parameters.Add("@curDt", fromDateTime)
                parameters.Add("@curDt2", toDateTime)
            End If

            HandleSpinner(True)
            Await GetReport(fromDateTime, toDateTime, query, parameters)
            HandleSpinner(False)
        ElseIf CurrentSelection = "Percentage Report" Then
            title = "Percentage Report"
            header = {"REPORT DATE", "SPOUT NO", "NOOF BAGS", "TOTAL PDT", "% OF UNDER WEIGHT(Below 49.99)", "UW BAGS", "% OF NORMAL WEIGHT(50.00 TO 50.40)", "NW BAGS", "% OF OVER WEIGHT(>50.40)", "OW BAGS"}
            If ChkFl Then
                query = " SELECT
                            ROW_NUMBER() OVER (ORDER BY [REPORT Date], [SPOUT NO]) AS [S.No],
                            [REPORT Date],
                            [SPOUT NO],
                            [NOOF BAGS],
                            [TOTAL PDT],
                            [UW BAGS],
                            [% OF UNDER WEIGHT(Below 49.99)],
                            [NW BAGS],
                            [% OF NORMAL WEIGHT(50.00 TO 50.40)],
                            [OW BAGS],
                            [% OF OVER WEIGHT(>50.40)]
                        FROM
                        (
                            SELECT
                                CAST(CAST(TimeDtFrmt AS DATE) AS varchar) AS [REPORT Date],
                                SpoutNo AS [SPOUT NO],
                                COUNT(*) AS [NOOF BAGS],
                                COUNT(*) AS [TOTAL PDT],
                                SUM(CASE WHEN ActualWt < TargetWt - 0.5 THEN 1 ELSE 0 END) AS [UW BAGS],
                                CAST(100.0 * SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) < TargetWt - 0.5 THEN 1 ELSE 0 END) / COUNT(*) AS Decimal(5,2)) AS [% OF UNDER WEIGHT(Below 49.99)],
                                SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) BETWEEN TargetWt - 0.5 AND TargetWt + 0.5 THEN 1 ELSE 0 END) AS [NW BAGS],
                                CAST(100.0 * SUM(CASE WHEN ActualWt BETWEEN TargetWt - 0.5 AND TargetWt + 0.5 THEN 1 ELSE 0 END) / COUNT(*) AS Decimal(5,2)) AS [% OF NORMAL WEIGHT(50.00 TO 50.40)],
                                SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) > TargetWt + 0.5 THEN 1 ELSE 0 END) AS [OW BAGS],
                                CAST(100.0 * SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) > TargetWt + 0.5 THEN 1 ELSE 0 END) / COUNT(*) AS Decimal(5,2)) AS [% OF OVER WEIGHT(>50.40)]
                            FROM SpoutData
                            WHERE PackerNo = @packer
                              AND TimeDtFrmt BETWEEN @startDt AND @endDt
                            GROUP BY CAST(TimeDtFrmt AS DATE), SpoutNo
                        ) AS groupedData
                        ORDER BY [REPORT Date], [SPOUT NO];"
            Else
                query = " SELECT
                            ROW_NUMBER() OVER (ORDER BY [REPORT Date], [SPOUT NO]) AS [S.No],
                            [REPORT Date],
                            [SPOUT NO],
                            [NOOF BAGS],
                            [TOTAL PDT],
                            [UW BAGS],
                            [% OF UNDER WEIGHT(Below 49.99)],
                            [NW BAGS],
                            [% OF NORMAL WEIGHT(50.00 TO 50.40)],
                            [OW BAGS],
                            [% OF OVER WEIGHT(>50.40)]
                        FROM
                        (
                            SELECT
                                CAST(CAST(TimeDtFrmt AS DATE) AS varchar) AS [REPORT Date],
                                SpoutNo AS [SPOUT NO],
                                COUNT(*) AS [NOOF BAGS],
                                COUNT(*) AS [TOTAL PDT],
                                SUM(CASE WHEN ActualWt < TargetWt - 0.5 THEN 1 ELSE 0 END) AS [UW BAGS],
                                CAST(100.0 * SUM(CASE WHEN ActualWt < TargetWt - 0.5 THEN 1 ELSE 0 END) / COUNT(*) AS Decimal(5,2)) AS [% OF UNDER WEIGHT(Below 49.99)],
                                SUM(CASE WHEN ActualWt BETWEEN TargetWt - 0.5 AND TargetWt + 0.5 THEN 1 ELSE 0 END) AS [NW BAGS],
                                CAST(100.0 * SUM(CASE WHEN ActualWt BETWEEN TargetWt - 0.5 AND TargetWt + 0.5 THEN 1 ELSE 0 END) / COUNT(*) AS Decimal(5,2)) AS [% OF NORMAL WEIGHT(50.00 TO 50.40)],
                                SUM(CASE WHEN ActualWt > TargetWt + 0.5 THEN 1 ELSE 0 END) AS [OW BAGS],
                                CAST(100.0 * SUM(CASE WHEN ActualWt > TargetWt + 0.5 THEN 1 ELSE 0 END) / COUNT(*) AS Decimal(5,2)) AS [% OF OVER WEIGHT(>50.40)]
                            FROM SpoutData
                            WHERE PackerNo = @packer
                              AND TimeDtFrmt BETWEEN @startDt AND @endDt
                            GROUP BY CAST(TimeDtFrmt AS DATE), SpoutNo
                        ) AS groupedData
                        ORDER BY [REPORT Date], [SPOUT NO];"
            End If

            parameters.Add("@packer", PackerId)
            parameters.Add("@startDt", fromDateTime)
            parameters.Add("@endDt", toDateTime)
            HandleSpinner(True)
            Await GetReport(fromDateTime, toDateTime, query, parameters)
            HandleSpinner(False)
        ElseIf CurrentSelection = "Spout Wise Report" Then
            title = "Spout Wise Report"
            header = {"DATE", "SPOUTNO", "NO OF BAGS", "TOTAL DISPATCH(Tones)", "DEVIATION", "AVERAGE OF TARGET", "ACTUAL WEIGHT(MAX)", "ACTUAL WEIGHT(MIN)"}
            If ChkFl Then
                query = "SELECT ROW_NUMBER() OVER (ORDER BY [DATE], [SPOUTNO]) AS [S.No],[DATE], [SPOUTNO],
                                                [NO OF BAGS],[TOTAL DISPATCH(Tones)],[DEVIATION],
                                                [ACTUAL WEIGHT(AVERAGE)],[ACTUAL WEIGHT(MIN)],[ACTUAL WEIGHT(MAX)]
                                            FROM
                                            (SELECT CAST(CAST(TimeDtFrmt AS DATE) AS varchar) AS [DATE],
                                             SpoutNo AS [SPOUTNO],COUNT(SpoutNo) AS [NO OF BAGS],
                                             ROUND(SUM( (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END)) / 1000, 2) AS [TOTAL DISPATCH(Tones)],
                                             ROUND(AVG(Deviation), 2) AS [DEVIATION],
                                             ROUND(AVG( (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END)), 2) AS [ACTUAL WEIGHT(AVERAGE)],
                                             ROUND(MIN( (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END)), 2) AS [ACTUAL WEIGHT(MIN)],
                                             ROUND(MAX( (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END)), 2) AS [ACTUAL WEIGHT(MAX)]
                                                FROM SpoutData
                                                WHERE PackerNo = @packer
                                                  AND TimeDtFrmt BETWEEN @startDt AND @endDt
                                                GROUP BY CAST(TimeDtFrmt AS DATE), SpoutNo
                                            ) AS groupedData
                                            ORDER BY [DATE], [SPOUTNO];"
            Else
                query = "SELECT ROW_NUMBER() OVER (ORDER BY [DATE], [SPOUTNO]) AS [S.No],[DATE], [SPOUTNO],
                                                [NO OF BAGS],
                                                [TOTAL DISPATCH(Tones)],
                                                [DEVIATION],
                                                [ACTUAL WEIGHT(AVERAGE)],
                                                [ACTUAL WEIGHT(MIN)],
                                                [ACTUAL WEIGHT(MAX)]
                                            FROM
                                            (
                                                SELECT
                                                    CAST(CAST(TimeDtFrmt AS DATE) AS varchar) AS [DATE],
                                                    SpoutNo AS [SPOUTNO],
                                                    COUNT(SpoutNo) AS [NO OF BAGS],
                                                    ROUND(SUM(ActualWt) / 1000, 2) AS [TOTAL DISPATCH(Tones)],
                                                    ROUND(AVG(Deviation), 2) AS [DEVIATION],
                                                    ROUND(AVG(ActualWt), 2) AS [ACTUAL WEIGHT(AVERAGE)],
                                                    ROUND(MIN(ActualWt), 2) AS [ACTUAL WEIGHT(MIN)],
                                                    ROUND(MAX(ActualWt), 2) AS [ACTUAL WEIGHT(MAX)]
                                                FROM SpoutData
                                                WHERE PackerNo = @packer
                                                  AND TimeDtFrmt BETWEEN @startDt AND @endDt
                                                GROUP BY CAST(TimeDtFrmt AS DATE), SpoutNo
                                            ) AS groupedData
                                            ORDER BY [DATE], [SPOUTNO];"
            End If
            parameters.Add("@packer", PackerId)
            parameters.Add("@startDt", fromDateTime)
            parameters.Add("@endDt", toDateTime)
            HandleSpinner(True)
            Await GetReport(fromDateTime, toDateTime, query, parameters)
            HandleSpinner(False)
        ElseIf CurrentSelection = "Weight Wise Report" Then
            title = "Weight Wise Report"
            header = {"DATE", "SPOUTNO", "<49", "49-49.6", "49.6-50", "50-50.53", "50.3-50.6", "50.6-51", "51-52", "52"}
            If ChkFl Then
                query = "SELECT ROW_NUMBER() OVER (ORDER BY [DATE], [SPOUTNO]) AS [S.No],
                                [DATE],[SPOUTNO],[<49], [49-49.6],[49.6-50],[50-50.3],[50.3-50.6],[50.6-51],[51-52], [>52] FROM
                                 ( SELECT CAST(CAST(TimeDtFrmt AS DATE) AS varchar) AS [DATE],
                                            SpoutNo AS [SPOUTNO],
                                            SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) < 49 THEN 1 ELSE 0 END) AS [<49],
                                            SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) >= 49 AND ActualWt < 49.6 THEN 1 ELSE 0 END) AS [49-49.6],
                                            SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) >= 49.6 AND ActualWt < 50 THEN 1 ELSE 0 END) AS [49.6-50],
                                            SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) >= 50 AND ActualWt < 50.3 THEN 1 ELSE 0 END) AS [50-50.3],
                                            SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) >= 50.3 AND ActualWt < 50.6 THEN 1 ELSE 0 END) AS [50.3-50.6],
                                            SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) >= 50.6 AND ActualWt < 51 THEN 1 ELSE 0 END) AS [50.6-51],
                                            SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) >= 51 AND ActualWt < 52 THEN 1 ELSE 0 END) AS [51-52],
                                            SUM(CASE WHEN  (CASE WHEN ActualWt > 50.2 THEN ActualWt - 0.4 ELSE ActualWt END) > 52 THEN 1 ELSE 0 END) AS [>52]
                                            FROM SpoutData WHERE PackerNo = @packer AND TimeDtFrmt BETWEEN @startDt AND @endDt GROUP BY CAST(TimeDtFrmt AS DATE), SpoutNo) AS groupedData
                                            ORDER BY [DATE], [SPOUTNO];"
            Else
                query = "SELECT ROW_NUMBER() OVER (ORDER BY [DATE], [SPOUTNO]) AS [S.No],
                                [DATE],[SPOUTNO],[<49], [49-49.6],[49.6-50],[50-50.3],[50.3-50.6],[50.6-51],[51-52], [>52] FROM
                                 ( SELECT CAST(CAST(TimeDtFrmt AS DATE) AS varchar) AS [DATE],
                                            SpoutNo AS [SPOUTNO],
                                            SUM(CASE WHEN ActualWt < 49 THEN 1 ELSE 0 END) AS [<49],
                                            SUM(CASE WHEN ActualWt >= 49 AND ActualWt < 49.6 THEN 1 ELSE 0 END) AS [49-49.6],
                                            SUM(CASE WHEN ActualWt >= 49.6 AND ActualWt < 50 THEN 1 ELSE 0 END) AS [49.6-50],
                                            SUM(CASE WHEN ActualWt >= 50 AND ActualWt < 50.3 THEN 1 ELSE 0 END) AS [50-50.3],
                                            SUM(CASE WHEN ActualWt >= 50.3 AND ActualWt < 50.6 THEN 1 ELSE 0 END) AS [50.3-50.6],
                                            SUM(CASE WHEN ActualWt >= 50.6 AND ActualWt < 51 THEN 1 ELSE 0 END) AS [50.6-51],
                                            SUM(CASE WHEN ActualWt >= 51 AND ActualWt < 52 THEN 1 ELSE 0 END) AS [51-52],
                                            SUM(CASE WHEN ActualWt > 52 THEN 1 ELSE 0 END) AS [>52]
                                            FROM SpoutData WHERE PackerNo = @packer AND TimeDtFrmt BETWEEN @startDt AND @endDt
                                            GROUP BY CAST(TimeDtFrmt AS DATE), SpoutNo) AS groupedData ORDER BY [DATE], [SPOUTNO];"
            End If
            parameters.Add("@packer", PackerId)
            parameters.Add("@startDt", fromDateTime)
            parameters.Add("@endDt", toDateTime)
            HandleSpinner(True)
            Await GetReport(fromDateTime, toDateTime, query, parameters)
            HandleSpinner(False)
        ElseIf CurrentSelection = "Manual Weight Report" Then
            title = "Manual Weight Report"
            header = {"Sl No", "Event Time", "Packer No", "Spout No", "Weight"}

            If Cmbospout.Text = "All" Then
                If ChkFl Then
                    query = $"SELECT  ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo) AS [S.No], DateTime,PackerNo,SpoutNo,Weight FROM ManualWt 
                 WHERE PackerNo = @PackerNo 
                 AND DateTime BETWEEN @curDt AND @curDt2"
                Else
                    query = $"SELECT  ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo) AS [S.No], DateTime,PackerNo,SpoutNo,Weight FROM ManualWt 
                 WHERE PackerNo = @PackerNo 
                 AND DateTime BETWEEN @curDt AND @curDt2"
                End If
                parameters.Add("@PackerNo", PackerId)
                parameters.Add("@curDt", fromDateTime)
                parameters.Add("@curDt2", toDateTime)
            Else
                If ChkFl Then
                    query = $"SELECT  ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo) AS [S.No],DateTime,PackerNo,SpoutNo,Weight FROM ManualWt 
                 WHERE PackerNo = @PackerNo AND SpoutNo = @SpoutNo 
                 AND DateTime BETWEEN @curDt AND @curDt2"
                Else
                    query = $"SELECT  ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo) AS [S.No],DateTime,PackerNo,SpoutNo,Weight FROM ManualWt 
                 WHERE PackerNo = @PackerNo AND SpoutNo = @SpoutNo 
                 AND DateTime BETWEEN @curDt AND @curDt2"
                End If
                parameters.Add("@PackerNo", PackerId)
                parameters.Add("@SpoutNo", _spout.SpoutId)
                parameters.Add("@curDt", fromDateTime)
                parameters.Add("@curDt2", toDateTime)
            End If
            HandleSpinner(True)
            Await GetReport(fromDateTime, toDateTime, query, parameters)
            HandleSpinner(False)
        ElseIf CurrentSelection = "Auto Correction Report" Then
            title = "Auto Correction Report"
            header = {"Sl No", "Event Time", "Packer No", "Spout No", "Old FinalAdjustment", "New FinalAdjustment"}

            If Cmbospout.Text = "All" Then
                If ChkFl Then
                    query = $"SELECT  ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo,TimeDtFrmt) AS [S.No], TimeDtFrmt,PackerNo,SpoutNo,OLD_FINALADJUSTMENTWT,NEW_FINALADJUSTMENTWT,AutoCorrectionRemarks FROM AUTO_CORRECTION_LOGS 
                 WHERE PackerNo = @PackerNo 
                 AND TimeDtFrmt BETWEEN @curDt AND @curDt2"
                Else
                    query = $"SELECT  ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo,TimeDtFrmt) AS [S.No], TimeDtFrmt,PackerNo,SpoutNo,OLD_FINALADJUSTMENTWT,NEW_FINALADJUSTMENTWT,AutoCorrectionRemarks FROM AUTO_CORRECTION_LOGS 
                 WHERE PackerNo = @PackerNo 
                 AND TimeDtFrmt BETWEEN @curDt AND @curDt2"
                End If
                parameters.Add("@PackerNo", PackerId)
                parameters.Add("@curDt", fromDateTime)
                parameters.Add("@curDt2", toDateTime)
            Else
                If ChkFl Then
                    query = $"SELECT  ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo,TimeDtFrmt) AS [S.No],TimeDtFrmt,PackerNo,SpoutNo,OLD_FINALADJUSTMENTWT,NEW_FINALADJUSTMENTWT,AutoCorrectionRemarks  FROM AUTO_CORRECTION_LOGS 
                 WHERE PackerNo = @PackerNo AND SpoutNo = @SpoutNo 
                 AND TimeDtFrmt BETWEEN @curDt AND @curDt2"
                Else
                    query = $"SELECT  ROW_NUMBER() OVER (ORDER BY PackerNo, SpoutNo,TimeDtFrmt) AS [S.No],TimeDtFrmt,PackerNo,SpoutNo,OLD_FINALADJUSTMENTWT,NEW_FINALADJUSTMENTWT,AutoCorrectionRemarks  FROM AUTO_CORRECTION_LOGS 
                 WHERE PackerNo = @PackerNo AND SpoutNo = @SpoutNo 
                 AND TimeDtFrmt BETWEEN @curDt AND @curDt2"
                End If
                parameters.Add("@PackerNo", PackerId)
                parameters.Add("@SpoutNo", _spout.SpoutId)
                parameters.Add("@curDt", fromDateTime)
                parameters.Add("@curDt2", toDateTime)
            End If
            HandleSpinner(True)
            Await GetReport(fromDateTime, toDateTime, query, parameters)
            HandleSpinner(False)
        End If
    End Sub

    Public Function GetReportRaw(fromDate As DateTime, toDate As DateTime, queryTemplate As String, parameterValues As Dictionary(Of String, Object)) As DataTable
        Dim dt As Date = fromDate
        Dim finalTable As DataTable = Nothing
        Dim tableName As String = "SpoutData"
        Dim actualQuery As String = queryTemplate.Replace("{tableName}", tableName)

        Dim freshParams As SqlParameter() = parameterValues.Select(
                                                                        Function(kvp) New SqlParameter(kvp.Key, kvp.Value)
                                                                    ).ToArray()

        Dim result As DataTable = helper.ExecuteDataTable(actualQuery, freshParams)
        Console.WriteLine(actualQuery)

        If result IsNot Nothing AndAlso result.Rows.Count > 0 Then
            If finalTable Is Nothing Then
                finalTable = result.Clone()
            End If
            For Each row As DataRow In result.Rows
                finalTable.ImportRow(row)
            Next
        End If
        Return finalTable
    End Function

    Private Function GetShiftTimeSetting(key As String, defaultSeconds As String) As String
        Dim timeStr As String = RestoreSettings(TITLE1, "Properties", key)
        If Not String.IsNullOrWhiteSpace(timeStr) Then
            Return timeStr
        End If
        Return defaultSeconds
    End Function

    Public Async Function GetReport(fromDate As DateTime, toDate As DateTime, queryTemplate As String, parameterValues As Dictionary(Of String, Object)) As Task
        Dim tableName As String = "SpoutData"
        Dim combinedTable As DataTable = Nothing

        Try
            If Not helper.TableExists(tableName) Then
                MsgBox("Table not found: " & tableName, MsgBoxStyle.Exclamation, "Database")
                DataGridView1.DataSource = Nothing
                Exit Function
            End If

            Dim actualQuery As String = queryTemplate.Replace("{tableName}", tableName)
            Dim freshParams As SqlParameter() = parameterValues.Select(
                Function(kvp) New SqlParameter(kvp.Key, kvp.Value)).ToArray()
            Dim result As DataTable = helper.ExecuteDataTable(actualQuery, freshParams)

            If result Is Nothing OrElse result.Rows.Count = 0 Then
                MsgBox("No record found for the selected period!!!", MsgBoxStyle.Information, "Report")
                DataGridView1.DataSource = Nothing
                Exit Function
            End If

            'combinedTable = result.Clone()

            'If Not combinedTable.Columns.Contains("S.No") Then
            '    combinedTable.Columns.Add("S.No", GetType(Integer)).SetOrdinal(0)
            'End If


            'For Each row As DataRow In result.Rows
            '    combinedTable.ImportRow(row)
            'Next

            'Dim serialNo As Integer = 1
            'For Each row As DataRow In combinedTable.Rows
            '    row("S.No") = serialNo
            '    serialNo += 1
            'Next


            'If header IsNot Nothing AndAlso header.Length = combinedTable.Columns.Count Then
            '    For i As Integer = 0 To header.Length - 1
            '        combinedTable.Columns(i).ColumnName = header(i)
            '    Next
            'End If


            Dim FTD As String = $"Date From: {fromDate}  TO: {toDate}"
            If excel Then
                Rep = New Report(result, title, FTD, excelFL:=True)
            ElseIf pdf Then
                Rep = New Report(result, title, FTD, pdfFL:=True)
            ElseIf View Then
                Me.Invoke(Sub()
                              DataGridView1.Visible = True
                              DataGridView1.DataSource = result
                              DataGridView1.ColumnHeadersDefaultCellStyle.Font = New System.Drawing.Font("Arial", 10, FontStyle.Bold)
                              DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                              DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
                              DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                              DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

                              For Each column As DataGridViewColumn In DataGridView1.Columns
                                  column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                                  If column.ValueType Is GetType(System.DateTime) Then
                                      column.DefaultCellStyle.Format = "G"
                                  End If
                                  If column.ValueType Is GetType(Double) OrElse column.ValueType Is GetType(Decimal) Then
                                      column.DefaultCellStyle.Format = "N2"
                                  End If
                                  column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                              Next
                          End Sub)
            End If
        Catch ex As Exception
            Console.WriteLine("Error in GetReport: " & ex.Message)
            MsgBox("Error while generating report: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
        Await Task.Delay(1)
    End Function

    Private Sub CmboPack_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmboPack.SelectedIndexChanged
        Dim PackerConbo = TryCast(sender, ComboBox)
        Dim PackerId = PackerConbo.SelectedIndex + 1
        Dim Packer = Packers.FirstOrDefault(Function(Pkr) Pkr.DeviceId = PackerId)
        Cmbospout.Items.Clear()
        If Packer IsNot Nothing Then
            Dim NoOfSpout = Packer.SpoutList.Count
            Cmbospout.Items.Add("All")
            For i = 1 To NoOfSpout
                Cmbospout.Items.Add(i)
            Next
            Cmbospout.SelectedIndex = 0
        End If
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        WGHFL = False
        PERFL = False
        DataGridView1.DataSource = Nothing
        Me.Close()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        excel = True
        pdf = False
        View = False
        HandleSpinner(True)
        PrepareQuery()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        excel = False
        pdf = True
        View = False
        HandleSpinner(True)
        PrepareQuery()
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        DataGridView1.DataSource = Nothing
        excel = False
        pdf = False
        View = True
        HandleSpinner(True)
        PrepareQuery()
    End Sub
    Private Sub FrmReports_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        ResizeLayout()
    End Sub
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            ChkFl = True
        Else
            ChkFl = False
        End If
    End Sub
End Class