Imports System.Security.Cryptography
Imports System.Text

Module AESDemo2FixedIV

    ' Key to use for the encyption and decryption process
    ' If used in production, find a better way to store and reference the keys
    Private Const AesKey256 As String = "1C77D4721927421390D663C670DBBC47"

    ' fixed IV.  
    Private Const AesIV128 As String = "572CD4703ACA482F"       ' ...don't do this... example only 

    Sub EncryptDecryptWithFixedIV()
        Console.Clear()

        Console.WriteLine("Encrypt/Decrypt (using same IV)")

        Console.WriteLine(vbCrLf + "Enter the text to encrypt:")

        Dim plainTwxt As String = Trim(Console.ReadLine())

        Dim cipherText As String = Encrypt256(plainTwxt)
        Console.WriteLine(vbCrLf + "The encrypted text is: " + cipherText)

        Dim decryptedText As String = Decrypt256(cipherText)
        Console.WriteLine(vbCrLf + "The decrypted text is: " + decryptedText)

        Console.WriteLine(vbCrLf + vbCrLf + "(The IV used was: " + AesIV128 + ")")
    End Sub

    ''' <summary>
    ''' Encrypts the provided string using AES256 encryption
    ''' </summary>
    ''' <returns>Ciphertext only (base 64)</returns>

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

            ' set thee IV
            .IV = Encoding.UTF8.GetBytes(AesIV128)
        End With

        ' encrypt
        Dim SrcText() As Byte
        SrcText = Encoding.Unicode.GetBytes(_thestring)
        Dim Encrypt = AesCrypto.CreateEncryptor()
        Dim DestText() As Byte
        DestText = Encrypt.TransformFinalBlock(SrcText, 0, SrcText.Length)

        ' return the ciphertext only
        Return Convert.ToBase64String(DestText)
    End Function

    ''' <summary>
    ''' Decrypts the provided string using AES256
    ''' </summary>
    ''' <param name="_thestring">String to decrypt, consisting of ciphertext only</param>
    ''' <returns>String containing the decrypted text</returns>

    Function Decrypt256(_thestring As String)
        If _thestring = "" Or IsNothing(_thestring) Or IsDBNull(_thestring) Then Return ""

        ' parse the input string for our IV and ciphertext
        Dim _ctext As String = _thestring

        ' setup for decrypt
        Dim AesCrypto As New AesCryptoServiceProvider()

        With AesCrypto
            .BlockSize = 128
            .KeySize = 256
            .Mode = CipherMode.CBC
            .Padding = PaddingMode.PKCS7
            .IV = Encoding.UTF8.GetBytes(AesIV128)
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
