Imports System.IO
Imports System.IO.Ports
Imports System.Net.Sockets
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Public Class CommunicationV1
    Private Shared ReadOnly cancellationTokenSource As New CancellationTokenSource()
    Private _ProcessResult As New ProcessResult
    Private ReadOnly _cancellationTokenSource As CancellationTokenSource
    Public _queryLoopTask As Task
    Public _DownloadParameterloop As Task
    Public Event DeviceDataReceived As EventHandler(Of SpoutController)
    Public Event DeviceParameterReceived As EventHandler(Of ParameterReceivedEventAgrs)
    Public Event UpdateBatchParameterProgressBar As EventHandler(Of ProgressbarEventAgrs)
    Public Event UpdateSystemParameterProgressBar As EventHandler(Of ProgressbarEventAgrs)
    Public Event UpdateSystemCopyParameterProgressBar As EventHandler(Of ProgressbarEventAgrs)
    Public Event UpdateMinicProgressBar As EventHandler(Of ProgressbarEventAgrs)
    Public Event CopyCompleteEvent As EventHandler(Of String)
    Dim ResponseFlg As Boolean
    Public Event ManualWeightReceived As EventHandler(Of ManualWeightEventAgrs)
    Public Event AutoCorrectDataReceived As EventHandler(Of SpoutController)
    Dim _UniPulse As New Unipulse
    Dim InitialParameterDownloaded As Boolean
    Public Event ToolStripUpdate As EventHandler(Of ToolStripEventArgs)

    Public Sub New(availablePacker As List(Of Packer))
        If availablePacker IsNot Nothing Then
            If availablePacker.Count > 0 Then
                For Each _Packer As Packer In availablePacker
                    _cancellationTokenSource = New CancellationTokenSource()
                    _queryLoopTask = Task.Run(Async Function()
                                                  Await StartCommunication(_Packer, 45, _Packer.QueryTimeOut, _cancellationTokenSource.Token) ' F_800E.RDACCBYTE
                                              End Function, _cancellationTokenSource.Token)
                Next
            End If
        End If
    End Sub

    Private Async Sub GetMustParamter(PackerId As Integer)
        Dim CurrentDatetime As DateTime = Date.Now()
        Dim Year = CurrentDatetime.Year Mod 100
        Dim Month = CurrentDatetime.Month.ToString("D2")
        Dim Day = CurrentDatetime.Day.ToString("D2")
        Dim hour = CurrentDatetime.Hour.ToString("D2")
        Dim Minute = CurrentDatetime.Minute.ToString("D2")
        Dim Second = CurrentDatetime.Second.ToString("D2")
        Dim _Packer = Packers.FirstOrDefault(Function(e) e.DeviceId = PackerId)
        Dim TmpSpout As New SpoutController With {.PackerId = _Packer.DeviceId}

        Dim TmpQuery As String = $"NO9999CT{Year}{Month}{Day}{hour}{Minute}{Second}{vbCr}"
        Dim QueryStr As New List(Of String) From {TmpQuery}
        'Dim _cancellationTokenSource = New CancellationTokenSource()
        'Dim Inbuf1 = Await ComOutputResult(_Packer, TmpSpout, QueryStr, QueryStr.Length, _Packer.QueryTimeOut, _cancellationTokenSource.Token, "Write")

        EnqueueDeviceCommand(PackerId, 1, 0, "TimeUpdate", QueryStr, , 0, "Write")

        Try

            For Each _Spout As SpoutController In _Packer.SpoutList
                Dim FunctionsToCall() As String = Nothing
                If _Packer.ControllerModel = "F800E" Then
                    If _Packer.DeviceModeType = "ControllerMode" Or _Packer.CheckWeigher = "True" Then
                        FunctionsToCall = {"Over", "Under", "Final", "FinalAdjustWeight"}
                    Else
                        FunctionsToCall = {"Final", "FinalAdjustWeight"}
                    End If
                ElseIf _Packer.ControllerModel = "F701S" Then
                    FunctionsToCall = {"Over", "Under", "Final"}
                ElseIf _Packer.ControllerModel = "F800" Then
                    FunctionsToCall = {"Over", "Under", "Final", "FinalAdjustWeight"}
                End If


                Dim allMethods = GetType(Unipulse).GetMethods()
                Dim validMethods = allMethods.Where(Function(m) FunctionsToCall.Contains(m.Name)).ToList()
                For Each m In validMethods
                    Dim ReadParameterQuery As QueryInfo = m.Invoke(_UniPulse, New Object() {_Spout.ControllerModel, _Spout.SpoutId, ""})

                    EnqueueDeviceCommand(PackerId, _Spout.SpoutId, ReadParameterQuery.ResponseLength, m.Name, ReadParameterQuery.Query, , ReadParameterQuery.DecimalFormat)
                Next
            Next

        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

        _Packer.IsParameterRequested = True
    End Sub
    Public Async Function StartCommunication(_Packer As Packer, TotalBytes As Integer, Timeout As Integer, ct As CancellationToken) As Task(Of String)
        Dim result As String = String.Empty
        Dim Inbuf
        Try
            GetMustParamter(_Packer.DeviceId)
            While Not ct.IsCancellationRequested
                Dim Command As QueuedCommand = Nothing
                Dim ParameterCount = _Packer.CommandQueue.Count
                Dim CurrentCount As Integer
                While _Packer.CommandQueue.TryDequeue(Command)
                    CurrentCount += 1
                    Try
                        Dim _Spout As SpoutController = _Packer.SpoutList.FirstOrDefault(Function(d) d.SpoutId = Command.SpoutId)
                        If Command.CommandData IsNot Nothing Then
                            For Each Query As String In Command.CommandData
                                If _Packer.CommunicationType = "Ethernet" Then
                                    Inbuf = Await ComOutputResult(_Packer, _Spout, Query, Command.TotalBytes, Timeout, ct, Command.QueryType)
                                Else
                                    Inbuf = Await SerialOutputResult(_Packer, _Spout, Query, Command.TotalBytes, Timeout, ct, Command.QueryType)
                                End If

                                If InStr(Query, "W", 0) > 0 Or InStr(Query, "RH", 0) > 0 Then
                                    If Inbuf <> "False" And Inbuf <> "" Then
                                        Dim value = _ProcessResult.FormatDataNew(Query, Inbuf, Command.DecimalPoint, Command.Name)
                                        Dim saveparameter As New SaveParameters(Command.Name, Query, value, _Spout)
                                        If Command.TextBoxReference IsNot Nothing Then
                                            Dim ParameterReceived = New ParameterReceivedEventAgrs With {
                                                                           .ParameterValue = value,
                                                                           .TextBoxRef = Command.TextBoxReference
                                                                       }
                                            RaiseEvent DeviceParameterReceived(Me, ParameterReceived)
                                        End If
                                        If InStr(Query, "RH", 0) > 0 Then
                                            RaiseEvent DeviceDataReceived(Me, _Spout)
                                        End If
                                    End If
                                End If
                                If CurrentFormOpened = "frmConfigBatProp" Then
                                    Dim Progess = New ProgressbarEventAgrs With {
                                                                  .MaxValue = ParameterCount,
                                                                  .MinValue = 0,
                                                                  .ProgessValue = CurrentCount
                                                                  }
                                    RaiseEvent UpdateBatchParameterProgressBar(Me, Progess)
                                ElseIf CurrentFormOpened = "frmParameterScr" Then
                                    Dim Progess = New ProgressbarEventAgrs With {
                                                                   .MaxValue = ParameterCount,
                                                                  .MinValue = 0,
                                                                  .ProgessValue = CurrentCount
                                }
                                    RaiseEvent UpdateSystemParameterProgressBar(Me, Progess)
                                ElseIf CurrentFormOpened = "frmCopyParameters" Then
                                    Dim Progess = New ProgressbarEventAgrs With {
                                                                  .MaxValue = ParameterCount,
                                                                  .MinValue = 0,
                                                                  .ProgessValue = CurrentCount
                                }
                                    RaiseEvent UpdateSystemCopyParameterProgressBar(Me, Progess)
                                    If IsCopy And Command.QueryType = "Write" Then
                                        Dim Message = $"Parameter {Command.Name} Updated for Spout no {_Spout.SpoutId } successfully"
                                        RaiseEvent CopyCompleteEvent(Me, Message)
                                    End If
                                ElseIf CurrentFormOpened = "frmMimic1" Then
                                    Dim Progess = New ProgressbarEventAgrs With {
                                                                 .PackerId = _Packer.DeviceId,
                                                                 .MaxValue = ParameterCount,
                                                                 .MinValue = 0,
                                                                 .ProgessValue = CurrentCount
                                                                    }
                                    RaiseEvent UpdateMinicProgressBar(Me, Progess)
                                End If
                            Next
                        End If
                        _Packer.IsParameterRequested = True
                        Await Task.Delay(100)
                    Catch ex As Exception
                        If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
                        _Packer.ErrorLog.Add($"StartCommunication Parameter - {_Packer.DeviceId}: {ex.Message}")
                        Console.WriteLine(ex.Message)
                    End Try
                End While
                CurrentCount = 0
                _Packer.IsParameterRequested = False
                _Packer.IsInitalParameterDownloaded = True
                IsApplicationLoaded(_Packer.DeviceId) = True

                For Each _Spout As SpoutController In _Packer.SpoutList
                    If _Packer.IsParameterRequested Then Exit For
                    If _Packer.CheckWeigher Then
                        GoTo ExitLoop
                    End If
                    For Each _SpoutQuery As String In _Spout.Query
                        If _Packer.CommunicationType = "Ethernet" Then
                            Await ComOutputResult(_Packer, _Spout, _SpoutQuery, TotalBytes, Timeout, ct)
                        Else
                            Await SerialOutputResult(_Packer, _Spout, _SpoutQuery, TotalBytes, Timeout, ct)
                        End If
                        Await Task.Delay(50)
                    Next
                Next


                'For getting Weight from Manual weighing Machine . Arvind Sir controller
                Dim DummyTextbox As TextBox = Nothing
                _Packer.IsParameterRequested = True
                Dim ManualWeightQuery As New List(Of String) From {
                                                                    $"AE51{vbCrLf}"
                                                                    }
                Dim TmpSpout As New SpoutController With {
            .PackerId = _Packer.DeviceId}
