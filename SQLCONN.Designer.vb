<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SQLCONN
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
        Me.txtsqlPwd = New System.Windows.Forms.TextBox()
        Me.txtsqlhost = New System.Windows.Forms.TextBox()
        Me.txtsqlUser = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtsqlDB = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtsqlPwd
        '
        Me.txtsqlPwd.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsqlPwd.Location = New System.Drawing.Point(21, 143)
        Me.txtsqlPwd.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtsqlPwd.Multiline = True
        Me.txtsqlPwd.Name = "txtsqlPwd"
        Me.txtsqlPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtsqlPwd.Size = New System.Drawing.Size(295, 30)
        Me.txtsqlPwd.TabIndex = 80
        Me.txtsqlPwd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtsqlhost
        '
        Me.txtsqlhost.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsqlhost.Location = New System.Drawing.Point(21, 30)
        Me.txtsqlhost.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtsqlhost.Multiline = True
        Me.txtsqlhost.Name = "txtsqlhost"
        Me.txtsqlhost.Size = New System.Drawing.Size(295, 30)
        Me.txtsqlhost.TabIndex = 79
        Me.txtsqlhost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtsqlUser
        '
        Me.txtsqlUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsqlUser.Location = New System.Drawing.Point(21, 87)
        Me.txtsqlUser.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtsqlUser.Multiline = True
        Me.txtsqlUser.Name = "txtsqlUser"
        Me.txtsqlUser.Size = New System.Drawing.Size(295, 30)
        Me.txtsqlUser.TabIndex = 78
        Me.txtsqlUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Gray
        Me.Button2.Font = New System.Drawing.Font("Cambria", 10.8!, System.Drawing.FontStyle.Bold)
        Me.Button2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Button2.Location = New System.Drawing.Point(169, 241)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4, 2, 4, 2)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(121, 26)
        Me.Button2.TabIndex = 77
        Me.Button2.Text = "EXIT"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Gray
        Me.Button1.Font = New System.Drawing.Font("Cambria", 10.8!, System.Drawing.FontStyle.Bold)
        Me.Button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Button1.Location = New System.Drawing.Point(27, 241)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4, 2, 4, 2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(134, 26)
        Me.Button1.TabIndex = 76
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(24, 178)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(125, 18)
        Me.Label4.TabIndex = 75
        Me.Label4.Text = "Database name"
        Me.Label4.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(24, 122)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(83, 18)
        Me.Label3.TabIndex = 74
        Me.Label3.Text = "Password"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(18, 9)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 18)
        Me.Label2.TabIndex = 73
        Me.Label2.Text = " HostName"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(24, 66)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 18)
        Me.Label1.TabIndex = 72
        Me.Label1.Text = "User name"
        '
        'txtsqlDB
        '
        Me.txtsqlDB.Enabled = False
        Me.txtsqlDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsqlDB.Location = New System.Drawing.Point(21, 199)
        Me.txtsqlDB.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtsqlDB.Multiline = True
        Me.txtsqlDB.Name = "txtsqlDB"
        Me.txtsqlDB.Size = New System.Drawing.Size(295, 30)
        Me.txtsqlDB.TabIndex = 81
        Me.txtsqlDB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtsqlDB.Visible = False
        '
        'SQLCONN
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(331, 283)
        Me.Controls.Add(Me.txtsqlDB)
        Me.Controls.Add(Me.txtsqlPwd)
        Me.Controls.Add(Me.txtsqlhost)
        Me.Controls.Add(Me.txtsqlUser)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "SQLCONN"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DB Conn"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtsqlPwd As TextBox
    Friend WithEvents txtsqlhost As TextBox
    Friend WithEvents txtsqlUser As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtsqlDB As TextBox
End Class
