Imports System.ComponentModel
Imports System.IO
Imports System.Management
Imports System.Windows.Forms


Module Module1

    Public Enum EventType
        Inserted = 2
        Removed = 3
    End Enum
    Public Enum OutputType
        INFO
        WARNING
        [ERROR]
    End Enum

    Private backupDir As String = "" 'Location of backup
    Private USBDevice As String = "" 'Name of the USB device to backup
    Private exitCode As Integer = 0
    Public Sub printString(text As String, Optional type As OutputType = OutputType.INFO)

        Dim originalColor As ConsoleColor = Console.ForegroundColor
        Select Case type
            Case OutputType.INFO
                Console.ForegroundColor = ConsoleColor.White
            Case OutputType.WARNING
                Console.ForegroundColor = ConsoleColor.Yellow
            Case OutputType.ERROR
                Console.ForegroundColor = ConsoleColor.Red
        End Select

        If text.ToLower.Contains("success") Then
            Console.ForegroundColor = ConsoleColor.Green
        End If

        If text <> "" Then

            Console.WriteLine(Date.Now.ToString("dd MMM HH:mm:ss") + " [" + type.ToString + "] " + text)
        Else
            Console.WriteLine(text)
        End If

        Console.ForegroundColor = originalColor
    End Sub

    Private Function GetDriveName(letter As String) As String
        letter = letter + "\"
        For Each drive As DriveInfo In My.Computer.FileSystem.Drives
            If drive.DriveType = DriveType.Removable AndAlso drive.Name.ToLower = letter.ToLower Then
                Return drive.VolumeLabel
            End If
        Next
        Return "Unknown Device"
    End Function
    Private Sub DeviceInsertedEvent(sender As Object, e As EventArrivedEventArgs)

        Try
            Dim driveName As String = e.NewEvent.Properties("DriveName").Value.ToString()
            Dim eventType As EventType = e.NewEvent.Properties("EventType").Value

            Dim eventName As String = [Enum].GetName(GetType(EventType), eventType)
            Dim deviceName As String = GetDriveName(driveName)

            printString(deviceName + " " + eventName)

            If deviceName = USBDevice Then
                'run backup
                exitCode = startRoboCopy(driveName)
                Environment.Exit(exitCode)
            End If
        Catch ex As Exception
            printString(ex.Message, OutputType.ERROR)
            Environment.Exit(999)
        End Try


    End Sub
    Private Function startRoboCopy(driveLetter As String) As Integer
        Try
            Dim proc = New Process()
            proc.StartInfo = New ProcessStartInfo()
            proc.StartInfo.FileName = "C:\Windows\SysWow64\Robocopy.exe"
            proc.StartInfo.Arguments = driveLetter + "\ " + backupDir + " /e /mir "
            proc.StartInfo.UseShellExecute = False
            proc.StartInfo.RedirectStandardOutput = True
            proc.StartInfo.CreateNoWindow = True

            proc.Start()
            While Not proc.StandardOutput.EndOfStream
                ' do something with line
                Dim line As String = proc.StandardOutput.ReadLine()
                Console.WriteLine(line)
            End While

            If proc.ExitCode < 7 Then
                printString("Backup completed successfully!")
                Return proc.ExitCode
            Else
                Select Case proc.ExitCode
                    Case 8
                        printString("Some files Or directories could Not be copied And the retry limit was exceeded.", OutputType.ERROR)
                    Case 16
                        printString("Robocopy did Not copy any files.  Check the command line parameters And verify that Robocopy has enough rights to write to the destination folder.", OutputType.ERROR)
                End Select
                Return proc.ExitCode
            End If
        Catch ex As Exception
            printString(ex.Message, OutputType.ERROR)
            Return 999
        End Try
    End Function

    Private Sub DeviceRemovedEvent(sender As Object, e As EventArrivedEventArgs)
        printString("Device event listener started.")
        Try
            Dim instance As ManagementBaseObject = DirectCast(e.NewEvent("TargetInstance"), ManagementBaseObject)
            For Each [property] In instance.Properties
                Console.WriteLine([property].Name + " = " + [property].Value)
            Next
        Catch ex As Exception
            printString(ex.Message, OutputType.ERROR)
            Environment.Exit(999)
        End Try

    End Sub

    Sub Main()
        Dim args As String() = Environment.GetCommandLineArgs()

        If Not args.Contains("-d") Or Not args.Contains("-l") Then
            Environment.Exit(998)
        End If

        For i As Integer = 0 To args.Length - 1
            If args(i) = "-d" Then
                USBDevice = args(i + 1)
            ElseIf args(i) = "-l" Then
                backupDir = args(i + 1)
            End If
        Next

        If USBDevice = "" Or USBDevice = "-l" Or backupDir = "" Then
            Environment.Exit(998)
        End If

        Dim watcher As New ManagementEventWatcher()
        Dim query As New WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3")
        AddHandler watcher.EventArrived, AddressOf DeviceInsertedEvent
        watcher.Query = query
        watcher.Start()
        printString("Device Insert Event listener started.")

        Console.ReadKey()
    End Sub

End Module