GetMoreWeight:
                Dim Inbuf1 = Await ComOutputResult(_Packer, TmpSpout, $"AE51{vbCrLf}", 20, 100, ct)
                If Inbuf1 <> "" And Inbuf1.Length > 6 Then
                    Dim StartIndex As Integer = InStr(Inbuf1, "AE51")
                    Dim WeightAvailable As String = Mid(Inbuf1, StartIndex + 4, 1)
                    Dim MoreWeightAvailable As String = Mid(Inbuf1, StartIndex + 5, 1)
                    Dim Message As String
                    If WeightAvailable = 1 Then
                        Dim SpoutId As String = Mid(Inbuf1, 8, 2)
                        Dim Weight As String = Mid(Inbuf1, 12, 12)
                        Message = $"Packer No {_Packer.DeviceId} SpoutId {SpoutId} and weight {Weight}"
                        'Console.WriteLine($"Packer No {_Packer.DeviceId} SpoutId {SpoutId} and weight {Weight}")
                        Dim Ack = Await ComOutputResult(_Packer, TmpSpout, $"AE51OK{vbCrLf}", 20, Timeout, ct)
                        RaiseEvent ManualWeightReceived(Me, New ManualWeightEventAgrs With {
                                                                                             .SpoutId = SpoutId,
                                                                                             .PackerId = _Packer.DeviceId,
                                                                                             .DateTime = Now(),
                                                                                             .Weight = Weight
                                                                                             })
                    Else
                        Message = $"Packer No {_Packer.DeviceId}  No Weight Avaiable"
                        'Console.WriteLine($"Packer No {_Packer.DeviceId}  No Weight Avaiable")
                    End If

                    If MoreWeightAvailable = 1 Then
                        GoTo GetMoreWeight
                    End If
                End If
