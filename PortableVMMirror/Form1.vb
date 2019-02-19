Imports System.ComponentModel
Imports System.IO
Imports System.Management
Public Class Form1
    Dim bgw As New BackgroundWorker
    Private backupDir As String = "" 'Location of backup
    Private USBDevice As String = "" 'Name of the USB device to backup
    Private proc = New Process()

    Public Sub New()
        InitializeComponent()
        AddHandler bgw.DoWork, AddressOf bgw_DoWork
        AddHandler bgw.RunWorkerCompleted, AddressOf bgw_RunWorkerCompleted
        bgw.WorkerSupportsCancellation = True
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NotifyIcon1.Visible = True

        NotifyIcon1.Icon = My.Resources.busy
        NotifyIcon1.Text = "Not monitoring..."
        'Me.Hide()
        ShowInTaskbar = True
        txtUSBDevice.Text = My.Settings.USBDevice
        txtBackup.Text = My.Settings.backupDir
    End Sub
    Private Sub bgw_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        If Not e.Cancelled Then
            'Start the process again to wait for USB drive insert
            bgw.RunWorkerAsync()
        Else
            writeOutput("Stopped")
        End If

    End Sub

    Private Sub bgw_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs)

        Try
            proc = New Process()
            proc.StartInfo = New ProcessStartInfo()
            proc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "ConsoleApplication1.exe"
            proc.StartInfo.UseShellExecute = False
            proc.StartInfo.RedirectStandardOutput = True
            proc.StartInfo.CreateNoWindow = True
            proc.StartInfo.Arguments = "-d " + USBDevice + " -l " + backupDir

            proc.Start()

            While Not proc.StandardOutput.EndOfStream
                ' do something with line
                Dim line As String = proc.StandardOutput.ReadLine()

                writeOutput(line)
            End While

            If proc.ExitCode < 7 Then
                NotifyIcon1.Icon = My.Resources.ok
                NotifyIcon1.Text = "Ok"
                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
                NotifyIcon1.BalloonTipTitle = "Portable VM Mirror App"
                NotifyIcon1.BalloonTipText = "Backup was Successful!"
                NotifyIcon1.ShowBalloonTip(2000)
            Else
                NotifyIcon1.Icon = My.Resources.failed
                NotifyIcon1.Text = "Something went wrong..."
                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Error
                NotifyIcon1.BalloonTipTitle = "Portable VM Mirror App"
                NotifyIcon1.BalloonTipText = "Backup failed!"
                NotifyIcon1.ShowBalloonTip(2000)
                Select Case proc.ExitCode
                    Case 8
                        writeOutput("Some files Or directories could Not be copied And the retry limit was exceeded.")
                    Case 16
                        writeOutput("Robocopy did Not copy any files.  Check the command line parameters And verify that Robocopy has enough rights to write to the destination folder.")
                    Case 998
                        writeOutput("USBDevice or Backup location is not set.")
                    Case 999
                        writeOutput("Console exception was thrown.")
                End Select
                e.Cancel = True
                bgw.CancelAsync()
            End If
        Catch ex As Exception
            writeOutput(ex.Message)
            NotifyIcon1.Icon = My.Resources.failed
            e.Cancel = True
            bgw.CancelAsync()
        End Try
    End Sub
    Private Sub writeOutput(out As String)
        If txtOutput.InvokeRequired Then
            txtOutput.Invoke(Sub() txtOutput.AppendText(out + vbNewLine))
        Else
            txtOutput.AppendText(out + vbNewLine)
        End If
    End Sub
    Private Sub btnBackup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click
        If Not bgw.IsBusy Then
            backupDir = txtBackup.Text
            USBDevice = txtUSBDevice.Text
            bgw.RunWorkerAsync()
        Else
            writeOutput("Worker is busy.")
        End If

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            ShowInTaskbar = False
        End If
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub txtUSBDevice_TextChanged(sender As Object, e As EventArgs) Handles txtUSBDevice.TextChanged
        My.Settings.USBDevice = txtUSBDevice.Text
        My.Settings.Save()
    End Sub

    Private Sub txtBackup_TextChanged(sender As Object, e As EventArgs) Handles txtBackup.TextChanged
        My.Settings.backupDir = txtBackup.Text
        My.Settings.Save()
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            proc.Kill()
        Catch ex As Exception

        End Try
    End Sub
End Class
