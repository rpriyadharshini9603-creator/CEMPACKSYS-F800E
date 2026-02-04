Imports System.Collections.Concurrent
Imports System.IO.Ports
Imports System.Net.Sockets

Public Class Packer
    Public Property DeviceIdentifier As String
    Public Property CommunicationType As String
    Public Property IPAddress As String
    Public Property Port As Integer
    Public Property PortName As String
    Public Property BaudRate As Integer
    Public Property Parity As String
    Public Property DataBits As Integer
    Public Property StopBits As StopBits
    Public Property CheckWeigher As Boolean
    Public Property CWPort As Integer
    Public Property CWPortName As String
    Public Property CWBaudRate As String
    Public Property CWParity As Parity
    Public Property CWDataBits As Integer
    Public Property CWStopBits As StopBits
    Public Property DeviceId As Integer
    Public Property DeviceModeType As String
    Public Property IsConnected As Boolean
    Public Property Client As TcpClient 'Persistent TCP client for this device
    Public Property serial As SerialPort
    Public Property QueryTimeOut As Integer
    Public Property MaxSpout As Integer
    Public Property BagCount As Long
    Public Property ControllerModel As String
    Public Property PackerTph As Long
    Public Property SpoutList As New List(Of SpoutController)()
    Public ReadOnly Property CommandQueue As New ConcurrentQueue(Of QueuedCommand)()
    Public Property ErrorLog As New List(Of String)()
    Public Property IsParameterRequested As Boolean
    Public Property IsInitalParameterDownloaded As Boolean
End Class
Public Class SpoutController
    Public Property PackerId As Integer
    Public Property PackerName As String
    Public Property SpoutName As String
    Public Property SpoutId As Integer
    Public Property CodeNo As Integer
    Public Property ControllerModel As String
    Public Property DeviceModeType As string
    Public Property Query As List(Of String)
    Public Property BagCount As Long
    Public Property AccumulationCount As Long
    Public Property CPS As Double
    Public Property FinalWeight As Double
    Public Property FinalAdjustmentWeight As Double
    Public Property oldSP2 As Double
    Public Property SP2 As Double
    Public Property RealCorrection As Double
    Public Property BatchStartTime As DateTime
    Public Property TotalWtTonnes As Double
    Public Property AutoCorrectionRemarks As String
    Public Property Over As Double
    Public Property Under As Double
    Public Property TotalPackerBagCount As Long
    Public Property CurrentCode As Integer
    Public Property LastDischargedBagWeight As String
    Public Property LastDIschargedBagDateTime As DateTime
    Public Property Deviation As Double
    Public Property AverageDeviation As Double
    Public Property datetime As String
    Public Property SampleCount As Integer
    Public Property SampleWeights As New List(Of Double)
    Public Property SampleWeightStr As String
    Public Property Last20Bags As New LimitedSizeQueue(Of Measurement)(20)
    Public Property SpoutParamter As Dictionary(Of String, String)
    Public Property IsConnected As Boolean = True
    Public Property NewDataFlag As Boolean
    Public Property NoReplyCount As Integer
    Public Property EmptyRound As Integer
End Class
Public Class QueuedCommand
    Public Property Name As String
    Public Property SpoutId As Integer
    Public Property TotalBytes As Integer
    Public Property DecimalPoint As Integer
    Public Property CommandData As List(Of String)
    Public Property QueryType As String = "Read"
    Public Property TextBoxReference As TextBox
    Public Sub New(name As String, SpoutId As Integer, TotalBytes As Integer, data As List(Of String), TextBoxReference As TextBox, DecimalPoint As Integer, QueryType As String)
        Me.Name = name
        Me.SpoutId = SpoutId
        Me.TotalBytes = TotalBytes
        Me.CommandData = data
        Me.TextBoxReference = TextBoxReference
        Me.DecimalPoint = DecimalPoint
        Me.QueryType = QueryType
    End Sub
End Class