ExitLoop:
            End While
        Catch ex As SocketException
            If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"StartCommunication SocketException - {_Packer.DeviceId}: {ex.Message}")
            Console.WriteLine(ex.Message)
        Catch ex As IOException
            If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"StartCommunication IOException - {_Packer.DeviceId}: {ex.Message}")
            Console.WriteLine(ex.Message)
        Catch ex As TaskCanceledException
            If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"StartCommunication TaskCanceledException - {_Packer.DeviceId}: {ex.Message}")
            Console.WriteLine(ex.Message)
        Catch ex As Exception
            If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"StartCommunication Exception - {_Packer.DeviceId}: {ex.Message}")
            Console.WriteLine(ex.Message)
        End Try
        Return result
    End Function
    Public Async Function SerialOutputResult(_Packer As Packer, _Spout As SpoutController, OpStr As String, TotalBytes As Integer, Timeout As Integer, ct As CancellationToken, Optional QueryType As String = "Read") As Task(Of String)
        Dim responseData As New List(Of Byte)
        Dim isCurrentlyConnected As Boolean
        Dim buffer(1024) As Byte
        Dim EndByte As Byte
        Dim PKRE As Integer
        Dim ResponseOkFlag As Boolean
        PKRE = CInt(_Spout.PackerId)

        Dim NewUpdate As New ToolStripEventArgs With {
                .PackerId = _Packer.DeviceId,
                .MessageType = "Query",
                .Message = OpStr.Replace(vbCr, "")
            }
        RaiseEvent ToolStripUpdate(Me, NewUpdate)
        If InStr(OpStr, "QT") > 0 Then
            EndByte = 13
        Else
            EndByte = 10
        End If
        If isClosing Then
            Return ""
        End If


        Try
                If _Packer.serial Is Nothing OrElse Not _Packer.serial.IsOpen Then
                    _Packer.serial = New SerialPort(_Packer.PortName, _Packer.BaudRate, Parity.None, 8, StopBits.One)
                    _Packer.serial.ReadTimeout = 2000
                    _Packer.serial.WriteTimeout = Timeout
                    _Packer.serial.Open()
                    isCurrentlyConnected = True
                Else
                    isCurrentlyConnected = True
                End If
            Catch ex As OperationCanceledException
                HandlePackerDisconnectionStatus(_Spout)
                If _Packer.ErrorLog.Count > 50 Then _Packer.ErrorLog.Clear()
                _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
            Catch ex As TimeoutException ' Catch the custom timeout exception
                HandlePackerDisconnectionStatus(_Spout)
                If _Packer.ErrorLog.Count > 50 Then _Packer.ErrorLog.Clear()
                _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
            Catch ex As SocketException
                HandlePackerDisconnectionStatus(_Spout)
                If _Packer.ErrorLog.Count > 50 Then _Packer.ErrorLog.Clear()
                _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
            Catch ex As Exception
                HandlePackerDisconnectionStatus(_Spout)
                If _Packer.ErrorLog.Count > 50 Then _Packer.ErrorLog.Clear()
                _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
            End Try

            Try
                If isCurrentlyConnected Then
