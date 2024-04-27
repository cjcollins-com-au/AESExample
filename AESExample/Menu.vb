Module Menu

    Sub Main()

        Do While True

            Console.Clear()

            Console.WriteLine("Select from the following:" + vbCrLf)

            Console.WriteLine("[1] Regular Encryption/Decryption Demo")
            Console.WriteLine("[2] Encryption/Decryption With Fixed IV")

            Console.WriteLine(vbCrLf)
            Console.WriteLine("The regular encryption option (1) uses AES encryption and a different IV each time.")
            Console.WriteLine("The 'Fixed IV' encryption option (2) uses AES encryption but the same IV each time (deterministic/same result).")

            Console.WriteLine(vbCrLf)
            Console.WriteLine("Please make your selection: ")
            Dim selection As String = Console.ReadLine()

            Select Case selection
                Case = "1"
                    ProperEncryptDecrypt()
                Case = "2"
                    EncryptDecryptWithFixedIV()
                Case Else
                    Console.WriteLine(vbCrLf + "Error - Invalid Selection")
            End Select

            Console.WriteLine(vbCrLf + "Press <Enter> to go again, or type Q then press <Enter> to quit...")
            Dim response = Console.ReadLine()
            If UCase(response) = "Q" Then
                Exit Do
            End If
        Loop

    End Sub

End Module
