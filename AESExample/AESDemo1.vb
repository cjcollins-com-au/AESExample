Imports System.Security.Cryptography
Imports System.Text

Module AESDemo1

    ' Key to use for the encyption and decryption process
    ' If used in production, find a better way to store and reference the keys
    Private Const AesKey256 As String = "1C77D4721927421390D663C670DBBC47"

    ''' <summary>
    ''' Interface / demo process
    ''' </summary>

    Sub ProperEncryptDecrypt()
        Console.Clear()

        Console.WriteLine("Encrypt/Decrypt (using random IV)")

        Console.WriteLine(vbCrLf + "Enter the text to encrypt:")

        Dim plainTwxt As String = Console.ReadLine()

        Dim cipherText As String = Encrypt256(plainTwxt)
        Console.WriteLine(vbCrLf + "The encrypted text (with IV prefix surrounded by *'s) is: " + cipherText)

        Dim decryptedText As String = Decrypt256(cipherText)
        Console.WriteLine(vbCrLf + "The decrypted text is: " + decryptedText)
    End Sub

    ''' <summary>
    ''' Creates 16 char IV from GUID
    ''' </summary>
    ''' <returns>String containing 16 character uniqueish string</returns>

    Function GetNewIV()
        Dim newIV As String = System.Guid.NewGuid.ToString()
        ' the guid generated above will have dashes - remove them...
        newIV = Replace(newIV, "-", "")
        Return Left(newIV, 16)
    End Function

    ''' <summary>
    ''' Encrypts the provided string using AES256 encryption
    ''' </summary>
    ''' <returns>String containing a unique IV, delimited with *, followed by the ciphertext</returns>

    Function Encrypt256(_thestring As String)
        If _thestring = "" Then Return ""

        ' setup for encryption
        Dim AesCrypto As New AesCryptoServiceProvider()
        With AesCrypto
            .BlockSize = 128
            .KeySize = 256
            .Mode = CipherMode.CBC
            .Padding = PaddingMode.PKCS7

            ' set the key
            .Key = Encoding.UTF8.GetBytes(AesKey256)
        End With

        ' get a unique IV
        Dim _iv As String = GetNewIV()
        AesCrypto.IV = Encoding.UTF8.GetBytes(_iv)

        ' encrypt
        Dim SrcText() As Byte
        SrcText = Encoding.Unicode.GetBytes(_thestring)
        Dim Encrypt = AesCrypto.CreateEncryptor()
        Dim DestText() As Byte
        DestText = Encrypt.TransformFinalBlock(SrcText, 0, SrcText.Length)

        ' return the IV (with * delimiters) plus ciphertext
        Return "*" + _iv + "*" + Convert.ToBase64String(DestText)
    End Function

    ''' <summary>
    ''' Decrypts the provided string using AES256
    ''' </summary>
    ''' <param name="_thestring">String to decrypt, consisting of 16 char plaintext IV delimited with *, followed by the ciphertext</param>
    ''' <returns>String containing the decrypted text</returns>

    Function Decrypt256(_thestring As String)
        If _thestring = "" Or IsNothing(_thestring) Or IsDBNull(_thestring) Then Return ""

        ' parse the input string for our IV and ciphertext
        ' assumes the IV will be at the start of the string and 16 chars long (plus the * delimiters)
        Dim _ctext As String = Mid(_thestring, 19)          ' ciphertext portion
        Dim _iv As String = Mid(_thestring, 2, 16)          ' IV without it's * delimiters

        ' setup for decrypt
        Dim AesCrypto As New AesCryptoServiceProvider()

        With AesCrypto
            .BlockSize = 128
            .KeySize = 256
            .Mode = CipherMode.CBC
            .Padding = PaddingMode.PKCS7
            .IV = Encoding.UTF8.GetBytes(_iv)                   ' the IV again
            .Key = Encoding.UTF8.GetBytes(AesKey256)
        End With

        ' decrypt
        Dim SrcText() As Byte
        Dim DestText() As Byte

        Try
            SrcText = System.Convert.FromBase64String(_ctext)   ' just the ciphertext portion of the original string
            Dim Decrypt = AesCrypto.CreateDecryptor()
            DestText = Decrypt.TransformFinalBlock(SrcText, 0, SrcText.Length)

            Return Encoding.Unicode.GetString(DestText)
        Catch
            ' if decryption fails for any reason (incorrect key, incorrect IV or the iv and ciphertext not correctly
            ' split up from the string provided), decryption will fail completely.  You won't get junk results
            ' or some other random value for any reason - the process will fail.
            Return "Cannot Decrypt"
        End Try

    End Function

End Module
