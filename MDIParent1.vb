Imports System.ComponentModel
Imports System.IO.Ports
Imports Newtonsoft.Json
Imports System.IO
Imports System.Linq.Expressions
Public Class MDIParent1
    Dim TmpTmr1 As DateTime = DateTime.Now
    Private RCBST(5) As String
    Private RC(5) As Integer
    Private SER_RCVSTR(5) As String
    Private SERDATAOKFL(5) As Boolean
    Private SERCMPSTR(5) As String
    Private SERIALLEN(5) As Integer
    Private COMBYTRCFL(5) As Boolean
    Private COMBYTCTR1(5) As Integer
    Private _Unipulse As New Unipulse
    Private Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs)
        Dim ChildForm As New System.Windows.Forms.Form
        ChildForm.MdiParent = Me
        m_ChildFormNumber += 1
        ChildForm.Text = "Window " & m_ChildFormNumber
        ChildForm.Show()
    End Sub
    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs)
        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        OpenFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = OpenFileDialog.FileName
        End If
    End Sub
    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        If (SaveFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = SaveFileDialog.FileName

        End If
    End Sub
    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.Close()
    End Sub
    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub
    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub
    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub
    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub
    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub
    Private m_ChildFormNumber As Integer
    Private Sub MDIParent1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            InitializePackers()
            Dim ChildForm As New frmMimic1()
            ChildForm.MdiParent = Me
            ChildForm.WindowState = FormWindowState.Normal
            ChildForm.BringToFront()
            ChildForm.Show()
            If NewDB = True Then
                frmPackerPort.Show()
            End If
        Catch ex As Exception
            CentralErrhandler("MainForm_Load", Me.Name)
        End Try
    End Sub
    Private Sub InitializePackers()
Retryquery:
        Dim sql As String = "SELECT * FROM COMMPKRTB ORDER BY PKRNO"
        Try
            Dim dt As DataTable = helper.ExecuteDataTable(sql)
            If dt.Rows.Count > 0 Then
                Dim I As Integer = 0
                Dim K As Integer
                Dim parityVal As Parity
                For Each row As DataRow In dt.Rows
                    Dim _Packer As New Packer With {
                        .DeviceIdentifier = row("PKRNAME").ToString(),
                        .IPAddress = row("IP").ToString(),
                        .Port = Convert.ToInt32(row("PORT")),
                        .MaxSpout = Convert.ToInt32(row("TOTALSPOUT")),
                        .DeviceId = row("PKRNO"),
                        .DeviceModeType = row("Mode"),
                        .IsConnected = False,
                        .IsParameterRequested = False,
                        .QueryTimeOut = Convert.ToInt32(row("RESPTI")),
                        .CommunicationType = row("CommunicationType"),
                        .PortName = row("PortName"),
                        .BaudRate = row("BaudRate"),
                        .ControllerModel = row("PKRTYPE"),
                        .Parity = row("Parity"),
                        .CWPortName = row("CWPortName"),
                        .CWBaudRate = row("CWBaudRate"),
                        .CWParity = [Enum].TryParse(row("CWParity"), True, parityVal),
                        .CheckWeigher = row("IsCheckWeigher")
                    } '.Parity = [Enum].TryParse(row("Parity"), True, parityVal),
                    For K = 1 To Convert.ToInt32(row("TOTALSPOUT"))
                        Dim _SpoutQuery As QueryInfo = Nothing
                        If _Packer.DeviceModeType = "ControllerMode" Then
                                _SpoutQuery = _Unipulse.ReadWeightQT(row("PKRTYPE"), K, 2)
                            ElseIf _Packer.DeviceModeType = "SlipringMode" Then
                                _SpoutQuery = _Unipulse.ReadWeight(row("PKRTYPE"), K, "")
                            End If
                            Dim _SpoutController As New SpoutController With {
                               .PackerId = row("PKRNO"),
                               .ControllerModel = row("PKRTYPE"),
                               .PackerName = row("PKRNAME"),
                               .SpoutId = K,
                               .SpoutName = "Spout - " & K,
                               .Query = _SpoutQuery.Query
                            }
                            _SpoutController.DeviceModeType = _Packer.DeviceModeType
                            _Packer.SpoutList.Add(_SpoutController)
                    Next
                    spoutCount = Convert.ToInt32(row("TOTALSPOUT")) + 1
                    Packers.Add(_Packer)

                    I += 1
                Next
            Else
                frmPackerPort.ShowDialog()
                GoTo Retryquery
            End If
            ReDim IsApplicationLoaded(Packers.Count)
            ReDim sample(Packers.Count)
            ReDim coefficient(Packers.Count)
            ReDim CorrectionRangeFrom(Packers.Count)
            ReDim CorrectionRangeTo(Packers.Count)
            ReDim ValidWeightFrom(Packers.Count)
            ReDim ValidWeightTo(Packers.Count)
            ReDim CorrectionLimitFrom(Packers.Count)
            ReDim CorrectionLimitTo(Packers.Count)
            ReDim IsAutoCorrectionEnabled(Packers.Count)
            packerCount = Packers.Count
            MAXPKR = Packers.Count
        Catch ex As Exception
            CentralErrhandler("RetrieveProp", Me.Name)
        End Try
    End Sub
    Private Sub UpdateStatusSafe(statusLabel As ToolStripStatusLabel, newText As String)
        Dim cleanData As String = newText.Replace(vbCr, "").Replace(vbLf, "").Trim()
        Try
            If statusLabel.GetCurrentParent.InvokeRequired Then
                statusLabel.GetCurrentParent.Invoke(Sub()
                                                        SetStatusText(statusLabel, cleanData)
                                                    End Sub)
            Else
                SetStatusText(statusLabel, cleanData)
            End If
        Catch ex As NullReferenceException

        End Try
    End Sub
    Private Sub SetStatusText(statusLabel As ToolStripStatusLabel, newText As String)
        If statusLabel.Text <> newText Then
            statusLabel.Text = newText
            statusLabel.GetCurrentParent.Invalidate(False)
        End If
    End Sub
    Public Function CompareLic(LicKey As String) As Boolean
        Dim LicArr() As String
        Dim LicInt As Integer
        If LicKey <> "" Then
            LicArr = Split(LicKey, "-", -1, vbTextCompare)
            LicInt = UBound(LicArr) - LBound(LicArr) + 1
            If LicInt >= 2 Then
                If LicArr(0) = LISTR(1) And LicArr(1) = LISTR(2) And LicArr(2) = LISTR(3) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
    Private Function ConvIDToStr(IDN As Integer, PKC As Integer) As String
        Dim I As Integer
        Dim RT As String
        On Error Resume Next
        I = IDN
        If I >= 0 And I <= 9 Then
            RT = "000" & I
        ElseIf I >= 10 And I <= 99 Then
            RT = "00" & I
        ElseIf I >= 100 And I <= 999 Then
            RT = "0" & I
        Else
            RT = I
        End If
        Return RT
    End Function
    Private Sub SavePackerDetails()
        Dim _SpoutJson As New List(Of SpoutController)
        Dim JsonString As String = Nothing
        For Each _packer As Packer In Packers
            For Each _Spout As SpoutController In _packer.SpoutList
                Dim _Json As New SpoutController With {
                        .PackerId = _Spout.PackerId,
                    .CodeNo = _Spout.CodeNo,
                    .LastDischargedBagWeight = _Spout.LastDischargedBagWeight,
                    .FinalWeight = _Spout.FinalWeight,
                    .Under = _Spout.Under,
                    .Over = _Spout.Over,
                    .BagCount = _Spout.BagCount,
                    .TotalWtTonnes = _Spout.TotalWtTonnes,
                    .Deviation = _Spout.Deviation,
                    .LastDIschargedBagDateTime = _Spout.LastDIschargedBagDateTime,
                    .EmptyRound = _Spout.EmptyRound,
                    .BatchStartTime = _Spout.BatchStartTime,
                    .datetime = _Spout.datetime
                    }
                _SpoutJson.Add(_Json)
            Next
        Next
        JsonString = JsonConvert.serializeobject(_SpoutJson)
        Dim DirectoryPath As String = $"c:\{Application.ProductName}"
        Dim FilePath As String = Path.Combine(DirectoryPath, "Spout.Json")

        If Not Directory.Exists(DirectoryPath) Then
            Directory.CreateDirectory(DirectoryPath)
        End If
        File.WriteAllText(FilePath, JsonString)
    End Sub
    Private Sub MDIParent1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        SavePackerDetails()
        If allowClose = False Then
            Frmclosing.ShowDialog()
            e.Cancel = True
        End If
        If Not allowClose Then Exit Sub
        isClosing = True
        SQLCONN.Close()
    End Sub
    Private Sub ClearAccValuesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearAccValuesToolStripMenuItem.Click
        frmCleaAccData.Show()
    End Sub
    Private Sub CopyBatchParameterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyBatchParameterToolStripMenuItem.Click
        CFGPASFL = False
        frmCfgPass.ShowDialog()
        If CFGPASFL = True Then
            frmCopyParameters.Show()
        End If
    End Sub
    Private Sub ConfigureAdditionalParameterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureAdditionalParameterToolStripMenuItem.Click
        frmParameterScr.Show()
    End Sub
    Private Sub ConfigureBatchParameterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureBatchParameterToolStripMenuItem.Click
        frmConfigBatProp.Show()
    End Sub
    Private Sub WriteCodeNoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WriteCodeNoToolStripMenuItem.Click
        frmWrCodeNo.Show()
    End Sub
    Private Sub ChangeClosePasswordToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeClosePasswordToolStripMenuItem.Click
        CFGPASFL = False
        frmCfgPass.ShowDialog()
        If CFGPASFL = True Then
            frmChangePass.Show()
        End If
    End Sub
    Private Sub ShiftTimingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShiftTimingToolStripMenuItem.Click
        frmShiftTiming.Show()
    End Sub
    Private Sub CommProperToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CommProperToolStripMenuItem.Click
        frmPackerPort.Show()
    End Sub
    Private Sub ReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportToolStripMenuItem.Click
        FrmReports.Show()
    End Sub
    Private Sub AutoCorrectionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoCorrectionToolStripMenuItem.Click
        FrmAutocrt.Show()
    End Sub
    Private Sub ToolStripTextBox2_Click(sender As Object, e As EventArgs)
        frmgridscale.Show()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub GridPropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GridPropertiesToolStripMenuItem.Click
        frmgridscale.Show()
    End Sub

    Private Sub LicenseKeyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LicenseKeyToolStripMenuItem.Click
        LicenseDetails.Show()
    End Sub
End Class
