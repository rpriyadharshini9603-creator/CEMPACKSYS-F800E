Imports System.IO
Imports System.Management
Imports System.Data.SqlClient
Public Class frmSplash
    Dim InitDel As Integer = 0
    Private Const MARQUEE = "CEMENT PACKER MONITORING AND LOGGING SOFTWARE............."
    Dim strMarquee As String = MARQUEE

    ' UI Controls
    Public titleLabel As New Label
    Public statusLabel As New Label
    Public centerLabel As New Label
    Public Label4 As New Label
    Public Label5 As New Label
    Public Label7 As New Label
    Public CancelButton1 As New Button
    Public centerLayout As New TableLayoutPanel
    Private PictureBoxImageBar As PictureBox
    Private PanelTrackBar As Panel
    Private PanelProgressFill As Panel
    Public progressBar As New ProgressBar

    Private Sub frmSplash_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Design()
        If Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1 Then
            MessageBox.Show("System is Already in Run Mode ...", "System is Already Running ...", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If
        CheckLicense()
        Timer1.Enabled = True
        Timer2.Enabled = True
        statusLabel.Text = MARQUEE
    End Sub
    Private Sub CheckLicense()
        If Not LicenseManager.CheckLicense() Then
            LicenseDetails.ShowDialog()
            If Not LicenseManager.CheckLicense() Then
                Application.Exit()
            End If
        End If
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        InitDel += 1
        Application.DoEvents()

        Dim maxPos As Integer = (PanelTrackBar.Width) - 5
        Dim progressRatio As Double = Math.Min(InitDel, 100) / 100.0
        Dim currentWidth As Integer = CInt(progressRatio * maxPos)
        PanelProgressFill.Width = currentWidth

        If InitDel < 30 Then
            Timer1.Interval = 10
            Label4.Text = "Application is loading, please wait..."
        ElseIf InitDel < 100 Then
            Timer1.Interval = 30
            Label4.Text = "Copying DLL Files, please wait..."
        Else

            Label4.Text = "Ready to Start the Application"
            progressBar.Visible = False
            Label4.Visible = True
            Timer1.Enabled = False
            InitDel = 0
            PanelProgressFill.Width = maxPos
            CreatChkMastDB()
            CancelButton1.Text = "Enter"
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If strMarquee.Length > 1 Then
            strMarquee = strMarquee.Substring(1) & strMarquee(0)
            statusLabel.Text = strMarquee
        End If
    End Sub

    Private Sub Label7_DoubleClick(sender As Object, e As EventArgs)
        Dim inputCode As String = Microsoft.VisualBasic.Interaction.InputBox("Double Click Label and Enter the number received through SMS", "SMS REPLY NUMBER")
        If Not String.IsNullOrWhiteSpace(inputCode) Then
            Dim serial As String = GetVolumeSerial("C")
            If String.IsNullOrEmpty(serial) Then
                serial = GetVolumeSerial(Application.StartupPath.Substring(0, 1))
            End If
            If String.IsNullOrEmpty(serial) Then
                serial = "-1987654321"
            End If

            If serial.Length > 2 Then
                serial = serial.Substring(2)
            End If

            If serial = inputCode Then
                File.WriteAllText("license.txt", inputCode)
                Label7.Text = ""
                MessageBox.Show("License Verified!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Entered code is incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Function GetVolumeSerial(driveLetter As String) As String
        Try
            Dim searcher As New ManagementObjectSearcher($"SELECT * FROM Win32_LogicalDisk WHERE DeviceID='{driveLetter}:'")
            For Each mo As ManagementObject In searcher.Get()
                Return mo("VolumeSerialNumber").ToString()
            Next
        Catch ex As Exception
            MessageBox.Show("Error getting serial number: " & ex.Message)
        End Try
        Return String.Empty
    End Function


    Private Sub CancelButton_Click(sender As Object, e As EventArgs)
        If CancelButton1.Text = "Cancel" Then
            Me.Close()
            Application.Exit()
        Else
            frm_user_pass.Show()
            Me.Close()
        End If
    End Sub


    Private Sub CreatChkMastDB()
        Dim tableScripts As String() = {
         "IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserNameDetails') " &
        "BEGIN CREATE TABLE UserNameDetails (UserType NVARCHAR(40), UserName NVARCHAR(40), Password NVARCHAR(40), ClPassword NVARCHAR(40)) END",
         "IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'COMMPKRTB') " &
        "BEGIN CREATE TABLE COMMPKRTB (" &
        "PKRNO INT PRIMARY KEY, PKRNAME NVARCHAR(15), CommunicationType varchar(40),PORT INT,PortName varchar(40), BaudRate INT,Parity varchar(40), IP NVARCHAR(15), PKRTYPE NVARCHAR(15), " &
        "RESPTI INT, TOTALSPOUT INT,Mode VARCHAR(80),IsCheckWeigher VARCHAR(80),CWPortName varchar(40), CWBaudRate INT,CWParity varchar(40) ) END",
        "IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AUTO_CORRECTION_LOGS') " &
        "BEGIN CREATE TABLE AUTO_CORRECTION_LOGS (" &
        "RecordId INT PRIMARY KEY,PackerNo int, SpoutNo varchar(40), SampleWeights varchar(100), FINALWT varchar(40), " &
        "OLD_FINALADJUSTMENTWT varchar(40), NEW_FINALADJUSTMENTWT varchar(40),TimeDtFrmt datetime, AutoCorrectionRemarks varchar(300)) END",
        $"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SpoutData') " &
        $"BEGIN CREATE TABLE [SpoutData] (SINO INT IDENTITY(1,1) PRIMARY KEY,PackerNo INT, PackerName VARCHAR(40), SpoutNo VARCHAR(40), SpoutName VARCHAR(80), CodeNumber INT, StoreTime INT, ActCount INT, AccWt FLOAT, AccWtStr VARCHAR(80), ActualWt FLOAT, TargetWt FLOAT, OverWt FLOAT, UnderWt FLOAT, Deviation FLOAT, NoofBags INT, Field01 VARCHAR(20), Field02 VARCHAR(20), Field03 VARCHAR(20), Field04 VARCHAR(20), Rem1 VARCHAR(80), Mode VARCHAR(80),IntStDt INT,Current_FinalWt float,TimeDtFrmt DATETIME) END",
        $"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LOGS') " &
        $"BEGIN CREATE TABLE [LOGS] (RecordId INT IDENTITY(1,1) PRIMARY KEY,PackerNo INT, SpoutNo VARCHAR(40),Old_SP2 VARCHAR(40), New_SP2 VARCHAR(40),SampleWeights VARCHAR(100),FINALWT VARCHAR(40) ,OLD_FINALWT VARCHAR(40), NEW_FINALWT VARCHAR(40),TimeDtFrmt DATETIME) END",
        $"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'IdleTime') " &
        $"BEGIN CREATE TABLE [IdleTime] (RecordId INT IDENTITY(1,1) PRIMARY KEY,PackerNo INT, SpoutNo VARCHAR(40),StartTime DATETIME,EndTime DATETIME,TimeDiff VARCHAR(40),Remarks VARCHAR(40)) END",
        $"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RemarksData') " &
        $"BEGIN CREATE TABLE [RemarksData] (Id INT IDENTITY(1,1) PRIMARY KEY,Remarks VARCHAR(255), CreatedAt datetime) END",
        $"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ManualWt') " &
        $"BEGIN CREATE TABLE [ManualWt] (RecordId INT IDENTITY(1,1) PRIMARY KEY,PackerNo INT, SpoutNo VARCHAR(40),DateTime DATETIME,Weight  VARCHAR(40)) END"
        }

        Try
            For Each script As String In tableScripts
                helper.ExecuteNonQuery(script)
            Next

            Dim userSeedSql As String =
            "IF NOT EXISTS (SELECT 1 FROM UserNameDetails) " &
            "INSERT INTO UserNameDetails (UserType, UserName, Password, ClPassword) " &
            "VALUES (@ut, @un, @pw, @cpw)"

            helper.ExecuteNonQuery(userSeedSql, {
                New SqlParameter("@ut", "admin"),
                New SqlParameter("@un", "admin"),
                New SqlParameter("@pw", "admin"),
                New SqlParameter("@cpw", "admin")
            })

        Catch ex As Exception
            MessageBox.Show("SQL Server Error: " & ex.Message)
            SQLCONN.Show()
            Me.Close()
        End Try
    End Sub
    Private Sub Design()
        ' Basic form setup
        Me.Text = "Loading"
        Me.Size = New Size(600, 400)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog

        ' Master layout
        Dim masterLayout As New TableLayoutPanel With {
        .Dock = DockStyle.Fill,
        .RowCount = 5,
        .ColumnCount = 1
    }

        masterLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 40))  ' Title
        masterLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 50))   ' Main content
        masterLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 45))  ' Button
        masterLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 25))  ' Status

        ' Title label
        titleLabel = CreateStyledLabel("CEMENT PACKER MONITORING AND LOGGING SOFTWARE", 18)
        titleLabel.BackColor = Color.FromArgb(100, 100, 100)
        titleLabel.ForeColor = Color.White
        masterLayout.Controls.Add(titleLabel, 0, 0)

        ' Content layout
        Dim contentLayout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .RowCount = 5,
            .ColumnCount = 1
        }

        contentLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 20))     ' Spacer
        contentLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 20))     ' Version
        contentLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 20))     ' Label4
        contentLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 30))    ' Progress bar

        ' Labels
        centerLabel = CreateStyledLabel("Licensed Version : Version No.1.0", 12, False)
        Label4 = CreateStyledLabel("Ready to Start the Application", 11, False)

        contentLayout.Controls.Add(New Label() With {.Height = 10}, 0, 0)
        contentLayout.Controls.Add(centerLabel, 0, 1)
        contentLayout.Controls.Add(Label4, 0, 2)

        ' Track bar (gray)
        PanelTrackBar = New Panel With {
            .BackColor = Color.Transparent,
            .Height = 20,
            .Dock = DockStyle.Fill,
            .Margin = New Padding(10, 5, 10, 5)
        }

        ' Progress fill (green)
        PanelProgressFill = New Panel With {
            .BackColor = Color.FromArgb(200, 200, 200),
            .Height = 20,
            .Width = 0,
            .Left = 0,
            .Top = 0
        }
        ' Truck image inside the fill panel (docks to right)
        PictureBoxImageBar = New PictureBox With {
            .Image = My.Resources.truckLoading,
            .SizeMode = PictureBoxSizeMode.Zoom,
            .Height = 20,
            .Width = 60,
            .Dock = DockStyle.Right,
            .BackColor = Color.Transparent
        }

        ' Nest the controls properly
        PanelProgressFill.Controls.Add(PictureBoxImageBar)
        PanelTrackBar.Controls.Add(PanelProgressFill)
        contentLayout.Controls.Add(PanelTrackBar, 0, 4)

        ' Add to master layout
        masterLayout.Controls.Add(contentLayout, 0, 1)
        ' Cancel button
        CancelButton1 = New Button With {
            .Text = "Cancel",
            .Font = New Font("Segoe UI", 11, FontStyle.Bold),
            .Dock = DockStyle.Fill,
            .Height = 35
        }
        AddHandler CancelButton1.Click, AddressOf CancelButton_Click
        masterLayout.Controls.Add(CancelButton1, 0, 2)

        ' Status label
        statusLabel = CreateStyledLabel("", 9, False)
        statusLabel.Dock = DockStyle.Fill
        statusLabel.BackColor = Color.DarkGray
        statusLabel.ForeColor = Color.White
        masterLayout.Controls.Add(statusLabel, 0, 3)

        ' Add everything to form
        Me.Controls.Add(masterLayout)
    End Sub


    Private Function CreateStyledLabel(text As String, fontSize As Integer, Optional bold As Boolean = True) As Label
        Return New Label With {
            .Text = text,
            .Font = New Font("Arial", fontSize, If(bold, FontStyle.Bold, FontStyle.Regular)),
            .ForeColor = Color.Black,
            .BackColor = Color.Transparent,
            .AutoSize = False,
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleCenter
        }
    End Function
End Class
