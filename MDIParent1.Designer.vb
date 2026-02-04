<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MDIParent1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.FileMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.CommProperToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GridPropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.WriteCodeNoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeClosePasswordToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShiftTimingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LicenseKeyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigureBatchParameterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigureAdditionalParameterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyBatchParameterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClearAccValuesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AutoCorrectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip
        '
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileMenu, Me.EditMenu, Me.ViewMenu, Me.ToolsMenu})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(632, 24)
        Me.MenuStrip.TabIndex = 9
        Me.MenuStrip.Text = "MenuStrip"
        '
        'FileMenu
        '
        Me.FileMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CommProperToolStripMenuItem, Me.GridPropertiesToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.FileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder
        Me.FileMenu.Name = "FileMenu"
        Me.FileMenu.Size = New System.Drawing.Size(37, 20)
        Me.FileMenu.Text = "&File"
        '
        'CommProperToolStripMenuItem
        '
        Me.CommProperToolStripMenuItem.Name = "CommProperToolStripMenuItem"
        Me.CommProperToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.CommProperToolStripMenuItem.Text = "Communication Proper"
        '
        'GridPropertiesToolStripMenuItem
        '
        Me.GridPropertiesToolStripMenuItem.Name = "GridPropertiesToolStripMenuItem"
        Me.GridPropertiesToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.GridPropertiesToolStripMenuItem.Text = "Grid Properties"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'EditMenu
        '
        Me.EditMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WriteCodeNoToolStripMenuItem, Me.ChangeClosePasswordToolStripMenuItem, Me.ShiftTimingToolStripMenuItem})
        Me.EditMenu.Name = "EditMenu"
        Me.EditMenu.Size = New System.Drawing.Size(39, 20)
        Me.EditMenu.Text = "&Edit"
        '
        'WriteCodeNoToolStripMenuItem
        '
        Me.WriteCodeNoToolStripMenuItem.Name = "WriteCodeNoToolStripMenuItem"
        Me.WriteCodeNoToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.WriteCodeNoToolStripMenuItem.Text = "Write code No"
        '
        'ChangeClosePasswordToolStripMenuItem
        '
        Me.ChangeClosePasswordToolStripMenuItem.Name = "ChangeClosePasswordToolStripMenuItem"
        Me.ChangeClosePasswordToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.ChangeClosePasswordToolStripMenuItem.Text = "Change Close Password"
        '
        'ShiftTimingToolStripMenuItem
        '
        Me.ShiftTimingToolStripMenuItem.Name = "ShiftTimingToolStripMenuItem"
        Me.ShiftTimingToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.ShiftTimingToolStripMenuItem.Text = "Shift Timing"
        '
        'ViewMenu
        '
        Me.ViewMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReportToolStripMenuItem, Me.LicenseKeyToolStripMenuItem})
        Me.ViewMenu.Name = "ViewMenu"
        Me.ViewMenu.Size = New System.Drawing.Size(44, 20)
        Me.ViewMenu.Text = "&View"
        '
        'ReportToolStripMenuItem
        '
        Me.ReportToolStripMenuItem.Name = "ReportToolStripMenuItem"
        Me.ReportToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ReportToolStripMenuItem.Text = "Report "
        '
        'LicenseKeyToolStripMenuItem
        '
        Me.LicenseKeyToolStripMenuItem.Name = "LicenseKeyToolStripMenuItem"
        Me.LicenseKeyToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.LicenseKeyToolStripMenuItem.Text = "License Key"
        '
        'ToolsMenu
        '
        Me.ToolsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConfigureBatchParameterToolStripMenuItem, Me.ConfigureAdditionalParameterToolStripMenuItem, Me.CopyBatchParameterToolStripMenuItem, Me.ClearAccValuesToolStripMenuItem, Me.AutoCorrectionToolStripMenuItem})
        Me.ToolsMenu.Name = "ToolsMenu"
        Me.ToolsMenu.Size = New System.Drawing.Size(61, 20)
        Me.ToolsMenu.Text = "&Settings"
        '
        'ConfigureBatchParameterToolStripMenuItem
        '
        Me.ConfigureBatchParameterToolStripMenuItem.Name = "ConfigureBatchParameterToolStripMenuItem"
        Me.ConfigureBatchParameterToolStripMenuItem.Size = New System.Drawing.Size(242, 22)
        Me.ConfigureBatchParameterToolStripMenuItem.Text = "Configure Batch Parameter"
        '
        'ConfigureAdditionalParameterToolStripMenuItem
        '
        Me.ConfigureAdditionalParameterToolStripMenuItem.Name = "ConfigureAdditionalParameterToolStripMenuItem"
        Me.ConfigureAdditionalParameterToolStripMenuItem.Size = New System.Drawing.Size(242, 22)
        Me.ConfigureAdditionalParameterToolStripMenuItem.Text = "Configure Additional parameter"
        '
        'CopyBatchParameterToolStripMenuItem
        '
        Me.CopyBatchParameterToolStripMenuItem.Name = "CopyBatchParameterToolStripMenuItem"
        Me.CopyBatchParameterToolStripMenuItem.Size = New System.Drawing.Size(242, 22)
        Me.CopyBatchParameterToolStripMenuItem.Text = "Copy Batch Parameter"
        '
        'ClearAccValuesToolStripMenuItem
        '
        Me.ClearAccValuesToolStripMenuItem.Name = "ClearAccValuesToolStripMenuItem"
        Me.ClearAccValuesToolStripMenuItem.Size = New System.Drawing.Size(242, 22)
        Me.ClearAccValuesToolStripMenuItem.Text = "Clear Acc Values"
        '
        'AutoCorrectionToolStripMenuItem
        '
        Me.AutoCorrectionToolStripMenuItem.Name = "AutoCorrectionToolStripMenuItem"
        Me.AutoCorrectionToolStripMenuItem.Size = New System.Drawing.Size(242, 22)
        Me.AutoCorrectionToolStripMenuItem.Text = "Auto Correction"
        '
        'MDIParent1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(632, 453)
        Me.Controls.Add(Me.MenuStrip)
        Me.IsMdiContainer = True
        Me.Name = "MDIParent1"
        Me.Text = "Packer MIS Software"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Timer2 As Timer
    Friend WithEvents Timer3 As Timer
    Friend WithEvents MenuStrip As MenuStrip
    Friend WithEvents FileMenu As ToolStripMenuItem
    Friend WithEvents CommProperToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditMenu As ToolStripMenuItem
    Friend WithEvents WriteCodeNoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChangeClosePasswordToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShiftTimingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewMenu As ToolStripMenuItem
    Friend WithEvents ReportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolsMenu As ToolStripMenuItem
    Friend WithEvents ConfigureBatchParameterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConfigureAdditionalParameterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyBatchParameterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ClearAccValuesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AutoCorrectionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GridPropertiesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LicenseKeyToolStripMenuItem As ToolStripMenuItem
End Class
