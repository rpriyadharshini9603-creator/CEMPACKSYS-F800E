Public Class Unipulse
    Public Function ReadWeight(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"RJ{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}RL{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}RJ{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function ReadCount(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"RH{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}RH{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function ReadWeightQT(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                   $"QT{SpoutNo.ToString().PadLeft(2, "0"c)}{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"QT{SpoutNo.ToString().PadLeft(2, "0"c)}{Value}{vbCr}"
                   }
                '_QueryInfo.Query = New List(Of String) From {
                '   $"QT{vbCr}"
                '   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function ReadCode(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W00{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W00{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function Clear(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"CJ{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}CJ{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function ChangeCode(Model As String, SpoutNo As Integer, Optional Value As String = "0") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{vbCr}", $"W02{SpoutNo.ToString().PadLeft(5, "0"c)}{vbCr}", $"E{Value.ToString().PadLeft(4, "0"c)}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = ""
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W02{Value.ToString().PadLeft(5, "0"c)}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = ""
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W000000{Value.ToString().PadLeft(5, "0"c)}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = ""
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function SP2AdjustmentWeight(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"

            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W10{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function SP2(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W11{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W11{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W05{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function Final(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W12{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W12{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W09{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function Over(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W13{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W13{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W07{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function Under(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W14{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W14{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W08{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function CPS(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W15{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "00.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W15{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "00.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
              $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W06{Value}{vbCr}"
              }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "00.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function AutoFreeFallCompensation(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W16{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W16{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W15{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function CompensationFeedingTime(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W17{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W17{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W14{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function Timer(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W20{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0-0.0"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W20{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0-0.0"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function Timer2(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W20{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0-0.0"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W20{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0-0.0"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function ComperisionInhibitTime(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W21{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W21{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W11{Value}{vbCr}"
                }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function

    Public Function UpperLimit(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W22{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W22{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W01{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function

    Public Function LowerLimit(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W23{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W23{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W02{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 3
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function

    Public Function NearZero(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W24{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W24{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W03{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function


    Public Function PresetTareWeight(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W25{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W25{Value}{vbCr}"
                }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function

    Public Function FinalAdjustWeight(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W25{Value}{vbCr}"
                }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"

            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function


    Public Function AutoZeroCount(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W26{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W26{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W44{Value}{vbCr}"
                }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function


    Public Function JudgingCount(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W27{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W27{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W12{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function

    Public Function DischargeingTime(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W28{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W28{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
              $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W42{Value}{vbCr}"
              }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function SP3TargetTime(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W29{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 0.0"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function SP3AllowableTime(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W29{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 0.0"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function SequenceMode(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W30{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00000"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W30{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00000"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function WeighingFunction1(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W31{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W31{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W16{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function WeighingFunction2(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W32{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00000"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W32{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00000"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W17{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function WeighingFunction3(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W33{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W33{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W31{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function FunctionKeyInhibited(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W34{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W34{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function Filter(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W35{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W35{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function MotionDetection(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W36{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W36{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W23{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function MotionDetection2(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W36{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W36{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W23{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function ZeroTracking(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W37{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W37{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W24{Value}{vbCr}"
                }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function ZeroTracking2(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W37{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W37{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W25{Value}{vbCr}"
                }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 1
                _QueryInfo.ResponseFormat = "0.0 0 00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function BalanceWeightValue(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W40{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W40{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W61{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function Capacity(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W41{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W41{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W62{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function MinimumScaleDivision(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W42{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W42{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W63{Value}{vbCr}"
                }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "0.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function NetOver(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W43{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W43{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W67{Value}{vbCr}"
                }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function GrossOver(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W44{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W44{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W68{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function FunctionSelection(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W45{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W45{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function GravitationalAccelerationCompensation(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W46{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W46{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                  $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W66{Value}{vbCr}"
                  }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function MaximumWeight(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W50{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W50{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function MinimumWeight(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W51{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W51{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function Max_Mini_Diff(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W52{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W52{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function AverageWeight(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W53{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W53{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function GeneralStandardDeviation(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W54{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W54{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function SampleStandardDeviation(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W55{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W55{Value}{vbCr}"
                   }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701S"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    '====================================================================================================
    Public Function FineFeedDuration(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W14{Value}{vbCr}"
                }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function SP1(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
                _QueryInfo.Query = New List(Of String) From {
                    $"S{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}", $"W10{Value}{vbCr}", $"E{SpoutNo.ToString().PadLeft(4, "0"c)}{Value}{vbCr}"
                    }
                _QueryInfo.ResponseLength = 13
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W04{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function ExtendedFunctionSelection1(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W51{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function ExtendedFunctionSelection2(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W52{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function JudgingTime(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W45{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function CompleteOutputTime(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W13{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function DigitalLowPassFilter(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W21{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function MovingAvgFilter(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W22{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function KeyInvalidLock(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W32{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function BagClampOutputTime(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W41{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function WeighingStartTime(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W43{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function Sequencemode1(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W46{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function Sequencemode2(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W47{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function FillingPromotionWeight(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W48{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function DzRegulationVal(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                 $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W64{Value}{vbCr}"
                 }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 2
                _QueryInfo.ResponseFormat = "000.00"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function FunctionSelection3(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W31{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function TotalComparisonSelection(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W26{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function TotallimitHigh(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W27{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function TotallimitUnder(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W28{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function CountLimit(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W29{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function NetZero(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W67{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function TareSetting(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W18{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function InputSelection1(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W33{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function InputSelection2(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W34{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function OutputSelection1(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W35{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function OutputSelection2(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W36{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function ErrorOutputSelection(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W37{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function ReserveOutputSelection(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W38{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function DisplaySelection1(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W65{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    Public Function DisplaySelection2(Model As String, SpoutNo As Integer, Optional Value As String = "") As QueryInfo
        Dim _QueryInfo As New QueryInfo
        Select Case Model
            Case "F800"
            Case "F800E"
            Case "F701S"
                _QueryInfo.Query = New List(Of String) From {
                   $"NO{SpoutNo.ToString().PadLeft(4, "0"c)}W69{Value}{vbCr}"
                       }
                _QueryInfo.ResponseLength = 16
                _QueryInfo.DecimalFormat = 0
                _QueryInfo.ResponseFormat = "0000"
            Case "F701C"
            Case Else
                Return _QueryInfo
        End Select
        Return _QueryInfo
    End Function
    '====================================================================================================
    Function FormatValue(ByVal InputSting As String, Length As Integer, DecimalPoint As Integer) As String
        Dim input As String
        Dim multipliedValue As Integer
        If DecimalPoint = 0 Then
            Return CDbl(InputSting).ToString().PadLeft(5, "0"c)
        ElseIf DecimalPoint = 1 Then
            input = CDbl(InputSting).ToString("0.0")
            multipliedValue = CDbl(input) * 10
        ElseIf DecimalPoint = 2 Then
            input = CDbl(InputSting).ToString("0.00")
            multipliedValue = CDbl(input) * 100
        ElseIf DecimalPoint = 3 Then
            input = CDbl(InputSting).ToString("0.000")
            multipliedValue = CDbl(input) * 1000
        ElseIf DecimalPoint = 4 Then
            input = CDbl(InputSting).ToString("0.0000")
            multipliedValue = CDbl(input) * 1000
        End If
        Return multipliedValue.ToString().PadLeft(Length, "0"c)
    End Function
End Class
Public Class QueryInfo
    Public Query As List(Of String)
    Public ResponseLength As Integer
    Public DecimalFormat As Integer
    Public ResponseFormat As String
    Public MinValue As Double
    Public MaxValue As Double
End Class