RetrySpout:

                    _Packer.serial.DiscardInBuffer()
                    _Packer.serial.DiscardOutBuffer()

                    Dim outData As Byte() = Encoding.ASCII.GetBytes(OpStr)
                    Await _Packer.serial.BaseStream.WriteAsync(outData, 0, outData.Length, ct)
                    ' Console.WriteLine("send: " & OpStr.Replace(vbCr, ""))
                    Await Task.Delay(100)
                    Dim sw As Stopwatch = Stopwatch.StartNew()
                    Dim bytesReadCurrentSession As Integer = 0
                    Dim receivedStringBuilder As New StringBuilder()
                    Dim startTime As Date = Date.Now

                    If QueryType = "Write" Then
                        Return ""
                    End If

                    If Not _Spout.ControllerModel = "F701S" Then
                        If InStr(OpStr, "W02") > 0 Then
                            Return ""
                        End If
                    End If
                    Do
                        If _Packer.serial.BytesToRead > 0 Then
                            Dim readBuffer(0) As Byte
                            Dim bytesRead = _Packer.serial.ReadByte()
                            If bytesRead > 0 Then
                                Dim currentByte = readBuffer(0)
                                receivedStringBuilder.Append(ChrW(bytesRead))
                                bytesReadCurrentSession += 1
                                If currentByte = EndByte Then
                                    ResponseOkFlag = True
                                    Exit Do
                                End If
                            End If
                        Else Await Task.Delay(10, ct)
                        End If
                        If (Date.Now - startTime).TotalMilliseconds > Timeout Then Exit Do
                        If bytesReadCurrentSession >= buffer.Length Then Exit Do
                    Loop While True
                    ResponseFlg = False
                    Dim ClInstr As String = receivedStringBuilder.ToString().Trim().Replace(vbCr, "")
                    '   Console.WriteLine($"Receiving message:  {ClInstr.Replace(vbCr, "")} TimeStamp - {Date.Now().ToString("HH:mm:ss")}")
                    Dim NewResponse As New ToolStripEventArgs With {
                        .PackerId = _Packer.DeviceId,
                        .MessageType = "Response",
                        .Message = ClInstr.Replace(vbCr, "")
                    }
                    RaiseEvent ToolStripUpdate(Me, NewResponse)
                    '  If ResponseOkFlag = False Then ClInstr = ""
                    If InStr(OpStr, "AE") > 0 Then
                        Return ClInstr
                    End If
                    If ClInstr = "" Or ClInstr.Length < 3 Then

                        _Spout.NoReplyCount += 1
                        If _Spout.NoReplyCount > 3 Then
                            _Spout.NoReplyCount = 0
                            _Spout.IsConnected = False
                            Return False
                        End If
                        Await Task.Delay(500)
                        RaiseEvent DeviceDataReceived(Me, _Spout)
                        GoTo RetrySpout
                    End If
                    _Spout.IsConnected = True
                    '   Console.WriteLine($"Receiving message:{ ClInstr} TimeStamp - {Date.Now().ToString("HH:mm:ss")}")
                    If _Packer.IsParameterRequested Then
                        Return ClInstr
                    Else
                        If InStr(ClInstr, "QT") > 0 Then
                            Dim CleanedData As String
                            Dim Length = InStr(ClInstr, vbCr)
                            If Length > 0 Then
                                CleanedData = ClInstr.Substring(0, Length - 1).Replace("QT", "")
                            Else
                                CleanedData = ClInstr.Replace("QT", "")
                            End If
                            Dim SpoutId As Integer = CInt(Mid(CleanedData, 1, 2))
                            Dim NoOfBags As Integer = CInt(Mid(CleanedData, 3, 1))
                            Dim weightString As String = Mid(CleanedData, 4)
                            Dim StartPos As Integer = 0
                            For Z = 1 To NoOfBags
                                Dim BagCode = CInt(Mid(weightString, 1 + StartPos, 1))
                                Dim Bag As String = Mid(weightString, 2 + StartPos, 6)
                                StartPos += 7
                                _Spout.BagCount += 1
                                _Packer.BagCount += 1
                                _Spout.LastDischargedBagWeight = Bag
                                _Spout.CurrentCode = BagCode
                                _Spout.datetime = Date.Now()
                                _Spout.NewDataFlag = True
                                '      Console.WriteLine($"Receiving message: {Bag} TimeStamp - {Date.Now().ToString("HH:mm:ss")}")
                                _Spout.TotalPackerBagCount = _Packer.BagCount
                                Dim measurement As New Measurement(Date.Now(), Bag)
                                _Spout.Last20Bags.Enqueue(measurement)
                                _Spout.LastDischargedBagWeight = Bag
                                _Spout.datetime = Date.Now().ToString("HH:mm:ss")
                                _Spout.LastDIschargedBagDateTime = Date.Now()
                                _Spout.IsConnected = True
                                'CheckForAutoCorrection(_Packer, _Spout, Bag)
                                RaiseEvent DeviceDataReceived(Me, _Spout)
                            Next
                        Else

                            Dim Rjmatch As Match = Regex.Match(ClInstr, "(\d+\.\d+)")
                            Dim match = Regex.Match(ClInstr, "RL(\d+)\+(\d{2,3}\.\d{2})\+(\d+)\+(\d+)\+(\d+)")
                            Dim collectedVal As Double
                            If match.Success Then
                                ResponseFlg = True
                                Dim DecimalPoint As New QueryInfo
                                _Spout.CurrentCode = CInt(match.Groups(1).Value)

                                _Spout.FinalWeight = match.Groups(3).Value
                                DecimalPoint = _UniPulse.Final(_Spout.ControllerModel, _Spout.SpoutId)
                                _Spout.FinalWeight = _ProcessResult.FormatParameter(_Spout.FinalWeight, DecimalPoint.DecimalFormat)

                                _Spout.Over = match.Groups(4).Value
                                DecimalPoint = _UniPulse.Over(_Spout.ControllerModel, _Spout.SpoutId)
                                _Spout.Over = _ProcessResult.FormatParameter(_Spout.Over, DecimalPoint.DecimalFormat)

                                _Spout.Under = If(match.Groups(5).Value.Length >= 3, match.Groups(5).Value.Substring(0, 3), match.Groups(3).Value)
                                DecimalPoint = _UniPulse.Under(_Spout.ControllerModel, _Spout.SpoutId)
                                _Spout.Under = _ProcessResult.FormatParameter(_Spout.Under, DecimalPoint.DecimalFormat)

                                If Double.TryParse(match.Groups(2).Value, collectedVal) AndAlso collectedVal > 0 Then
                                    _Spout.BagCount += 1
                                    _Packer.BagCount += 1
                                    _Spout.LastDischargedBagWeight = collectedVal.ToString("000.00")
                                    _Spout.NewDataFlag = True
                                    Dim formatdt As New Formatdatetime(ClInstr, _Spout)
                                End If
                                _Spout.TotalPackerBagCount = _Packer.BagCount
                                Dim measurement As New Measurement(Date.Now(), collectedVal)
                                _Spout.Last20Bags.Enqueue(measurement)
                                _Spout.LastDischargedBagWeight = collectedVal.ToString("000.00")
                                _Spout.LastDIschargedBagDateTime = Date.Now()
                                _Spout.IsConnected = True
                                'CheckForAutoCorrection(_Packer, _Spout, collectedVal)
                                RaiseEvent DeviceDataReceived(Me, _Spout)
                            ElseIf Rjmatch.Success Then
                                If Double.TryParse(Rjmatch.Value, collectedVal) AndAlso collectedVal > 0 Then
                                    _Spout.BagCount += 1
                                    _Packer.BagCount += 1
                                    _Spout.LastDischargedBagWeight = collectedVal.ToString("000.00")
                                    _Spout.datetime = Now.ToString("HH:mm:ss")
                                    _Spout.NewDataFlag = True
                                End If
                                _Spout.TotalPackerBagCount = _Packer.BagCount
                                Dim measurement As New Measurement(Date.Now(), collectedVal)
                                _Spout.Last20Bags.Enqueue(measurement)
                                _Spout.LastDischargedBagWeight = collectedVal.ToString("000.00")
                                _Spout.LastDIschargedBagDateTime = Date.Now()
                                _Spout.IsConnected = True
                                'CheckForAutoCorrection(_Packer, _Spout, collectedVal)
                                RaiseEvent DeviceDataReceived(Me, _Spout)
                            End If

                        End If
                        If Not ResponseFlg Then
                            RaiseEvent DeviceDataReceived(Me, _Spout)
                        End If
                    End If
                    Return True
                End If
            Catch ex As SocketException
                HandlePackerDisconnectionStatus(_Spout)
                If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
                _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
            Catch ex As IOException
                HandlePackerDisconnectionStatus(_Spout)
                If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
                _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
            Catch ex As Exception
                If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
                _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
            Finally
            End Try

        Return False
    End Function
    Public Async Function ComOutputResult(_Packer As Packer, _Spout As SpoutController, OpStr As String, TotalBytes As Integer, Timeout As Integer, ct As CancellationToken, Optional QueryType As String = "Read") As Task(Of String)
        Dim responseData As New List(Of Byte)
        Dim isCurrentlyConnected As Boolean
        Dim buffer(1023) As Byte
        Dim EndByte As Byte
        Dim PKRE As Integer
        Dim ResponseOkFlag As Boolean
        PKRE = CInt(_Spout.PackerId)



        Dim NewUpdate As New ToolStripEventArgs With {
                .PackerId = _Packer.DeviceId,
                .MessageType = "Query",
                .Message = OpStr.Replace(vbCr, "")
            }
        RaiseEvent ToolStripUpdate(Me, NewUpdate)
        If InStr(OpStr, "QT") > 0 Then
            EndByte = 13
        Else
            EndByte = 10
        End If
        If isClosing Then
            Return ""
        End If
        Try
            If _Packer.Client Is Nothing OrElse Not _Packer.Client.Connected Then
                _Packer.Client = New TcpClient()
                Dim connectTask As Task = _Packer.Client.ConnectAsync(_Packer.IPAddress, _Packer.Port)
                Dim timeoutTask As Task = Task.Delay(TimeSpan.FromMilliseconds(2000), ct)
                Dim completedTask As Task = Await Task.WhenAny(connectTask, timeoutTask)
                If completedTask Is timeoutTask Then
                    Throw New TimeoutException($"Connection to {_Packer.IPAddress} timed out after {200} seconds.")
                End If
                If connectTask.IsFaulted Then
                    isCurrentlyConnected = False
                    Throw connectTask.Exception.InnerException
                End If
                isCurrentlyConnected = True
            Else
                isCurrentlyConnected = True
            End If
        Catch ex As OperationCanceledException
            HandlePackerDisconnectionStatus(_Spout)
            If _Packer.ErrorLog.Count > 50 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
        Catch ex As TimeoutException ' Catch the custom timeout exception
            HandlePackerDisconnectionStatus(_Spout)
            If _Packer.ErrorLog.Count > 50 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
        Catch ex As SocketException
            HandlePackerDisconnectionStatus(_Spout)
            If _Packer.ErrorLog.Count > 50 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
        Catch ex As Exception
            HandlePackerDisconnectionStatus(_Spout)
            If _Packer.ErrorLog.Count > 50 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
        End Try

        Try
            If isCurrentlyConnected Then
