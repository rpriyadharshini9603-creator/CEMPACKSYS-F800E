Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Module LicenseManager
    Private licenseFilePath As String = Path.Combine(Application.StartupPath, "license.dat")
    Private usedKeysPath As String = Path.Combine(Application.StartupPath, "Base.dat")
    Private secretKey As String = "Accsys!Secret@Key#2025"
    Private saltBytes As Byte() = Encoding.UTF8.GetBytes("Salt$For#License2025")
    Private licenseFilePath2 As String = Path.Combine(Application.StartupPath, "Dummy.dat")
    Private licenseFilePath3 As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MIS", "MIS.dat")

    Public Sub ActivateLicense(encodedKey As String)
        If String.IsNullOrWhiteSpace(encodedKey) Then
            MessageBox.Show("No license key provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        'Decode key using AES
        Dim key As String
        Try
            key = DecodeKey(encodedKey)
        Catch ex As Exception
            MessageBox.Show("Invalid or unreadable encrypted license key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

        ' Prevent reuse of the same key
        If IsKeyUsed(encodedKey) Then
            MessageBox.Show("This license key has already been used!", "Duplicate Key", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Validate
        Dim daysValid As Integer = ParseDaysFromKey(key)
        If daysValid <= 0 OrElse Not ValidateKeyFormat(key) Then
            MessageBox.Show("Invalid license key content.", "Invalid Key", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Dim licenseFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MIS")
        If Not Directory.Exists(licenseFolder) Then
            Directory.CreateDirectory(licenseFolder)
        End If

        ' Encrypt and store activation data
        Dim activationDate As Date = Date.Today
        Dim expiryDate As Date = activationDate.AddDays(daysValid)
        Dim plainData As String = $"{key}|{activationDate:yyyy-MM-dd}|{expiryDate:yyyy-MM-dd}"
        Dim encryptedData As String = EncryptString(plainData, secretKey)
        File.WriteAllText(licenseFilePath, encryptedData)
        File.WriteAllText(licenseFilePath2, encryptedData)
        File.WriteAllText(licenseFilePath3, encryptedData)
        MarkKeyAsUsed(encodedKey)
        MessageBox.Show($"License activated successfully! Valid till {expiryDate:yyyy-MM-dd}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Public Function CheckValidity()
        If Not File.Exists(licenseFilePath) Or Not File.Exists(licenseFilePath2) Or Not File.Exists(licenseFilePath3) Then
            MessageBox.Show("No license found! Please activate.", "License Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Try
            Dim encryptedData As String = File.ReadAllText(licenseFilePath)
            Dim plainData As String = DecryptData(encryptedData, secretKey)
            Dim parts() As String = plainData.Split("|"c)
            Dim expiryDate As Date = Date.Parse(parts(2))
            Dim daysLeft As Integer = (expiryDate - Date.Today).Days

            Return daysLeft
        Catch
            MessageBox.Show("License file is corrupted or unreadable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function CheckLicense() As Boolean
        If Not File.Exists(licenseFilePath) Or Not File.Exists(licenseFilePath2) Or Not File.Exists(licenseFilePath3) Then
            MessageBox.Show("No license found! Please activate.", "License Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Try
            Dim encryptedData As String = File.ReadAllText(licenseFilePath)
            Dim plainData As String = DecryptData(encryptedData, secretKey)
            Dim parts() As String = plainData.Split("|"c)
            If parts.Length < 3 Then
                MessageBox.Show("License file invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Dim expiryDate As Date = Date.Parse(parts(2))
            Dim daysLeft As Integer = (expiryDate - Date.Today).Days

            If Date.Today > expiryDate Then
                MessageBox.Show("Your license has expired! Please contact support.", "License Expired", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            ElseIf daysLeft <= 5 Then
                MessageBox.Show($"Your license will expire in {daysLeft} day(s)!", "License Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            Return True
        Catch
            MessageBox.Show("License file is corrupted or unreadable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Function DecodeKey(encoded As String) As String
        Return DecryptString(encoded, secretKey)
    End Function

    Private Function ParseDaysFromKey(key As String) As Integer
        If String.IsNullOrWhiteSpace(key) Then Return 0

        Dim periodPart As String = ""
        Dim len As Integer = key.Length

        If len = 12 Then
            periodPart = key.Substring(len - 2)  'last 3 Chars
        ElseIf len = 13 Then
            periodPart = key.Substring(len - 4)  ' last 3 chars
        ElseIf len = 14 Then
            periodPart = key.Substring(len - 4)  ' last 4 chars
        ElseIf len > 14 Then
            periodPart = key.Substring(len - 5)
        End If

        Dim m As Match = Regex.Match(periodPart, "(\d+)([YMD])$", RegexOptions.IgnoreCase)
        If Not m.Success Then Return 0
        Dim num As Integer
        If Not Integer.TryParse(m.Groups(1).Value, num) Then Return 0

        Select Case m.Groups(2).Value.ToUpperInvariant()
            Case "Y"
                Return num * 365
            Case "M"
                Return num * 30
            Case "D"
                Return num
            Case Else
                Return 0
        End Select
    End Function
    Private Function ValidateKeyFormat(key As String) As Boolean
        Return key.StartsWith("ACCSYS", StringComparison.OrdinalIgnoreCase) AndAlso
               Regex.IsMatch(key, "\d+[YMD]$", RegexOptions.IgnoreCase)
    End Function
    ' AES ENCRYPTION
    Public Function EncryptString(plainText As String, password As String) As String
        Dim key As Byte() = New Rfc2898DeriveBytes(password, saltBytes).GetBytes(32)
        Using aes As Aes = Aes.Create()
            aes.Key = key
            aes.GenerateIV()
            Using ms As New MemoryStream()
                ms.Write(aes.IV, 0, aes.IV.Length)
                Using cs As New CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write)
                    Dim plainBytes As Byte() = Encoding.UTF8.GetBytes(plainText)
                    cs.Write(plainBytes, 0, plainBytes.Length)
                    cs.FlushFinalBlock()
                End Using
                Return Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
    End Function
    ' AES DECRYPTION
    Private Function DecryptData(cipherText As String, password As String) As String
        Dim fullCipher As Byte() = Convert.FromBase64String(cipherText)
        Dim key As Byte() = New Rfc2898DeriveBytes(password, saltBytes).GetBytes(32)
        Using aes As Aes = Aes.Create()
            Dim iv(15) As Byte
            Array.Copy(fullCipher, 0, iv, 0, iv.Length)
            aes.Key = key
            aes.IV = iv
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(fullCipher, iv.Length, fullCipher.Length - iv.Length)
                    cs.FlushFinalBlock()
                End Using
                Return Encoding.UTF8.GetString(ms.ToArray())
            End Using
        End Using
    End Function
    Public Function GetMacAddress() As String
        Dim MacId As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
        For Each Mac As NetworkInterface In MacId
            If Mac.OperationalStatus = OperationalStatus.Up AndAlso Mac.NetworkInterfaceType <> NetworkInterfaceType.Loopback Then
                Return Mac.GetPhysicalAddress().ToString()
            End If
        Next
        Return String.Empty
    End Function
    Private Function DecryptString(cipherText As String, password As String) As String
        Dim fullCipher As Byte() = Convert.FromBase64String(cipherText)
        Dim key As Byte() = New Rfc2898DeriveBytes(password, saltBytes).GetBytes(32)
        Using aes As Aes = Aes.Create()
            Dim iv(15) As Byte
            Array.Copy(fullCipher, 0, iv, 0, iv.Length)
            aes.Key = key
            aes.IV = iv

            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(fullCipher, iv.Length, fullCipher.Length - iv.Length)
                    cs.FlushFinalBlock()
                End Using

                Dim decryptedText As String = Encoding.UTF8.GetString(ms.ToArray())
                ' Decrypted text should be: "MAC|ActualData"
                Dim parts() As String = decryptedText.Split("|"c)
                If parts.Length < 2 Then Return Nothing

                Dim storedMac As String = parts(0)
                Dim actualMac As String = GetMacAddress() ' current PC MAC
                If storedMac <> actualMac Then
                    Throw New Exception("This data is not for this PC.")
                End If

                Return parts(1)
            End Using
        End Using
    End Function

    ' Duplcate Key Tracking 
    Private Function IsKeyUsed(encryptedKey As String) As Boolean
        If Not File.Exists(usedKeysPath) Then Return False
        Dim usedKeys = File.ReadAllLines(usedKeysPath)
        Return usedKeys.Contains(encryptedKey.Trim())
    End Function

    Private Sub MarkKeyAsUsed(encryptedKey As String)
        File.AppendAllText(usedKeysPath, encryptedKey.Trim() & Environment.NewLine)
    End Sub
End Module
