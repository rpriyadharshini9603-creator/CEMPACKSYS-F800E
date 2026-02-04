Public Class LogForm
    Private ReadOnly _deviceListBox As New Dictionary(Of String, ListBox)()
    Private Sub LogForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _deviceListBox.Clear()
        CreateUI()
    End Sub
    Private Sub CreateUI()
        Const groupBoxWidth As Integer = 300   ' Width of each GroupBox
        Const groupBoxHeight As Integer = 300  ' Height of each GroupBox
        Dim ClientWidth As Integer = Me.ClientSize.Width - 50
        Dim ClientHeight As Integer = Me.ClientSize.Height - 80
        Dim MaxHorizontalTile As Integer = (ClientWidth \ groupBoxWidth)
        Dim MaxVertialTile As Integer = (ClientHeight \ groupBoxHeight)

        Dim TabControl As New TabControl With {
            .Name = "Packer",
            .Text = "Packer",
            .Width = ClientWidth,
            .Height = ClientHeight,
            .Margin = New Padding(20, 20, 20, 20),
            .Font = New Font("Arial", 13, FontStyle.Bold),
            .BackColor = Color.LightGray,
            .Location = New Point(20, 20)
        }
        For Each Packer As Packer In Packers
            Dim PackerTab As New TabPage($"Packer{Packer.DeviceId}")
            TabControl.TabPages.Add(PackerTab)
            TabControl.SelectedTab = PackerTab
            PackerTab.Font = New Font("Arial", 9, FontStyle.Regular)
            PackerTab.BackColor = Color.LightGray
            PackerTab.Tag = Packer.DeviceId
            PackerTab.AutoScroll = True
            PackerTab.Location = New Point(10, 100)
            AddTabControlsItems(TabControl, Packer.DeviceId)
            TabControl.SelectedTab = PackerTab
        Next
        Me.Controls.Add(TabControl)
        Timer1.Interval = 3000
        Timer1.Enabled = True
    End Sub
    Private Sub AddTabControlsItems(TabControl As TabControl, PackerId As Integer)
        Dim ClientWidth As Integer = Me.ClientSize.Width
        Dim ClientHeight As Integer = Me.ClientSize.Height

        Dim ErrorLabel As New Label() With {
                         .Text = "Error Logs",
                        .Location = New Point(10, 25),
                        .AutoSize = True,
                        .ForeColor = Color.Black
                     }
        Dim ErrorListBox As New ListBox With {
            .Name = "ErrorListBox" & PackerId.ToString(),
            .Text = "0",
            .Top = ErrorLabel.Top + 20,
            .Left = ErrorLabel.Left,
            .AutoSize = True,
            .Width = TabControl.Width - 50,
            .Height = TabControl.Height - 120,
            .Padding = New Padding(10, 10, 10, 10)
        }
        _deviceListBox.Add(ErrorListBox.Name, ErrorListBox)
        Dim ClearButton As New Button() With {
                                .Name = $"ClearButton{PackerId}",
                                .Text = $"Clear-{PackerId}",
                                .Tag = $"ErrorListBox{PackerId}",
                                .Width = 50,
                                .AutoSize = True,
                                .BackColor = Color.Red,
                                .ForeColor = Color.White
                             }
        AddHandler ClearButton.Click, AddressOf HandleClearButton
        ClearButton.Left = ErrorListBox.Left + ErrorListBox.Width - ClearButton.Width
        ClearButton.Top = TabControl.Height - 80
        TabControl.SelectedTab?.Controls.Add(ErrorLabel)
        TabControl.SelectedTab?.Controls.Add(ErrorListBox)
        TabControl.SelectedTab?.Controls.Add(ClearButton)
    End Sub
    Private Sub HandleClearButton(sender As Object, e As EventArgs)
        Dim clearButton = TryCast(sender, Button)
        Dim _listbox As ListBox = Nothing
        If _deviceListBox.TryGetValue(clearButton.Tag, _listbox) Then
            _listbox.Items.Clear()
        End If
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        For Each _Packer As Packer In Packers
            Dim _listbox As ListBox = Nothing
            If _deviceListBox.TryGetValue($"ErrorListBox{_Packer.DeviceId}", _listbox) Then
                _listbox.Items.Clear()
                For Each ErrorStr As String In _Packer.ErrorLog
                    _listbox.Items.Add(ErrorStr)
                Next
            End If
        Next
        Timer1.Enabled = True
    End Sub
End Class