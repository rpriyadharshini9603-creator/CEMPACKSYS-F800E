Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Data.SqlClient
Imports System.Collections.Concurrent
Imports System.Threading
Imports System.IO
Imports System.IO.Ports
Public Class Measurement
    Public Property Timestamp As DateTime
    Public Property Weight As String

    Public Sub New(timestamp As DateTime, weight As String)
        Me.Timestamp = timestamp
        Me.Weight = weight
    End Sub
End Class
Public Class LimitedSizeQueue(Of T)
    Private ReadOnly _queue As New Queue(Of T)()
    Private ReadOnly _maxCapacity As Integer

    Public Sub New(maxCapacity As Integer)
        If maxCapacity <= 0 Then
            Throw New ArgumentOutOfRangeException("maxCapacity", "Max capacity must be greater than zero.")
        End If
        Me._maxCapacity = maxCapacity
    End Sub

    Public Function GetTolist() As List(Of T)
        Return _queue.ToList()
    End Function
    Public Sub New()
    End Sub

    Public Sub Enqueue(item As T)
        _queue.Enqueue(item)
        If _queue.Count > _maxCapacity Then
            _queue.Dequeue() ' Remove the oldest item
        End If
    End Sub

    Public Function Dequeue() As T
        Return _queue.Dequeue()
    End Function
    Public Function Peek() As T
        Return _queue.Peek()
    End Function

    Public ReadOnly Property Count As Integer
        Get
            Return _queue.Count
        End Get
    End Property

    Public Sub Clear()
        _queue.Clear()
    End Sub
    Public Function Contains(item As T) As Boolean
        Return _queue.Contains(item)
    End Function

    Public Function ToArray() As T()
        Return _queue.ToArray()
    End Function
End Class



Public Class StatusUpdateEventArgs
    Inherits EventArgs

    Public Property Controller As SpoutController
    Public Property Message As String

    Public Sub New(controller As SpoutController, message As String)
        Me.Controller = controller
        Me.Message = message
    End Sub
End Class




Public Class Formatdatetime
    Public Sub New(ClInstr As String, _spout As SpoutController)
        Dim datetimeStr As String = ClInstr.Substring(ClInstr.Length - 6)
        Dim inputFormat As String = "HHmmss"
        Dim dt As DateTime = DateTime.ParseExact(datetimeStr, inputFormat, Globalization.CultureInfo.InvariantCulture)
        _spout.datetime = dt.ToString("HH:mm:ss")
    End Sub
End Class
Public Class SaveParameters
    Public Sub New(CommandName As String, query As String, value As String, _spout As SpoutController)
        Dim parsedValue As Double
        If Double.TryParse(value?.ToString(), parsedValue) Then
            Select Case CommandName
                Case "Final"
                    _spout.FinalWeight = parsedValue
                Case "SP2"
                    _spout.SP2 = parsedValue
                Case "FinalAdjustWeight"
                    _spout.FinalAdjustmentWeight = parsedValue
                Case "CPS"
                    _spout.CPS = parsedValue
                Case "Under"
                    _spout.Under = parsedValue
                Case "Over"
                    _spout.Over = parsedValue
                Case "ReadCount"
                    _spout.AccumulationCount = parsedValue
            End Select
        End If
    End Sub

End Class
Public Class ToolStripEventArgs
    Inherits EventArgs
    Public Property PackerId As Integer
    Public Property MessageType As String
    Public Property Message As String
End Class
Public Class ManualWeightEventAgrs
    Public Property PackerId As Integer
    Public Property SpoutId As Integer
    Public Property DateTime As DateTime
    Public Property Weight As Double
End Class
Public Class ParameterReceivedEventAgrs
    Public Property ParameterValue As String
    Public Property TextBoxRef As TextBox
End Class


Public Class ProgressbarEventAgrs
    Public Property PackerId As Integer
    Public Property MinValue As Integer
    Public Property MaxValue As Integer
    Public Property ProgessValue As Integer
    Public Property Progressbar As ProgressBar
End Class

