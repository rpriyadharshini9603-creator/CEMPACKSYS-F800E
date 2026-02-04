Imports System.IO.Ports
Imports System.Linq.Expressions
Imports System.Text
Imports System.Threading
Imports Org.BouncyCastle.Bcpg

Public Class CheckWeigher

    Private ReadOnly serialLock As New Object()
    Dim bytesReadCurrentSession As Integer = 0
    Dim receivedStringBuilder As New StringBuilder()
    Dim EndByte As Byte
    Public Event DeviceDataReceived As EventHandler(Of SpoutController)
    Private ReadOnly _cancellationTokenSource As CancellationTokenSource
    Public _queryLoopTask As Task
    Public Event ToolStripUpdate As EventHandler(Of ToolStripEventArgs)

    Public Sub New(availablePacker As List(Of Packer))
        If availablePacker IsNot Nothing Then
            If availablePacker.Count > 0 Then
                For Each _Packer As Packer In availablePacker
                    _cancellationTokenSource = New CancellationTokenSource()
                    If _Packer.CheckWeigher Then
                        _queryLoopTask = Task.Run(Async Function()
                                                      Await Startcommunicating(_Packer, _cancellationTokenSource.Token)
                                                  End Function, _cancellationTokenSource.Token)
                    Else

                    End If

                Next
            End If
        End If
    End Sub
    Public Async Function Startcommunicating(Packer As Packer, ct As CancellationToken) As Task
        If Packer.serial Is Nothing OrElse Not Packer.serial.IsOpen Then
            Packer.serial = New SerialPort(Packer.CWPortName, Packer.CWBaudRate, Parity.None, 8, StopBits.One)
            Packer.serial.ReadTimeout = 500
            Packer.serial.WriteTimeout = 200
            Packer.serial.Open()
            Packer.IsConnected = True
        End If
        EndByte = 13
        Try
            While Not ct.IsCancellationRequested
                Try

                    Dim bytesRead As Integer = Packer.serial.ReadByte()
                    If bytesRead >= 0 Then
                        Dim currentChar As Char = ChrW(bytesRead)
                        receivedStringBuilder.Append(currentChar)

                        If bytesRead = EndByte Then
                            Dim ClInstr As String = receivedStringBuilder.ToString().Trim().Replace(vbCr, "")
                            receivedStringBuilder.Clear()

                            Dim parts() As String = ClInstr.Split("+"c)
                            If parts.Length >= 2 Then
                                Dim Spout = Packer.SpoutList.FirstOrDefault(Function(d) d.SpoutId = parts(0))
                                If Spout IsNot Nothing Then
                                    Spout.BagCount += 1
                                    Packer.BagCount += 1
                                    Spout.LastDischargedBagWeight = CDbl(parts(1)).ToString("000.00")
                                    Spout.datetime = Now.ToString("HH:mm:ss")
                                    Spout.NewDataFlag = True
                                    Spout.TotalPackerBagCount = Packer.BagCount

                                    Dim measurement As New Measurement(Date.Now(), parts(1))
                                    Spout.Last20Bags.Enqueue(measurement)
                                    Spout.LastDIschargedBagDateTime = Date.Now()
                                    Spout.IsConnected = True
                                    Dim NewUpdate As New ToolStripEventArgs With {
                                                .PackerId = Packer.DeviceId,
                                                .MessageType = "Query",
                                                .Message = ClInstr
                                            }
                                    RaiseEvent DeviceDataReceived(Me, Spout)
                                    RaiseEvent ToolStripUpdate(Me, NewUpdate)
                                End If
                            End If
                        End If
                    End If

                Catch ex As TimeoutException
                    Thread.Sleep(100)
                End Try
            End While
        Catch ex As Exception
            Console.WriteLine("Error reading serial data: " & ex.Message)
        End Try
    End Function

    Public Sub StopCommunication()
        If Not _cancellationTokenSource.IsCancellationRequested Then
            _cancellationTokenSource.Cancel()
            For Each _Packer As Packer In Packers
                If _Packer.IsConnected Then
                    _Packer.serial.Close()
                End If

            Next
        End If
    End Sub





End Class
