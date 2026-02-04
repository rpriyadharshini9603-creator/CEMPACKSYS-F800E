Public Class frmCleaAccData
    Dim _spout As New SpoutController
    Dim _packer As New Packer
    Dim _Unipulse As New Unipulse

    Private Sub frmCleaAccData_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim I As Integer = CboSpout.SelectedIndex + 1
        Dim j As Integer
        CboPacker.Items.Clear()
        For I = 1 To packerCount
            CboPacker.Items.Add(I)
        Next
        CboPacker.SelectedIndex = 0
        If CboPacker.Text <> "" Then
            j = CboPacker.SelectedIndex + 1
            For I = 1 To spoutCount - 1
                CboSpout.Items.Add(I)
            Next I
            CboSpout.SelectedIndex = 0
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        COMHLTFL(CboSpout.SelectedIndex + 1, CboPacker.SelectedIndex + 1) = True
        If COMHLTFL(CboSpout.SelectedIndex + 1, CboPacker.SelectedIndex + 1) = True Then
            ClearAccData(CboPacker.SelectedIndex + 1)
        End If
    End Sub

    Private Sub ClearAccData(PKR9 As Integer)
        Dim I As Integer
        Dim J As Integer
        J = CboPacker.SelectedIndex + 1
        I = CboSpout.SelectedIndex + 1
        _packer = Packers.FirstOrDefault(Function(e) e.DeviceId = J)
        _spout = _packer.SpoutList.FirstOrDefault(Function(e) e.SpoutId = I)
        Dim SpoutId As Integer = _spout.SpoutId
        Dim PackerId As Integer = _spout.PackerId
        Dim Query As QueryInfo = _Unipulse.Clear(_spout.ControllerModel, SpoutId)
        PackerCommunication.EnqueueDeviceCommand(PackerId, SpoutId, 16, "ClearAccData", Query.Query)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class