RetrySpout:
                If _Packer.Client Is Nothing Then Exit Function
                Dim stream As NetworkStream = _Packer.Client.GetStream()
                If stream.DataAvailable Then
                    Dim GarageBuffer(1023) As Byte
                    Dim GaragebytesRead = Await stream.ReadAsync(GarageBuffer, 0, GarageBuffer.Length, ct)
                    GarageBuffer = Nothing
                End If
                Dim outData As Byte() = Encoding.ASCII.GetBytes(OpStr)
                Await stream.WriteAsync(outData, 0, outData.Length, ct)
                Console.WriteLine($"Sending message:  {OpStr} TimeStamp - {Date.Now().ToString("HH:mm:ss")}")

                Await Task.Delay(100)
                Dim sw As Stopwatch = Stopwatch.StartNew()
                Dim bytesReadCurrentSession As Integer = 0
                Dim receivedStringBuilder As New StringBuilder()
                Dim startTime As Date = Date.Now

                If QueryType = "Write" Then
                    Return ""
                End If
                If Not _Spout.ControllerModel = "F701S" Then
                    If InStr(OpStr, "W02") > 0 Then
                        Return ""
                    End If
                End If

                Do
                        If stream.DataAvailable Then
                            Dim readBuffer(0) As Byte
                            Dim bytesRead = Await stream.ReadAsync(readBuffer, 0, 1, ct)
                            If bytesRead > 0 Then
                                Dim currentByte = readBuffer(0)
                                receivedStringBuilder.Append(Encoding.UTF8.GetString(readBuffer))
                                bytesReadCurrentSession += 1
                                If currentByte = EndByte Then
                                    ResponseOkFlag = True
                                    Exit Do
                                End If
                            End If
                        Else
                            Await Task.Delay(10, ct)
                        End If

                        If (Date.Now - startTime).TotalMilliseconds > Timeout Then Exit Do
                        If bytesReadCurrentSession >= buffer.Length Then Exit Do
                    Loop While True


                    Dim ClInstr As String = receivedStringBuilder.ToString().Trim().Replace(vbCr, "")
                ' Console.WriteLine($"Receiving message:  {ClInstr.Replace(vbCr, "")} TimeStamp - {Date.Now().ToString("HH:mm:ss")}")
                Dim NewResponse As New ToolStripEventArgs With {
                        .PackerId = _Packer.DeviceId,
                        .MessageType = "Response",
                        .Message = ClInstr.Replace(vbCr, "")
                    }

                    RaiseEvent ToolStripUpdate(Me, NewResponse)
                    If ResponseOkFlag = False Then ClInstr = ""
                    If InStr(OpStr, "AE") > 0 Then
                        Return ClInstr
                    End If
                    If ClInstr = "" Or ClInstr.Length < 3 Then
                        If _Spout.IsConnected = True Then
                            _Spout.NoReplyCount += 1
                            If _Spout.NoReplyCount > 3 Then
                                _Spout.NoReplyCount = 0
                                _Spout.IsConnected = False
                                _Spout.LastDischargedBagWeight = ""
                                RaiseEvent DeviceDataReceived(Me, _Spout)
                                Return False
                            End If
                            Await Task.Delay(1500)
                            GoTo RetrySpout
                        End If
                    End If
                    _Spout.IsConnected = True
                    '  Console.WriteLine($"Receiving message:  { ClInstr} TimeStamp - {Date.Now().ToString("HH:mm:ss")}")
                    If _Packer.IsParameterRequested Then
                        Return ClInstr
                    Else
                        Dim timeDifference As TimeSpan = Date.Now().Subtract(_Spout.LastDIschargedBagDateTime)
                        _Spout.EmptyRound = timeDifference.Seconds / 12
                        If _Spout.EmptyRound > 0 Then _Spout.EmptyRound -= 1
                        If InStr(ClInstr, "QT") > 0 Then
                            Dim CleanedData As String
                            Dim Length = InStr(ClInstr, vbCr)
                            If Length > 0 Then
                                CleanedData = ClInstr.Substring(0, Length - 1).Replace("QT", "")
                            Else
                                CleanedData = ClInstr.Replace("QT", "")
                            End If
                            Dim SpoutId As Integer = CInt(Mid(CleanedData, 1, 2))
                            Dim NoOfBags As Integer = CInt(Mid(CleanedData, 3, 1))
                            Dim weightString As String = Mid(CleanedData, 4)
                            Dim StartPos As Integer = 0
                            For Z = 1 To NoOfBags
                                Dim BagCode = CInt(Mid(weightString, 1 + StartPos, 1))
                                Dim Bag As String = Mid(weightString, 2 + StartPos, 6)
                                StartPos += 7
                                _Spout.BagCount += 1
                                _Packer.BagCount += 1
                                _Spout.LastDischargedBagWeight = Bag
                                _Spout.CurrentCode = BagCode
                                _Spout.datetime = Date.Now()
                                _Spout.NewDataFlag = True
                                ' Console.WriteLine($"Receiving message: {Bag} TimeStamp - {Date.Now().ToString("HH:mm:ss")}")
                                _Spout.TotalPackerBagCount = _Packer.BagCount
                                Dim measurement As New Measurement(Date.Now(), Bag)
                                _Spout.Last20Bags.Enqueue(measurement)
                                _Spout.LastDischargedBagWeight = Bag
                                _Spout.datetime = Date.Now().ToString("HH:mm:ss")

                                _Spout.LastDIschargedBagDateTime = Date.Now()
                                _Spout.IsConnected = True

                                If _Spout.Under = 0 Then
                                    _Spout.Under = 0.4
                                    Dim ReadParameterQuery As QueryInfo = _UniPulse.Under(_Spout.ControllerModel, _Spout.SpoutId, "")
                                    EnqueueDeviceCommand(_Packer.DeviceId, _Spout.SpoutId, ReadParameterQuery.ResponseLength, "Under", ReadParameterQuery.Query, , ReadParameterQuery.DecimalFormat)
                                End If
                                If _Spout.Over = 0 Then
                                    _Spout.Over = 0.7
                                    Dim ReadParameterQuery As QueryInfo = _UniPulse.Over(_Spout.ControllerModel, _Spout.SpoutId, "")
                                    EnqueueDeviceCommand(_Packer.DeviceId, _Spout.SpoutId, ReadParameterQuery.ResponseLength, "Over", ReadParameterQuery.Query, , ReadParameterQuery.DecimalFormat)
                                End If

                                If _Spout.FinalWeight = 0 Then
                                    _Spout.FinalWeight = 50.1
                                    Dim ReadParameterQuery As QueryInfo = _UniPulse.Final(_Spout.ControllerModel, _Spout.SpoutId, "")
                                    EnqueueDeviceCommand(_Packer.DeviceId, _Spout.SpoutId, ReadParameterQuery.ResponseLength, "Final", ReadParameterQuery.Query, , ReadParameterQuery.DecimalFormat)
                                End If

                                If IsAutoCorrectionEnabled(_Packer.DeviceId - 1) Then
                                    CheckForAutoCorrection(_Packer, _Spout, Bag)
                                End If
                                RaiseEvent DeviceDataReceived(Me, _Spout)
                            Next
                            'If NoOfBags = 0 Then
                            '    RaiseEvent DeviceDataReceived(Me, _Spout)
                            'End If
                        Else
                            Dim Rjmatch As Match = Regex.Match(ClInstr, "(\d+\.\d+)")
                            Dim match = Regex.Match(ClInstr, "RL(\d+)\+(\d{2,3}\.\d{2})\+(\d+)\+(\d+)\+(\d+)")
                            Dim collectedVal As Double
                            If match.Success Then
                                Dim DecimalPoint As New QueryInfo
                                _Spout.CurrentCode = CInt(match.Groups(1).Value)


                                _Spout.FinalWeight = match.Groups(3).Value
                                DecimalPoint = _UniPulse.Final(_Spout.ControllerModel, _Spout.SpoutId)
                                _Spout.FinalWeight = _ProcessResult.FormatParameter(_Spout.FinalWeight, DecimalPoint.DecimalFormat)


                                _Spout.Over = match.Groups(4).Value
                                DecimalPoint = _UniPulse.Over(_Spout.ControllerModel, _Spout.SpoutId)
                                _Spout.Over = _ProcessResult.FormatParameter(_Spout.Over, DecimalPoint.DecimalFormat)



                                _Spout.Under = If(match.Groups(5).Value.Length >= 3, match.Groups(5).Value.Substring(0, 3), match.Groups(5).Value)
                                DecimalPoint = _UniPulse.Under(_Spout.ControllerModel, _Spout.SpoutId)
                                _Spout.Under = _ProcessResult.FormatParameter(_Spout.Under, DecimalPoint.DecimalFormat)

                                If Double.TryParse(match.Groups(2).Value, collectedVal) AndAlso collectedVal > 0 Then
                                    _Spout.BagCount += 1
                                    _Packer.BagCount += 1
                                    _Spout.LastDischargedBagWeight = collectedVal.ToString("000.00")
                                    _Spout.NewDataFlag = True
                                    Dim formatdt As New Formatdatetime(ClInstr, _Spout)
                                End If
                                _Spout.TotalPackerBagCount = _Packer.BagCount
                                Dim measurement As New Measurement(Date.Now(), collectedVal)
                                _Spout.Last20Bags.Enqueue(measurement)
                                _Spout.LastDischargedBagWeight = collectedVal.ToString("000.00")

                                _Spout.LastDIschargedBagDateTime = Date.Now()
                                _Spout.IsConnected = True
                                If IsAutoCorrectionEnabled(_Packer.DeviceId - 1) Then
                                    CheckForAutoCorrection(_Packer, _Spout, collectedVal)
                                End If

                                RaiseEvent DeviceDataReceived(Me, _Spout)
                            ElseIf Rjmatch.Success Then
                                If Double.TryParse(Rjmatch.Value, collectedVal) AndAlso collectedVal > 0 Then
                                    _Spout.BagCount += 1
                                    _Packer.BagCount += 1
                                    _Spout.LastDischargedBagWeight = collectedVal.ToString("000.00")
                                    _Spout.datetime = Now.ToString("HH:mm:ss")
                                    _Spout.NewDataFlag = True
                                End If
                                _Spout.TotalPackerBagCount = _Packer.BagCount
                                Dim measurement As New Measurement(Date.Now(), collectedVal)
                                _Spout.Last20Bags.Enqueue(measurement)
                                _Spout.LastDischargedBagWeight = collectedVal.ToString("000.00")
                                _Spout.LastDIschargedBagDateTime = Date.Now()
                                _Spout.IsConnected = True
                                'CheckForAutoCorrection(_Packer, _Spout, collectedVal)
                                RaiseEvent DeviceDataReceived(Me, _Spout)

                            End If
                        End If

                    End If
                    Return True
                End If
        Catch ex As SocketException
            HandlePackerDisconnectionStatus(_Spout)
            If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
        Catch ex As IOException
            HandlePackerDisconnectionStatus(_Spout)
            If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
        Catch ex As Exception
            If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"Connection to {_Packer.IPAddress} cancelled: {ex.Message}")
        Finally
            'COMBUSYFL(PKRE) = False
        End Try
        Return False
    End Function
    Private Sub CheckForAutoCorrection(_Packer As Packer, _Spout As SpoutController, CurrentWeight As Double)
        Try
            If _Spout.SampleWeights.Count >= 1 Then
                If CurrentWeight >= CorrectionRangeFrom(_Packer.DeviceId - 1) And CurrentWeight <= CorrectionRangeTo(_Packer.DeviceId - 1) Then
                    Dim LastBagIndex = _Spout.SampleWeights.Count - 1
                    Dim LastBag = _Spout.SampleWeights(LastBagIndex)
                    If (LastBag >= ValidWeightFrom(_Packer.DeviceId - 1) And LastBag <= ValidWeightTo(_Packer.DeviceId - 1)) And (CurrentWeight >= ValidWeightFrom(_Packer.DeviceId - 1) And CurrentWeight <= ValidWeightTo(_Packer.DeviceId)) Then
                        _Spout.SampleWeights.Clear()
                    Else
                        _Spout.SampleWeights.Add(CurrentWeight)
                        If _Spout.SampleWeights.Count >= sample(_Packer.DeviceId - 1) Then
                            Dim CurrentFinalWeight = _Spout.FinalWeight '+ _Spout.FinalAdjustmentWeight
                            If CurrentFinalWeight > 49 Then
                                'Dim Deviation As List(Of Double) = _Spout.SampleWeights.
                                'Where(Function(weight) weight < ValidWeightFrom(_Packer.DeviceId - 1) Or weight > ValidWeightTo(_Packer.DeviceId - 1)).
                                'Select(Function(weight) CurrentFinalWeight - weight).
                                'ToList()




                                Dim UnderWeightBagsInOverWeightSamples As List(Of Double) = _Spout.SampleWeights.
                                Where(Function(weight) weight < ValidWeightFrom(_Packer.DeviceId - 1)).ToList()

                                Dim OverWeightBagsInUnderWeightSamples As List(Of Double) = _Spout.SampleWeights.
                                Where(Function(weight) weight > ValidWeightTo(_Packer.DeviceId - 1)).ToList()

                                If UnderWeightBagsInOverWeightSamples.Count > 0 And OverWeightBagsInUnderWeightSamples.Count > 0 Then
                                    _Spout.AutoCorrectionRemarks = "Correction Not done, Due to weight fluctuation"
                                    _Spout.RealCorrection = 0
                                    _Spout.SampleWeightStr = String.Join(",", _Spout.SampleWeights)
                                    RaiseEvent AutoCorrectDataReceived(Me, _Spout)
                                    _Spout.SampleWeights.Clear()
                                    Exit Sub
                                End If

                                Dim Deviation As List(Of Double) = _Spout.SampleWeights.Select(Function(weight) CurrentFinalWeight - weight).ToList()


                                Dim CorrectionValue As Double = Deviation.Average() * coefficient(_Packer.DeviceId - 1)
                                Dim CalculateFinalAdjustmentWeight = Math.Round(_Spout.FinalAdjustmentWeight + CorrectionValue, 2)
                                '_Spout.AutoCorrectionRemarks = $"Correcntion Value {CorrectionValue} - {CalculateFinalAdjustmentWeight} Range {CorrectionLimitFrom(_Packer.DeviceId - 1)}-{CorrectionLimitTo(_Packer.DeviceId - 1)}"
                                'RaiseEvent AutoCorrectDataReceived(Me, _Spout)
                                If CorrectionValue > CorrectionLimitFrom(_Packer.DeviceId - 1) And CorrectionValue < CorrectionLimitTo(_Packer.DeviceId - 1) Then
                                    If CalculateFinalAdjustmentWeight < 0 Then
                                        CalculateFinalAdjustmentWeight = 0
                                    End If
                                    If CalculateFinalAdjustmentWeight >= 0 And CalculateFinalAdjustmentWeight <> _Spout.FinalAdjustmentWeight Then
                                        _Spout.RealCorrection = CalculateFinalAdjustmentWeight
                                        Dim Query As QueryInfo = _UniPulse.ChangeCode(_Spout.ControllerModel, _Spout.SpoutId, _Spout.CurrentCode.ToString())
                                        EnqueueDeviceCommand(_Packer.DeviceId, _Spout.SpoutId, 16, "Write Code", Query.Query,,, "Write")
                                        Dim FunctionsToCall() As String = {"FinalAdjustWeight"}
                                        Dim allMethods = GetType(Unipulse).GetMethods()
                                        Dim validMethods = allMethods.Where(Function(m) m.Name = FunctionsToCall(0)).ToList()

                                        For Each m In validMethods
                                            Dim ReadParameterQuery As QueryInfo = m.Invoke(_UniPulse, New Object() {_Spout.ControllerModel, _Spout.SpoutId, ""})
                                            Dim value = _UniPulse.FormatValue(CalculateFinalAdjustmentWeight, 5, ReadParameterQuery.DecimalFormat)
                                            Dim WriteParameterQuery As QueryInfo = m.Invoke(_UniPulse, New Object() {_Spout.ControllerModel, _Spout.SpoutId, value})
                                            EnqueueDeviceCommand(_Packer.DeviceId, _Spout.SpoutId, WriteParameterQuery.ResponseLength, m.Name, WriteParameterQuery.Query, , WriteParameterQuery.DecimalFormat, "Write")
                                            EnqueueDeviceCommand(_Packer.DeviceId, _Spout.SpoutId, ReadParameterQuery.ResponseLength, m.Name, ReadParameterQuery.Query, , ReadParameterQuery.DecimalFormat, "Read")
                                        Next
                                        CurrentFormOpened = ""
                                        _Spout.SampleWeightStr = String.Join(",", _Spout.SampleWeights)
                                        _Spout.AutoCorrectionRemarks = "Correction done"
                                        RaiseEvent AutoCorrectDataReceived(Me, _Spout)
                                    End If

                                End If
                                _Spout.SampleWeights.Clear()
                            End If
                        End If
                    End If
                End If
            Else
                _Spout.SampleWeights.Add(CurrentWeight)
            End If
        Catch ex As Exception
            If _Packer.ErrorLog.Count > 100 Then _Packer.ErrorLog.Clear()
            _Packer.ErrorLog.Add($"CheckForAutoCorrection - {_Packer.DeviceId}: {ex.Message}")
            Console.WriteLine(ex.Message)
        End Try
    End Sub
    Private Sub HandlePackerDisconnectionStatus(_Spout As SpoutController)
        Dim _Packer = Packers.FirstOrDefault(Function(d) d.DeviceId = _Spout.PackerId)
        If _Packer.Client IsNot Nothing Then
            If _Packer.IsConnected Then _Packer.Client.Close()
            _Packer.Client = Nothing
        End If
        _Spout.IsConnected = False
        _Spout.LastDischargedBagWeight = ""
        _Spout.CurrentCode = 0
        RaiseEvent DeviceDataReceived(Me, _Spout)
    End Sub
    Public Sub StopCommunication()
        If Not cancellationTokenSource.IsCancellationRequested Then
            cancellationTokenSource.Cancel()
            For Each _Packer As Packer In Packers
                If _Packer.Client IsNot Nothing Then
                    If _Packer.IsConnected Then _Packer.Client.Close()
                    _Packer.Client = Nothing
                End If
            Next
        End If
    End Sub
    Public Sub EnqueueDeviceCommand(PackerId As String, SpoutId As Integer, TotalBytes As Integer, commandName As String, Query As List(Of String), Optional TextBoxReference As TextBox = Nothing, Optional DecimalPoint As Integer = 0, Optional QueryType As String = "Read")
        Dim Packer = Packers.FirstOrDefault(Function(d) d.DeviceId = PackerId)
        Try
            Dim cmd As New QueuedCommand(commandName, SpoutId, TotalBytes, Query, TextBoxReference, DecimalPoint, QueryType)
            If Packer IsNot Nothing Then
                Packer.CommandQueue.Enqueue(cmd)
                Packer.IsParameterRequested = True
            Else
                Console.WriteLine($"Device with IP {PackerId} not found.")
            End If
        Catch ex As Exception
            If Packer IsNot Nothing Then
                If Packer.ErrorLog.Count > 100 Then Packer.ErrorLog.Clear()
                Packer.ErrorLog.Add($"EnqueueDeviceCommand {Packer.IPAddress} cancelled: {ex.Message}")
            End If
        End Try
    End Sub
End Class