Public Class ProcessResult
    Public Function FormatDataNew(CommandName As String, Inbuf As String, DecimalPoint As Integer, QueryName As String) As String
        Dim Cmd As String = Mid(CommandName, 7).Replace(vbCr, "")
        Dim StartPosition As Integer = InStr(1, Inbuf, Cmd)
        Dim Value As String
        Dim input As String = ""

        If StartPosition = 0 Then Return ""
        Try

            If QueryName = "SP3TargetTime" Then
                Value = Mid(Inbuf, StartPosition + 3, 2)
            ElseIf QueryName = "SP3AllowableTime" Then
                Value = Mid(Inbuf, StartPosition + 6, 2)
            ElseIf QueryName = "Timer" Then
                Value = Mid(Inbuf, StartPosition + 3, 2)
            ElseIf QueryName = "Timer2" Then
                Value = Mid(Inbuf, StartPosition + 6, 2)
            ElseIf QueryName = "MotionDetection" Then
                Value = Mid(Inbuf, StartPosition + 3, 2)
            ElseIf QueryName = "MotionDetection2" Then
                Value = Mid(Inbuf, StartPosition + 6, 2)
            ElseIf QueryName = "ZeroTracking" Then
                Value = Mid(Inbuf, StartPosition + 3, 2)
            ElseIf QueryName = "ZeroTracking2" Then
                Value = Mid(Inbuf, StartPosition + 6, 2)
            ElseIf QueryName = "Write Code" Then
                Return ""
            ElseIf QueryName = "ReadCount" Then
                Value = Mid(Inbuf, StartPosition + 2, 5)
            Else
                Value = Mid(Inbuf, StartPosition + 3, 5)
            End If

            If DecimalPoint = 0 Then
                Return CDbl(Value).ToString().PadLeft(5, "0"c)
            ElseIf DecimalPoint = 1 Then
                input = (CInt(Value) / 10).ToString("0.0")
            ElseIf DecimalPoint = 2 Then
                input = (CInt(Value) / 100).ToString("0.00")

            ElseIf DecimalPoint = 3 Then
                input = (CInt(Value) / 1000).ToString("0.000")

            ElseIf DecimalPoint = 4 Then
                input = (CInt(Value) / 10000).ToString("0.0000")

            End If
        Catch ex As Exception

            Console.WriteLine(ex.Message)

        End Try
        Return input.PadLeft(5, "0"c)

    End Function


    Public Function FormatParameter(Value As String, DecimalPoint As Integer) As String
        Dim input As String = ""
        If DecimalPoint = 0 Then
            Return CDbl(Value).ToString().PadLeft(5, "0"c)
        ElseIf DecimalPoint = 1 Then
            Input = (CInt(Value) / 10).ToString("0.0")
        ElseIf DecimalPoint = 2 Then
            Input = (CInt(Value) / 100).ToString("0.00")

        ElseIf DecimalPoint = 3 Then
            Input = (CInt(Value) / 1000).ToString("0.000")

        ElseIf DecimalPoint = 4 Then
            Input = (CInt(Value) / 10000).ToString("0.0000")

        End If
        Return input.PadLeft(5, "0"c)
    End Function


    Public Function Configbat(Value As String, ND As Integer, DP As Integer)
        Dim SD As String
        Dim AD As Double
        AD = Val(Value) / 10 ^ DP
        If AD <> 0 Then
            If InStr(1, CStr(AD), ".") = 0 Then
                SD = CStr(Format(AD, "#000.00"))
            ElseIf InStr(1, CStr(AD), ".") = 1 Then
                SD = CStr(Format(AD, "#0.0000"))
            ElseIf InStr(1, CStr(AD), ".") = 2 Then
                SD = CStr(Format(AD, "#0.0000"))
            ElseIf InStr(1, CStr(AD), ".") = 3 Then
                SD = CStr(Format(AD, "#00.000"))
            ElseIf InStr(1, CStr(AD), ".") = 4 Then
                SD = CStr(Format(AD, "#000.00"))
            Else
                SD = CStr(Format(AD, "#000.00"))
            End If
        Else
            SD = CStr(Format(AD, "000.00"))
        End If
        Return SD
    End Function
    Public Function CheckCRC_8(OutputString As String) As Boolean
        Dim CRC As Integer
        Dim RETSTR As String
        Dim I As Integer, length As Integer
        Dim CheckStr = OutputString.Substring(0, OutputString.Length - 2)
        Dim CalculatedCrc As String
        length = Len(CheckStr)
        CRC = 0
        For I = 1 To length
            CRC = CRC Xor Asc(Mid(CheckStr, I, 1))
        Next I
        RETSTR = CStr(Asc(CRC))
        If Len(RETSTR) = 1 Then
            RETSTR = "0" & Trim(RETSTR)
        End If
        CalculatedCrc = CheckStr & RETSTR
        If CalculatedCrc = OutputString Then
            Return True
        Else
            Return False
        End If
    End Function


End Class







































