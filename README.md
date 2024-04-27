# AES Encryption in VB.net

This program runs in the terminal window within Visual Studio (not VS Code, use the full or community edition of Visual Studio).

It is intended to demonstrate a typical implementation of AES cryptography and demonstrate to the user how this works with a random 
initialisation vector (recommended).  It is also shown with a fixed initialisation vector to demonstrate how this works.  More notes 
on this below.

The code can be copied for your own use (see notes below though).


## Requirements

(Windows Only).  Visual Studio Community Edition or better.  Not VS Code.  

Download all files


## Usage

Run in debug mode within VS. 

A terminal window will be displayed and prompt you for which approach you want to take 
(with random IV or fixed IV).  

It will then prompt you for the text to encrypt, and will display the encryption and decryption 
results.


## Role of the IV (Initialisation Vector) in AES Encryption

The role of the IV/Initialisation Vector is to provide an initial scrambling of the plaintext prior to encryption.  

This means that if two identical texts are encrypted using the same key, the IV (if random) ensures that the encrypted results are different.

This is desired as it means that if (for example) a password file with AES encyrypted passwords (*see note below*) was obtained, occurences of a 
commonly used password such as Password123 would be indistinguishable from each other in the ciphertext.

To facilitate this, the IV is typically stored with the ciphertext and separated from the ciphertext prior to 
decryption, to then be used in the decryption process itself.  

**The IV does not need to be secret**.  It is the key's job to be a key and remain secret; the IV is not a key.  

You don't want any of the data to get out of course, but as long as the key itself remains secure, AES is sufficiently 
robust that decryption is not computationally feasible if the IV - assuming it is random - is leaked with the ciphertext.  This satisfies 
Kerckhoffs's principle also.  

*Note - don't use AES encryption for passwords.  This is just an example.  Passwords should be hashed, not encypted.*

## Code Notes

The program AESDemo1.vb is the better example of the two programs provided as it has the random IV.  The functions here can be copied for your own use. Comments are 
included to explain the usage and process.

## Note - Block Modes

*The code samples use AES in CBC (Cipher Block Chaining) mode which is no longer recommended.  Although arguably still very secure, there are 
some known issues with CBC that mean it's days are numbered.  Use one of the other available block modes except for CBC and don't use ECB either.  (ECB itself is bad).*

