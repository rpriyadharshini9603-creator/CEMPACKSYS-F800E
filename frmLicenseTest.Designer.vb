<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLicenseTest
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnActivate = New System.Windows.Forms.Button()
        Me.btnCheck = New System.Windows.Forms.Button()
        Me.txtKey = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnActivate
        '
        Me.btnActivate.Location = New System.Drawing.Point(101, 181)
        Me.btnActivate.Name = "btnActivate"
        Me.btnActivate.Size = New System.Drawing.Size(99, 23)
        Me.btnActivate.TabIndex = 0
        Me.btnActivate.Text = "Activate License"
        Me.btnActivate.UseVisualStyleBackColor = True
        '
        'btnCheck
        '
        Me.btnCheck.Location = New System.Drawing.Point(291, 181)
        Me.btnCheck.Name = "btnCheck"
        Me.btnCheck.Size = New System.Drawing.Size(110, 23)
        Me.btnCheck.TabIndex = 1
        Me.btnCheck.Text = "Check Activation"
        Me.btnCheck.UseVisualStyleBackColor = True
        '
        'txtKey
        '
        Me.txtKey.Location = New System.Drawing.Point(12, 89)
        Me.txtKey.Multiline = True
        Me.txtKey.Name = "txtKey"
        Me.txtKey.Size = New System.Drawing.Size(503, 67)
        Me.txtKey.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(193, 43)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(125, 23)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Get Activation Key"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'frmLicenseTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(537, 236)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtKey)
        Me.Controls.Add(Me.btnCheck)
        Me.Controls.Add(Me.btnActivate)
        Me.Name = "frmLicenseTest"
        Me.Text = "frmLicenseTest"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnActivate As Button
    Friend WithEvents btnCheck As Button
    Friend WithEvents txtKey As TextBox
    Friend WithEvents Button1 As Button
End Class
