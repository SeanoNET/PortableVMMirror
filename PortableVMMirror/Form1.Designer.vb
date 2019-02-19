<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Me.btnBackup = New System.Windows.Forms.Button()
        Me.txtOutput = New System.Windows.Forms.TextBox()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtUSBDevice = New System.Windows.Forms.TextBox()
        Me.txtBackup = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnBackup
        '
        Me.btnBackup.Location = New System.Drawing.Point(611, 402)
        Me.btnBackup.Name = "btnBackup"
        Me.btnBackup.Size = New System.Drawing.Size(78, 23)
        Me.btnBackup.TabIndex = 0
        Me.btnBackup.Text = "Start Listener"
        Me.btnBackup.UseVisualStyleBackColor = True
        '
        'txtOutput
        '
        Me.txtOutput.Location = New System.Drawing.Point(12, 12)
        Me.txtOutput.Multiline = True
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtOutput.Size = New System.Drawing.Size(677, 384)
        Me.txtOutput.TabIndex = 1
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Visible = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 402)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "USB Device:"
        '
        'txtUSBDevice
        '
        Me.txtUSBDevice.Location = New System.Drawing.Point(87, 400)
        Me.txtUSBDevice.Name = "txtUSBDevice"
        Me.txtUSBDevice.Size = New System.Drawing.Size(179, 20)
        Me.txtUSBDevice.TabIndex = 3
        '
        'txtBackup
        '
        Me.txtBackup.Location = New System.Drawing.Point(341, 400)
        Me.txtBackup.Name = "txtBackup"
        Me.txtBackup.Size = New System.Drawing.Size(179, 20)
        Me.txtBackup.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(272, 403)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Backup To:"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(701, 432)
        Me.Controls.Add(Me.txtBackup)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtUSBDevice)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtOutput)
        Me.Controls.Add(Me.btnBackup)
        Me.Name = "Form1"
        Me.Text = "Portable VM Mirror"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnBackup As Button
    Friend WithEvents txtOutput As TextBox
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents Label1 As Label
    Friend WithEvents txtUSBDevice As TextBox
    Friend WithEvents txtBackup As TextBox
    Friend WithEvents Label2 As Label
End Class
