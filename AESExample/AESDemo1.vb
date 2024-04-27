Imports System.Security.Cryptography
Imports System.Text

Module AESDemo1

    Private Const AesKey256 As String = "1C77D4721927421390D663C670DBBC47"

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
        Dim _ctext As String = Mid(_thestring, 19)
        Dim _iv As String = Mid(_thestring, 2, 16)

        ' setup for decrypt
        Dim AesCrypto As New AesCryptoServiceProvider()

        With AesCrypto
            .BlockSize = 128
            .KeySize = 256
            .Mode = CipherMode.CBC
            .Padding = PaddingMode.PKCS7
            .IV = Encoding.UTF8.GetBytes(_iv)
            .Key = Encoding.UTF8.GetBytes(AesKey256)
        End With

        ' decrypt
        Dim SrcText() As Byte
        Dim DestText() As Byte

        Try
            SrcText = System.Convert.FromBase64String(_ctext)
            Dim Decrypt = AesCrypto.CreateDecryptor()
            DestText = Decrypt.TransformFinalBlock(SrcText, 0, SrcText.Length)

            Return Encoding.Unicode.GetString(DestText)
        Catch
            Return "Cannot Decrypt"
        End Try

    End Function

End Module
