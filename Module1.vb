Imports System.IO

Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Data.SqlClient
Imports System.Collections.Concurrent
Imports System.Runtime.InteropServices

Module Module1
    Public LastModifiedStr As String = "GPAC025/ULTRATECH/AWARPUR/F800E/24-09-2025/10:50/V1.5/RP"

    Public packerCount As Integer ' Set the desired packer count here
    Public spoutCount As Integer  ' Set the spout count (rows)
    Public extractedValue As String

    Public Const TITLE1 As String = "ACCEMMAINSCR"
    Public Const TITLE2 As String = "PKRAPPLICATION"
    Public Const TITLE11 As String = "MSOFFICEEXE"
    Public Const TXTFNAME As String = "ProfileStr.Txt"
    Public Const ERRFNAME As String = "ErrFile.Txt"

    Public MAXPKR = packerCount
    Public MAXSPOUT = spoutCount - 1  'maximum allowed spout per packer for this software
    Public Const RPLYNRWT = 0   'DATA WRITTEN AND HENCE NO WAIT TIME FOR REPLY IS PASSED TO COMMOPRESULT1
    Public Const DLYTMR1 = 700      'DELAY ROUTINE TIMER

    Public PackerCommunication As CommunicationV1
    Public _checkwire As CheckWeigher
    Public Packers As New List(Of Packer)()
    Public PackersCheckweigher As New List(Of Packer)()
    Public ctab As String = "1"
    Public Const SQLDBPRESTR = "UTCLAWPR4PKF8ETCP" 'THIS STRING IS USED FOR DB PRE ADD ACC THONDEBAVI_701S 2PKR YEAR 1516

    Public Const JOBNO = "215"  'JOBNO
    Public Const DEFDRVNAME = "C"
    Public Const DataDBMST = "RecordDB"
    Public Const SUBFOLDNAME = "UTCLAWPR4PKF8ETCP"   'CEMENT PACKER MONITORING SYSTEM
    Public Const MASTERFILE = "MasterRec"
    'Public Const MASTERFILES = "MasterRecord"
    Public Const MASTERFILES = "MDBUTROPAR701S"
    'Public Const DATAMAST = "RecordDB"
    Public Const DATAMAST = "RecordDB"
    Public Const DEFCLPW = "ACCSYS"
    Public Const SPECIALPASS = "12345"
    Public Const REPEXTENSION = "SPEC123"
    Public Const PKRIDSTART = 1 ''

    Public CurrentFormOpened As String
    'Open Array
    Public COMHLTFL(10, 10) As Boolean        'COMMUNICATION HEALTHY FLAG
    Public CUMWTDBL(10, 10) As Double

    Public CdData(10, 10, 10) As String
    Public FINALACC(10, 10) As String
    Public LOGSTTIME(10, 10) As Long

    Public MSDG1UPDFL As Boolean
    Public MSDG2UPDFL(10, 10) As Boolean


    Public LASTHIACC(10, 10) As Double
    Public LASTLOACC(10, 10) As Double


    ''single variable
    Public USERTYPESTR As String
    Public USERNAMESTR As String
    Public USERPASSSTR As String
    Public USERPASSFL As Boolean        'THIS FLAG IS SET WHEN THE APPLICATION CLOSE PASSWORD

    Public DBLMAXLMT As Double
    Public DBLMINLMT As Double

    Public GRIDWIDTH As Double
    Public GRIDHEIGHT As Double
    Public GRIDSCALEFL As Boolean
    Public MINACC(MAXSPOUT, MAXPKR) As Double
    Public MAXACC(MAXSPOUT, MAXPKR) As Double


    Public GRNTXT As String
    Public RDTXT As String
    Public BLUTXT As String
    Public FILELOC As String
    Public FOLDERNAME As String = "C:\UTCLAWPR4PKF8ETCP\"
    Public COMBUSYFL(MAXPKR) As Boolean
    Public CFGPASFL As Boolean
    Public LOGOSTR As String

    Public RDCDSTR As String

    Public isClosing As Boolean
    Public IsApplicationLoaded() As Boolean
    Public Const DCTYPE1 = "CTRLMODE"

    Public gridlabel() As TextBox = New TextBox(3) {}
    Public whiteIndex As Integer = 0
    Public IndCon As New SqlConnection

    Public Declare Sub ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" _
    (ByVal hwnd As Long, ByVal lpOperation As String,
     ByVal lpFile As String, ByVal lpParameters As String,
     ByVal lpDirectory As String, ByVal nShowCmd As Long)


    Public Declare Function GetVolumeInformation Lib _
   "kernel32.dll" Alias "GetVolumeInformationA" _
   (ByVal lpRootPathName As String,
   ByVal lpVolumeNameBuffer As String,
   ByVal nVolumeNameSize As Integer,
   lpVolumeSerialNumber As Long,
   lpMaximumComponentLength As Long,
   lpFileSystemFlags As Long,
   ByVal lpFileSystemNameBuffer As String,
   ByVal nFileSystemNameSize As Long) As Long
    Public LICHKFL As Boolean
    Public LIOKFL As Boolean
    Public LISTR(3) As String
    Public LICENCENO As String
    Public LICODE As String

    Public LICDATEVL As Long
    Public LICDATEVL1 As Date
    Public LICKEYFL As Boolean

    Public Function GetSetting(key As String, defaultValue As String) As String
        Dim value As String = RestoreSettings(TITLE1, "Properties", key)
        Return If(String.IsNullOrEmpty(value), defaultValue, value)


    End Function

    Public Function ConvIDToStr(IDN As Integer, PKC As Integer) As String
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
    Public Function Check_CRC(InputString As String) As Boolean
        Dim CRC As String
        Dim Data As String
        If Len(InputString) < 3 Then
            Return False
        End If
        CRC = Mid(InputString, Len(InputString) - 1, 2)
        Data = Mid(InputString, 1, Len(InputString) - 2)
        If CRC = CRC_16(Data) Then
            Return True
        Else
            Return False
        End If

        Return False
    End Function
    Public Function CRC_16(OutputString As String) As String
        Dim Generator, CRC As Long
        Dim I As Integer, J As Integer, length As Integer
        Dim Bit As Boolean
        Dim temp As Byte
        length = Len(OutputString)
        CRC = 65535
        Generator = 40961
        For I = 1 To length
            temp = Asc(Mid(OutputString, I, 1))
            CRC = CRC Xor temp
            For J = 1 To 8
                Bit = CRC And 1
                CRC = CRC \ 2
                If Bit = True Then
                    CRC = CRC Xor Generator
                End If
            Next J
        Next I
        Return Chr(CRC Mod 256) & Chr(CRC \ 256)
    End Function

    Public Function RestoreSettings(FrmName As String, ControlName As String, ControlProp As String) As String
        Dim X
        Dim sSection As String, sEntry As String, sDefault As String
        Dim sRetBuf As String, iLenBuf As Integer, sFileName As String
        sSection = TITLE1 & "|" & FrmName & "|" & ControlName & "|" & ControlProp
        sEntry$ = ControlProp
        sDefault$ = ""
        sRetBuf$ = Space(255)   '256 null characters
        iLenBuf% = Len(sRetBuf$)
        FILELOC = FOLDERNAME
        sFileName$ = CStr(FILELOC & "\ProfileStr.Txt")
        X = GetPrivateProfileString(sSection$, sEntry$,
        sDefault$, sRetBuf$, iLenBuf%, sFileName$)
        RestoreSettings = Microsoft.VisualBasic.Left$(sRetBuf$, X)

    End Function
    '===========================================================================================================================
    Public Declare Function GetPrivateProfileString Lib "kernel32" Alias _
  "GetPrivateProfileStringA" (ByVal lpApplicationName As String,
  ByVal lpKeyName As String,
  ByVal lpDefault As String,
  ByVal lpReturnedString As String,
  ByVal nSize As Integer, ByVal lpFileName As String) As Integer


    Public Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (
    ByVal lpApplicationName As String,
    ByVal lpKeyName As String,
    ByVal lpString As String,
    ByVal lpFileName As String) As Long


    Public Function SaveProfile_Setting(FrmName As String, ControlName As String, ControlProp As String, SaveText As Object) As Long
        Dim sSection As String
        Dim sEntry As String
        Dim sString As String
        Dim sFileName As String
        Dim FILELOC As String
        FILELOC = FOLDERNAME
        sFileName = Path.Combine(FILELOC, "ProfileStr.txt")

        If Not Directory.Exists(FILELOC) Then
            Directory.CreateDirectory(FILELOC)
        End If

        If Not File.Exists(sFileName) Then
            File.WriteAllText(sFileName, "")
        End If

        sSection = TITLE1 & "|" & FrmName & "|" & ControlName & "|" & ControlProp
        sEntry = ControlProp
        sString = SaveText.ToString()
        Return WritePrivateProfileString(sSection, sEntry, sString, sFileName)
    End Function

    Public Sub CentralErrhandler(methodName As String, className As String)
        MessageBox.Show($"Error in {className}.{methodName}: {Err.Description}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub


    Public localValue As String = ""
    Public localPkr As Integer = 0
    Public localSpout As String = ""

    Public DBHostName As String = ""
    Public DBPort As String = ""
    Public DBName As String = "UTCLAWPR4PKF8ETCP"
    Public DBUserId As String = ""
    Public DBPassword As String = ""

    Public recom As Boolean
    Public TCPconnflg As Boolean
    Public WGHFL As Boolean
    Public PERFL As Boolean

    Public packerPanels As New List(Of TableLayoutPanel)
    Public TextBoxDict As New Dictionary(Of String, TextBox)
    Public ProgressBar As New Dictionary(Of String, ProgressBar)
    Public ProgressBarPanel As New Dictionary(Of String, Panel)
    Public packerEndpoints As New Dictionary(Of String, (Ip As String, Port As Integer))
    Public TcpClients As New List(Of TcpClient)
    Public count As Integer
    Public Last20Responses As New ConcurrentQueue(Of KeyValuePair(Of String, String))
    Public storeFL As Boolean


    Public sample() As Integer
    Public coefficient() As Double
    Public CorrectionRangeFrom() As Double
    Public CorrectionRangeTo() As Double

    Public ValidWeightFrom() As Double
    Public ValidWeightTo() As Double

    Public CorrectionLimitFrom() As Double
    Public CorrectionLimitTo() As Double

    Public dequeuedItems As New List(Of Double)()
    Public IsAutoCorrectionEnabled() As Boolean

    Public currenttime As DateTime
    Public SampleWeights As List(Of Double) = Nothing
    Public IsCopy As Boolean
    Public helper As SQLHelper
    Public NewDB As Boolean
    Public allowClose As Boolean = False
    Public closingpassword As String
    Public newpacker As Boolean = False
End Module
